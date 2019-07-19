using System.IO;
using UnityEngine;

#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif


public static class VRMExporter
{
#if VRMIO

    //灰色の部分がWebGLの処理です

#if UNITY_WEBGL && !UNITY_EDITOR
    //プラグインの関数を定義
    [DllImport("__Internal")]
    private static extern void WebGLDownloadStringFile(string filename, string text);

    [DllImport("__Internal")]
    private static extern void WebGLDownloadBytesFile(string filename, byte[] array, int size);
#endif

    //Textを保存する
    public static void ExportText(string filename, string text)
    {

#if UNITY_WEBGL && !UNITY_EDITOR
        //プラグインを実行
        WebGLDownloadStringFile(filename, text);
#else
        //FileクラスでstreamingAssetsPathに書き込み(WebGL以外の場合は)
        string path = Application.streamingAssetsPath + "/" + filename;
        //ファイルが無ければ
        if (File.Exists(path))
        {
            //作成
            File.Create(path).Close();
        }
        //UnityEditorだと表示されるまで時間かかります
        File.WriteAllText(path, text);
#endif

    }



    //Vroidを保存する
    public static void ExportVRM(string filename, byte[] array)
    {

#if UNITY_WEBGL && !UNITY_EDITOR
        //プラグインを実行
        WebGLDownloadBytesFile(filename, array, array.Length);
#else
        //FileクラスでstreamingAssetsPathに書き込み(WebGL以外の場合は)
        string path = Application.streamingAssetsPath + "/" + filename;
        //ファイルが無ければ
        if (File.Exists(path))
        {
            //作成
            File.Create(path).Close();
        }
        //UnityEditorだと表示されるまで時間かかります
        File.WriteAllBytes(path, array);
#endif

    }
#endif
}

