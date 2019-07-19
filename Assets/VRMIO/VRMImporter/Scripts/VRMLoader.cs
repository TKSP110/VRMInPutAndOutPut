using UnityEngine;
#if VRMIO
using VRM;
using UniRx.Async;
#endif
namespace VRMIO
{
    public static class VRMLoader
    {
#if VRMIO
        //バイト配列からVroidを作成
        public static async UniTask<GameObject> InstantiateVRM(byte[] bytes)
        {
            //バイト配列からVRMファイルが生成されるまで待ってLoadingModelに格納する
            GameObject LoadingModel = await VRMLoader.LoadVRMFile(bytes);

            //モデルをワールド上に配置します
            LoadingModel.transform.position = new Vector3(0, -1, 0);
            LoadingModel.transform.Rotate(new Vector3(0, 0, 0));

            return LoadingModel;
        }

        public static async UniTask<GameObject> InstantiateVRM(byte[] bytes, Transform parent)
        {
            GameObject LoadingModel = await VRMLoader.LoadVRMFile(bytes);
            LoadingModel.transform.position = new Vector3(0, -1, 0);
            LoadingModel.transform.Rotate(new Vector3(0, 0, 0));
            LoadingModel.transform.SetParent(parent);
            return LoadingModel;
        }

        public static async UniTask<GameObject> InstantiateVRM(byte[] bytes, Vector3 position, Quaternion rotation)
        {
            GameObject LoadingModel = await VRMLoader.LoadVRMFile(bytes);
            LoadingModel.transform.position = new Vector3(0, -1, 0) + position;
            LoadingModel.transform.rotation = rotation;
            return LoadingModel;
        }

        public static async UniTask<GameObject> InstantiateVRM(byte[] bytes, Vector3 position, Quaternion rotation, Transform parent)
        {
            GameObject LoadingModel = await VRMLoader.LoadVRMFile(bytes);
            LoadingModel.transform.position = new Vector3(0, -1, 0) + position;
            LoadingModel.transform.rotation = rotation;
            LoadingModel.transform.SetParent(parent);
            return LoadingModel;
        }


        //バイト配列からVRMデータをロード
        private static async UniTask<GameObject> LoadVRMFile(byte[] bytes)
        {

            //VRMImporterContextがVRMを読み込む機能を提供します
            var vrmContext = new VRMImporterContext();

            // GLB形式でJSONを取得しParseします
            vrmContext.ParseGlb(bytes);

            // VRMのメタデータを取得
            var meta = vrmContext.ReadMeta(false); //引数をTrueに変えるとサムネイルも読み込みます

            //読み込めたかどうかログにモデル名を出力してみる
            Debug.LogFormat("meta: title:{0}", meta.Title);

            //疑似非同期処理で読み込みます
            await vrmContext.LoadCoroutine();
            //vrmContext.Load();

            //メッシュを表示します
            vrmContext.ShowMeshes();

            //読込が完了するとcontext.RootにモデルのGameObjectが入っています
            return vrmContext.Root;
        }
#endif
    }
}
