using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
#if VRMIO
using VRM;
using UniRx.Async;
using VRMIO;
#endif
public class SampleTitle : MonoBehaviour
{
#if VRMIO
    private VRMIOImporter importer;

    [Header("WebGLの場合はボタンクリック後にもう一度クリックを促す必要がある")]
    [SerializeField]
    private Canvas recommendClickCanvas;

    [SerializeField]
    private Button ImportButton;
    [SerializeField]
    private Button SetUpColliderButton;
    [SerializeField]
    private Button Game2DButton;
    [SerializeField]
    private Button Game3DButton;
    [SerializeField]
    private Button ExportButton;

    [SerializeField]
    private Canvas LoadingCanvas;

    // Start is called before the first frame update
    void Start()
    {

        importer = GameObject.FindWithTag("GameController").GetComponent<VRMIOImporter>();

        //もしモデルがあるならロード
        if (S_VRMDATA.MyModel.vrmdata != null)
        {
            createVroid(S_VRMDATA.MyModel.vrmdata, false).Forget();
            LoadingCanvas.enabled = true;
        }
        else
        {
            ImportButton.interactable = true;
        }

    }

    public void OnImportButton()
    {
        //VRMファイルからデータ読み込み開始
        importer.ImportVrmData(recommendClickCanvas, loadsuccess, loadfailed);
    }

    //読み込み成功時
    public void loadsuccess(byte[] data)
    {
        ImportButton.interactable = false;
        SetUpColliderButton.interactable = false;
        Game2DButton.interactable = false;
        Game3DButton.interactable = false;
        ExportButton.interactable = false;
        //UniTaskの関数発火
        createVroid(data, true).Forget();
        LoadingCanvas.enabled = true;
    }

    //失敗時
    public void loadfailed()
    {
        Debug.Log("エラーだよ！");
    }

    GameObject model;

    private async UniTask createVroid(byte[] data, bool dataupdate)
    {
        if (model != null)
        {
            Destroy(model);
            //Textureなどのアセットメモリ開放
            await Resources.UnloadUnusedAssets();
        }
    

        model = await VRMLoader.InstantiateVRM(data, Vector3.zero, Quaternion.Euler(0, 180, 0));

        if(dataupdate)
        S_VRMDATA.MyModel = new VroidData(data, getName(model.GetComponent<VRMMeta>().Meta.Title));
        
        ImportButton.interactable = true;
        SetUpColliderButton.interactable = true;
        Game2DButton.interactable = true;
        Game3DButton.interactable = true;
        ExportButton.interactable = true;
        LoadingCanvas.enabled = false;
    }

    private string getName(string namedata)
    {
        if (string.IsNullOrEmpty(namedata))
            return "名無し";

        return namedata;
    }

    public void OnSetUpColliderButton()
    {
        SceneManager.LoadScene("SetUpVroidCollider");
    }

    public void OnGame2DButton()
    {
        SceneManager.LoadScene("SampleGame2D");
    }

    public void OnGame3DButton()
    {
        SceneManager.LoadScene("SampleGame3D");
    }

    public void OnExportButton()
    {
        if (S_VRMDATA.MyModel.vrmdata != null)
            VRMExporter.ExportVRM(S_VRMDATA.MyModel.ModelName + ".vrm", S_VRMDATA.MyModel.vrmdata);
    }
#endif
}
