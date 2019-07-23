using UnityEngine.SceneManagement;
using UnityEngine;
#if VRMIO
using UniRx.Async;
using VRMIO;
using UnityChan;
#endif
public class Sample2DGame : MonoBehaviour
{


    [Header("アニメーションコントローラー")]
    [SerializeField]
    private RuntimeAnimatorController VRoidLocomotion;

    [Header("ロード用キャンバス")]
    [SerializeField]
    private Canvas LoadingCanvas;

    //VRoidオブジェクトが格納される
    private GameObject Player;

#if VRMIO
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
        
        vrmImporter = GameObject.FindWithTag("GameController").GetComponent<VRMIOImporter>();

        createVRM(S_VRMDATA.MyModel).Forget();

    }

    

    private async UniTask createVRM(VroidData data)
    {
        if (Player != null)
            Destroy(Player);

        Player = await VRMLoader.InstantiateVRM(data.vrmdata,Vector3.zero,Quaternion.Euler(0, 90, 0));

        LoadingCanvas.enabled = false;

        //コライダー設定
        vrmImporter.SetupVRMCollider(data, VRMCollider.ColliderType.TYPE2D, Player.transform);
        //リジッドボディ
        Rigidbody2D rb = Player.AddComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;


        //unitychanのサンプルシーンと同じ状況を作る

        //UnityChanControlScriptWithRgidBodyの改変版
        VRoidControllerSample2D controller = Player.AddComponent<VRoidControllerSample2D>();

        controller.useCurves = false;

        //空のオブジェクト生成
        GameObject standardPos = new GameObject("CamPos");
        GameObject frontPos = new GameObject("FrontPos");
        GameObject jumpPos = new GameObject("JumpPos");

        //子に設定し、座標移動
        standardPos.transform.SetParent(Player.transform);
        standardPos.transform.localPosition = new Vector3(5, 1.25f, 0);
        standardPos.transform.localRotation = Quaternion.Euler(7.5f, 270f, 0);
        frontPos.transform.SetParent(Player.transform);
        frontPos.transform.localPosition = new Vector3(-2.651793f, 1.367743f, 0.1f);
        frontPos.transform.localRotation = Quaternion.Euler(6.271f, 180, 0);
        jumpPos.transform.SetParent(Player.transform);
        //ジャンプ時のカメラが向かう座標
        jumpPos.transform.localPosition = new Vector3(0, 5, 0);
        jumpPos.transform.localRotation = Quaternion.Euler(-5 , 90, 0);

        //アニメーションコントローラー設定
        Player.GetComponent<Animator>().runtimeAnimatorController = VRoidLocomotion;

        //カメラを設定
        Camera.main.gameObject.AddComponent<ThirdPersonCamera>();
        standardPosT = standardPos.transform;
        frontPosT = frontPos.transform;
        startrStandardPos = standardPosT.position;
        startFrontPos = frontPosT.position;
        startrStandardRota = standardPosT.rotation;
        startFrontRota = frontPosT.rotation;
    }
#endif

    private Transform standardPosT;
    private Transform frontPosT;
    private Vector3 startrStandardPos;
    private Vector3 startFrontPos;
    private Quaternion startrStandardRota;
    private Quaternion startFrontRota;
    private void FixedUpdate()
    {
        if (Player == null)
            return;
        standardPosT.position = new Vector3(standardPosT.position.x, standardPosT.position.y, startrStandardPos.z);
        frontPosT.position = new Vector3(frontPosT.position.x, frontPosT.position.y, startFrontPos.z);
        standardPosT.rotation = startrStandardRota;
        frontPosT.rotation = startFrontRota;
    }

    public void OnBackTitleButton()
    {
        SceneManager.LoadScene("SampleTitle");
    }
}
