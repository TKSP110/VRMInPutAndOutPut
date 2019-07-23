using UnityEngine.SceneManagement;
using UnityEngine;
#if VRMIO
using UniRx.Async;
using VRMIO;
using UnityChan;
#endif
public class Sample3DGame : MonoBehaviour
{

    [Header("アニメーションコントローラー")]
    [SerializeField]
    private RuntimeAnimatorController VRoidLocomotion;

    [Header("ロード用キャンバス")]
    [SerializeField]
    private Canvas LoadingCanvas;

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

    private GameObject Player;

    private async UniTask createVRM(VroidData data)
    {
        if (Player != null)
            Destroy(Player);

        Player = await VRMLoader.InstantiateVRM(data.vrmdata);

        LoadingCanvas.enabled = false;

        //設定サイズのコライダー追加
        vrmImporter.SetupVRMCollider(data, VRMCollider.ColliderType.TYPE3D, Player.transform);
        //リジッドボディ
        Rigidbody rb = Player.AddComponent<Rigidbody>();
        //回転軸固定
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        //unitychanのサンプルシーンと同じ状況を作る

        //UnityChanControlScriptWithRgidBodyの改変版(コライダーの位置とGUI表示変えただけ)
        Player.AddComponent<VRoidControllerSample3D>();

        //空のオブジェクト生成
        GameObject standardPos = new GameObject("CamPos");
        GameObject frontPos = new GameObject("FrontPos");
        GameObject jumpPos = new GameObject("JumpPos");

        //子に設定し、座標移動
        standardPos.transform.SetParent(Player.transform);
        standardPos.transform.localPosition = new Vector3(0, 1.25f, -2f);
        standardPos.transform.localRotation = Quaternion.Euler(7.5f, 0, 90);
        frontPos.transform.SetParent(Player.transform);
        frontPos.transform.localPosition = new Vector3(0.1f, 1.367743f, 2.651793f);
        frontPos.transform.localRotation = Quaternion.Euler(6.271f, 180, 0);
        jumpPos.transform.SetParent(Player.transform);
        jumpPos.transform.localPosition = new Vector3(0, 0.540453f, -0.8993217f);
        jumpPos.transform.localRotation = Quaternion.Euler(-43.376f, 0, 0);

        //アニメーションコントローラー設定
        Player.GetComponent<Animator>().runtimeAnimatorController = VRoidLocomotion;

        //カメラを設定
        Camera.main.gameObject.AddComponent<ThirdPersonCamera>();


    }
#endif
    public void OnBackTitleButton()
    {
        SceneManager.LoadScene("SampleTitle");
    }
}
