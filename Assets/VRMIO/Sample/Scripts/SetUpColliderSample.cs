using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
#if VRMIO
using VRMIO;
using UniRx.Async;
#endif
public class SetUpColliderSample : MonoBehaviour
{
#if VRMIO
    //コライダーの可視化用カプセル
    private Transform ColliderView;

    [Header("モデルとコライダの親となるオブジェクト(こいつが回転する)")]
    [SerializeField]
    private Transform pibot;

    [Header("スライダー各種")]
    [SerializeField]
    private Slider widthSlider;
    [SerializeField]
    private Slider heightSlider;
    [SerializeField]
    private Slider modelHeighjtSlider;

    [SerializeField]
    private Canvas LoadingCanvas;

    //読み込んだVroidモデル
    private GameObject LoadingModel;

    private VRMIOImporter vrmImporter;


    // Start is called before the first frame update
    void Start()
    {
        if (S_VRMDATA.MyModel.vrmdata == null)
        {
            Debug.LogWarning("データがありません。タイトルに戻ります");
            SceneManager.LoadScene("SampleTitle");
            return;
        }

        //コライダーView取得
        ColliderView = pibot.Find("ColliderView");

        //MyModelあれば読み込み生成
        vrmImporter = GameObject.FindWithTag("GameController").GetComponent<VRMIOImporter>();
        createVRM(S_VRMDATA.MyModel.vrmdata).Forget();
        //スライダー初期化
        heightSlider.value = S_VRMDATA.MyModel.height;
        widthSlider.value = S_VRMDATA.MyModel.width;
        modelHeighjtSlider.value = S_VRMDATA.MyModel.modelheight - 1;
        //可視化コライダーサイズ初期化
        ColliderView.localScale = new Vector3(widthSlider.value, heightSlider.value, widthSlider.value);
        ColliderView.position = ColliderView.rotation * new Vector3(0, modelHeighjtSlider.value, 0);
    }


    private async UniTask createVRM(byte[] data)
    {
        if (LoadingModel != null)
            Destroy(LoadingModel);

        LoadingModel = await VRMLoader.InstantiateVRM(data, new Vector3(0, 0, 0), Quaternion.Euler(0, 230, 0), pibot);
        LoadingCanvas.enabled = false;
    }

    private Vector3 ViewRotation = Vector3.zero;

    //回転制御
    private void Update()
    {
        ViewRotation += new Vector3(Input.GetAxis("Vertical"), -Input.GetAxis("Horizontal"), 0);
        if (LoadingModel != null)
        {
            // 選択を解除
            EventSystem.current.SetSelectedGameObject(null);
            pibot.rotation = Quaternion.Euler(ViewRotation);
        }
    }

    private void cancelsellectable()
    {

    }

    public void OnChengeHeight(float val)
    {
        ColliderView.localScale = new Vector3(ColliderView.localScale.x, val, ColliderView.localScale.z);
    }
    public void OnChengeWidth(float val)
    {
        ColliderView.localScale = new Vector3(val, ColliderView.localScale.y, val);
    }
    public void OnChengeModelHeight(float val)
    {
        ColliderView.position = ColliderView.rotation * new Vector3(0, val, 0);
    }

    public void OnSaveAndBackButton()
    {
        //保存してタイトルに戻る
        //足が軸なので高さだけ+1しておく
        S_VRMDATA.MyModel.setSizeData(new Vector3(widthSlider.value, heightSlider.value, modelHeighjtSlider.value + 1));
        SceneManager.LoadScene("SampleTitle");
    }
#endif
}
