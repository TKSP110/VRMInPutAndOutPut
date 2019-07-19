using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Runtime.InteropServices;
#if VRMIO
using UniRx.Async;
using SimpleFileBrowser;
using VRMIO;
#endif
#if UNITY_WEBGL && !UNITY_EDITOR
using UnityEngine.Networking;
#endif


//シングルトン
public class VRMIOImporter : SingletonMonoBehaviourFast<VRMIOImporter>
{
#if VRMIO
    [DllImport("__Internal")]
    private static extern void ImportVRM();

    //実行中かどうか
    private bool IsDoingFuncNow = false;

    private Canvas recommendClickCanvas;

    //コールバック格納変数
    private UnityAction<byte[]> OnSuccessCallBack;
    private UnityAction OnFailedCallBack;

    //コライダー用
    private VRMCollider vrmCollider;

    protected override void Awake()
    {
        base.Awake();
        vrmCollider = GetComponent<VRMCollider>();
        DontDestroyOnLoad(this.gameObject);
    }

    //メインの関数(第二引数にデータが入る関数)
    public void ImportVrmData(Canvas recommendClickCanvas = null, UnityAction<byte[]> successCallBack = null, UnityAction failedCallBack = null)
    {
        //読み込み関数が進行中なら処理が被るので抜け出す
        if (IsDoingFuncNow) return;
        IsDoingFuncNow = true;

        this.recommendClickCanvas = recommendClickCanvas;

        //コールバック設定
        this.OnSuccessCallBack = successCallBack;
        this.OnFailedCallBack = failedCallBack;


        //WEBGLならプラグイン、エディタならSimpleFileBrowserアセットを使用
#if UNITY_WEBGL && !UNITY_EDITOR
        VRMLoadWebGL().Forget();
#else
        // フィルタを設定します
        FileBrowser.SetFilters
        (
            false,
            new FileBrowser.Filter("VRM", ".vrm")
        );

        // ダイアログが表示されたときに選択されるデフォルトフィルタを設定します
        FileBrowser.SetDefaultFilter(".vrm");

        // /除外する拡張子を設定します
        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe", ".jpg", ".png", ".txt", ".pdf");

        //コルーチンでロード
        StartCoroutine(ShowLoadDialogCoroutine());
#endif
    }



    /// <summary>
    /// WebGLの場合
    /// </summary>

    //プラグインで3つの項目を読み込んだか監視する
    bool IsLoadURL = false;
    bool IsLoadName = false;
    bool IsLoadDataSize = false;
    //読み込んだデータが格納される
    string URL;
    string FileName;
    int FileSize;


    private async UniTask VRMLoadWebGL()
    {
        //初期化
        IsLoadURL = false;
        IsLoadName = false;
        IsLoadDataSize = false;
        URL = null;
        FileName = null;
        FileSize = 0;

        //もう一度クリックすることを促す(Canvasがあれば)
        //ボタン押下後にマウスがウィンドウから外れるとダイアログが出ないため(Canvasが無ければ)
        if (recommendClickCanvas != null)
        {
            recommendClickCanvas.enabled = true;
            //1Flame待つ
            await UniTask.DelayFrame(1);
        }
        //プラグインを呼ぶ(Unityのフォーカスが外れ、コールバック待ちとなる)
        ImportVRM();
    }



    //選択ダイアログを開くと一度だけ呼ばれるコールバック(プラグインから呼ばれる)
    public void ClickCallBackMessageFromPlugin()
    {
        //キャンセル時に読み込み不可にならないようにボタン押下許可
        IsDoingFuncNow = false;
        //キャンバスを表示
        recommendClickCanvas.enabled = false;
    }

    //ファイルが選択された後に一度だけ呼ばれる。ファイルの場所を受け取るプラグインから呼ばれるコールバック
    public void URLCallBackMessageFromPlugin(string url)
    {
        IsLoadURL = true;
        URL = url;
    }
    //ファイルが選択された後に一度だけ呼ばれる。ファイルの名称を受け取るプラグインから呼ばれるコールバック
    public void NameCallBackFromPlugin(string name)
    {
        IsLoadName = true;
        FileName = name;
    }
    //ファイルが選択された後に一度だけ呼ばれる。ファイルのサイズを受け取るプラグインから呼ばれるコールバック
    public void SizeCallBackFromPlugin(string size)
    {
        IsLoadDataSize = true;
        FileSize = int.Parse(size.Remove(size.Length - 4, 4));
    }

    //ファイルが選択された後に一度だけ呼ばれる。全てのデータを取得するまで待つ
    public async UniTask SelectedCallBackFromPlugin()
    {
        bool success = false;
        float timeout = 10.0f;

        while (timeout > 0)
        {
            //全てのデータを取得するかタイムアウトまで1flame待つ無限ループ
            if (IsLoadURL && IsLoadName && IsLoadDataSize)
            {
                success = true;
                break;
            }
            await UniTask.DelayFrame(1);
            timeout -= Time.deltaTime;
        }

        if (success && URL != null)
        {
            //filenameの最後4文字が.vrmでなければ不正
            if (FileName.Substring((FileName.Length - 4), 4) == ".vrm")
            {
                //Jsonを読み込む
                StartCoroutine(LoadFileWebGL(URL));
            }
            else
            {
                //VRM拡張子じゃ無い
                OnError();
            }
        }
        else
        {
            //データなし
            OnError();
        }
    }

    //データをを読み込む
    private IEnumerator LoadFileWebGL(string url)
    {
        WWW www = new WWW(url);

        // データの取得
        while (!www.isDone)
        {
            yield return null;
        }
        if (www.error == null)
        {
            //読み込み完了
            OnSuccess(www.bytes);
        }
        else
        {
            //読み込み失敗
            OnError();
        }
    }


    /// <summary>
    /// WebGL以外の場合
    /// </summary>

    private IEnumerator ShowLoadDialogCoroutine()
    {
        // ファイル読み込みダイアログを表示してユーザーからの応答を待ちます
        yield return FileBrowser.WaitForLoadDialog(false, null, "Load File", "Load");

        if (FileBrowser.Success)
        {
            //ファイルをByte配列に読み込みます
            byte[] data = System.IO.File.ReadAllBytes(FileBrowser.Result);
            OnSuccess(data);
        }
        else
        {
            //キャンセル処理
            OnError();
        }
    }

    /// <summary>
    /// 共通処理
    /// </summary>


    //読み込み成功
    private void OnSuccess(byte[] data)
    {
        //読み込み再実行の許可
        IsDoingFuncNow = false;
        if (OnSuccessCallBack != null)
            OnSuccessCallBack(data);
        else
            Debug.Log("ロード成功");
    }

    //読み込み失敗や例外時(WebGLではないならキャンセル時も呼ばれる)
    public void OnError()
    {
        //読み込み再実行の許可
        IsDoingFuncNow = false;
        if (OnFailedCallBack != null)
            OnFailedCallBack();
        else
            Debug.Log("ロード失敗");
    }


    //コライダー作成
    public void SetupVRMCollider(VroidData data, VRMCollider.ColliderType type, Transform parent)
    {
        vrmCollider.SetupVRMCollider(data, type, parent);
    }
#endif
}