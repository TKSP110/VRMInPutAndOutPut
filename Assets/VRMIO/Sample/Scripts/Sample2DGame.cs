using UnityEngine.SceneManagement;
using UnityEngine;
#if VRMIO
using UniRx.Async;
using VRMIO;
#endif
public class Sample2DGame : MonoBehaviour
{

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

        //コライダー設定
        vrmImporter.SetupVRMCollider(data, VRMCollider.ColliderType.TYPE2D, Player.transform);
        //リジッドボディ
        Rigidbody2D rb = Player.AddComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        //カメラをプレイヤーの子へ
        Camera.main.transform.SetParent(Player.transform);

    }
#endif
    public void OnBackTitleButton()
    {
        SceneManager.LoadScene("SampleTitle");
    }
}
