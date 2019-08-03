using UnityEngine;
using System.Collections.Generic;

//読み込んだVRMデータをstatic保存して管理するクラス

namespace VRMIO
{
    //ゲームで必要なVroidの情報をまとめた構造体(当たり判定の大きさとvrmファイルの生データ)
    public struct VroidData
    {
        public string ModelName;
        public byte[] vrmdata;

        //コライダのデータ
        public float width;
        public float height;
        public float modelheight;


        public VroidData(byte[] bytes)
        {
            ModelName = null;
            vrmdata = bytes;
            width = 1;
            height = 1;
            modelheight = 1;
        }

        public VroidData(string PlayerName)
        {
            this.ModelName = PlayerName;
            vrmdata = null;
            width = 1;
            height = 1;
            modelheight = 1;
        }

        public VroidData(byte[] bytes, string PlayerName)
        {
            this.ModelName = PlayerName;
            vrmdata = bytes;
            width = 1;
            height = 1;
            modelheight = 1;
        }

        public VroidData(byte[] bytes, string PlayerName, Vector3 sizedata)
        {
            this.ModelName = PlayerName;
            vrmdata = bytes;
            width = sizedata.x;
            height = sizedata.y;
            modelheight = sizedata.z;
        }

        public VroidData(byte[] bytes, Vector3 sizedata)
        {
            this.ModelName = null;
            vrmdata = null;
            width = sizedata.x;
            height = sizedata.y;
            modelheight = sizedata.z;
        }

        public VroidData(string PlayerName, Vector3 sizedata)
        {
            this.ModelName = PlayerName;
            vrmdata = null;
            width = sizedata.x;
            height = sizedata.y;
            modelheight = sizedata.z;
        }

        public void setSizeData(Vector3 sizedata)
        {
            this.width = sizedata.x;
            this.height = sizedata.y;
            this.modelheight = sizedata.z;
        }

        public Vector3 getSizeData()
        {
            return new Vector3(width, height, modelheight);
        }

        public void SetData(byte[] bytes)
        {
            vrmdata = bytes;
        }

        public void SetPlayerName(string PlayerName)
        {
            this.ModelName = PlayerName;
        }

    }

    //読み込んだVRMデータをstatic保存して管理
    public static class S_VRMDATA
    {
        //読み込んだモデルをどのシーンでも使えるようにする
        public static VroidData MyModel = new VroidData();

        //VRMTeleporter用
        public static Dictionary<int, VroidData> VroidDictionary = new Dictionary<int, VroidData>();
    }
}
