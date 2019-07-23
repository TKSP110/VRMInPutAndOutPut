using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System;
using UnityEditor.SceneManagement;

namespace VRMIO
{

    public class ReadMe : EditorWindow
    {

        [InitializeOnLoadMethod]
        static void AutoOpen()
        {
            //セットアップが終わっていたら自動表示なし(Define定義をトリガーとする)
            if (IsExistDefineSymbols(BuildTargetGroup.WebGL, "VRMIO") && IsExistDefineSymbols(BuildTargetGroup.Standalone, "VRMIO"))
                return;

            //ゲーム再生時に呼び出された場合は表示しない
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            //開く(同じウィンドウは開かないように.GetTypeで検索する)
            ReadMe window = GetWindow<ReadMe>(typeof(EditorWindow).Assembly.GetType("VRMIO.ReadMe"));
            window.position = new Rect(100, 100, 550, 800);

        }

        //メニューから開く用
        [MenuItem("VRMIO/ReadMe")]
        static void Open()
        {
            isInit = false;
            //ReadMe window = GetWindow<ReadMe>();
            ReadMe window = GetWindow<ReadMe>(typeof(EditorWindow).Assembly.GetType("VRMIO.ReadMe"));
            window.position = new Rect(100, 100, 550, 800);
        }

        private void Reset()
        {

        }

        void OnEnable()
        {

        }

        void OnGUI()
        {

            Init();

            GUILayout.BeginHorizontal("In BigTitle");
            {
                GUILayout.Label(AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/VRMIO/Sprites/aya_tksp.png"), GUILayout.Width(130), GUILayout.Height(130));
                GUILayout.BeginVertical();
                {
                    GUILayout.Label("VRMInputAndOutPut", TitleStyle);


                    GUILayout.Label(
                        "このアセットはVRMのインポートとエクスポートをサポートします。" + Environment.NewLine +
                        "そしてVRoidを用いた簡単な2D,3Dゲーム制作のサンプルを含みます。" + Environment.NewLine +
                        "あなたがこのアセットを利用したことに関連して生ずる損害について" + Environment.NewLine +
                        "一切責任を負いません。ご利用は自己責任で。" + Environment.NewLine +
                        "※可能であればコードを解析してUniTaskのキャンセル処理を追加してください。"
                        , OverViewStyle);

                    GUILayout.Label("開発、動作環境 : Unity 2019.1.10f1 UnityEditor" + Environment.NewLine +
                        "Windows10, MacOS Mojave" + Environment.NewLine +
                        "ビルド : Standalone(Windows10, Mac), WebGL", RequirementSystemStyle);

                    if (LinkLabel(new GUIContent("コンタクト:@YMTKSP(Twitter)")))
                    {
                        Application.OpenURL("https://twitter.com/ymtksp");
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();
            {

                GUILayout.Label("使用前の準備:上から順番に実行してください。", UsageStyle);

                bool gacst = PlayerSettings.GetApiCompatibilityLevel(BuildTargetGroup.Standalone) != ApiCompatibilityLevel.NET_4_6;
                bool gacwb = PlayerSettings.GetApiCompatibilityLevel(BuildTargetGroup.WebGL) != ApiCompatibilityLevel.NET_4_6;

                bool srv = PlayerSettings.scriptingRuntimeVersion != ScriptingRuntimeVersion.Latest;

                if (gacst || gacwb || srv)
                    GUILayout.Label(".Net4.xに設定してください", UsageStyle);

                if (srv)
                {
                    if (GUILayout.Button("Scripting Runtime Version を.Net4.x Equivalentに変更", GUILayout.Width(500f), GUILayout.Height(20f)))
                    {
                        PlayerSettings.scriptingRuntimeVersion = ScriptingRuntimeVersion.Latest;
                    }
                }


                if (gacst || gacwb)
                {
                    if (GUILayout.Button("Api Compatibility Level を.Net4.x Equivalentに変更", GUILayout.Width(500f), GUILayout.Height(20f)))
                    {
                        PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Standalone, ApiCompatibilityLevel.NET_4_6);
                        PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.WebGL, ApiCompatibilityLevel.NET_4_6);
                        //Restart();
                    }
                }

                GUILayout.Space(5f);

                GUILayout.Label(
                    "以下のアセットをインポートしてください。", UsageStyle);


                GUILayout.Space(12f);

                GUILayout.Label("コルーチンの代わりに使用しています。(UniRx.Async.unitypackage)", RequirementAssetsStyle);
                if (LinkLabel(new GUIContent("UniTask (Ver 1.1.0)")))
                {
                    Application.OpenURL("https://github.com/Cysharp/UniTask/releases/tag/1.1.0");
                }
                GUILayout.Space(3f);


                GUILayout.Label("VRM扱うのに必須です。(UniVRM-0.53.0_6b07.unitypackage)", RequirementAssetsStyle);
                if (LinkLabel(new GUIContent("UniVRM-0.53.0")))
                {
                    Application.OpenURL("https://github.com/vrm-c/UniVRM/releases");
                }
                GUILayout.Space(3f);

                GUILayout.Label("WebGL以外でファイルウィンドウを開くために使用します。", RequirementAssetsStyle);
                if (LinkLabel(new GUIContent("Runtime File Browser(アセットストア)")))
                {
                    UnityEditorInternal.AssetStore.Open("com.unity3d.kharma:content/113006");
                }
                GUILayout.Label("こちらでも可(SimpleFileBrowser.unitypackage)中身は全く同じものです。", RequirementAssetsStyle);
                if (LinkLabel(new GUIContent("UnitySimpleFileBrowser(GitHub)")))
                {
                    Application.OpenURL("https://github.com/yasirkula/UnitySimpleFileBrowser");
                }
                GUILayout.Space(3f);

                GUILayout.Label("サンプルのアニメーション、カメラ制御、キャラ制御を使用しています。", RequirementAssetsStyle);
                if (LinkLabel(new GUIContent("Unity - Chan! Model(アセットストア) © UTJ/UCL")))
                {
                    UnityEditorInternal.AssetStore.Open("com.unity3d.kharma:content/18705");
                }

                GUILayout.Space(5f);

               

                if (!IsExistDefineSymbols(BuildTargetGroup.WebGL, "VRMIO") || !IsExistDefineSymbols(BuildTargetGroup.Standalone, "VRMIO"))
                {

                    GUILayout.Label("シンボルを追加してください", UsageStyle);

                    if (GUILayout.Button("VRMIOをScriptingDefineSymbolに追加"))
                    {
                        AddDefineSymbols(BuildTargetGroup.WebGL, "VRMIO");
                        AddDefineSymbols(BuildTargetGroup.Standalone, "VRMIO");
                    }
                }

                GUILayout.Label("下のボタンでサンプルシーンが自動で登録されます", UsageStyle);

                if (GUILayout.Button("サンプルシーンを開く(現在のシーンは保存されません)", GUILayout.Width(500f), GUILayout.Height(20f)))
                {
                    List<EditorBuildSettingsScene> sceneList = new List<EditorBuildSettingsScene>();
                    sceneList.Add(new EditorBuildSettingsScene("Assets/VRMIO/Sample/Scenes/SampleTitle.unity", true));
                    sceneList.Add(new EditorBuildSettingsScene("Assets/VRMIO/Sample/Scenes/SetUpVroidCollider.unity", true));
                    sceneList.Add(new EditorBuildSettingsScene("Assets/VRMIO/Sample/Scenes/SampleGame2D.unity", true));
                    sceneList.Add(new EditorBuildSettingsScene("Assets/VRMIO/Sample/Scenes/SampleGame3D.unity", true));
                    EditorBuildSettings.scenes = sceneList.ToArray();
                    EditorSceneManager.OpenScene("Assets/VRMIO/Sample/Scenes/SampleTitle.unity");
                }

                GUILayout.BeginVertical(GUI.skin.box);
                {
                    GUILayout.Label(
                "サンプルシーンのメタファイルが壊れている可能性があるので、お手数ですが" +
                "VRMIO内のSampleフォルダを削除した後、もう一度インポートしてください。その際 Reload を選択", RequirementAssetsStyle);
                }
                GUILayout.EndVertical();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/VRMIO/Sprites/SampleImage.png"), GUILayout.Width(250f), GUILayout.Height(250f));

                    GUILayout.BeginVertical();
                    {
                        GUILayout.Space(40f);
                       
                        GUILayout.BeginVertical();
                        {
                            GUILayout.Label("Let`s Develop VRoid Games!!", TextLabelStyle);
                            GUILayout.Space(20f);
                            GUILayout.Label("Special Thanks", TextLabelStyle);

                            if (LinkLabel(new GUIContent("SingletonMonoBehaviourFastお借りしてます")))
                            {
                                Application.OpenURL("https://gist.github.com/tsubaki/481a0460698bf03fd259");
                            }
                        }
                        GUILayout.EndVertical();
                    }
                    GUILayout.EndVertical();
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        //Unityエディタ再起動(なんかうまく動かんし.Net変更後に普通に動くから使うのやめた)
        private static void Restart()
        {
#if UNITY_EDITOR_OSX
        //Mac用Unity起動パス
        var filename = EditorApplication.applicationPath + "/Contents/MacOS/Unity";
        var arguments = "-projectPath " + Application.dataPath.Replace("/Assets", string.Empty);
        var psi = new ProcessStartInfo();
        psi.FileName = "/usr/bin/osascript";
        string shell = filename + " " + arguments;
        psi.Arguments = "-e \"do shell script \\\"(" + shell + ")\\\" \"";
        Process.Start(psi);
        //アプリケーションを終了する
        EditorApplication.Exit(0);
#else
            var filename = EditorApplication.applicationPath;
            var arguments = "-projectPath " + Application.dataPath.Replace("/Assets", string.Empty);
            var startInfo = new ProcessStartInfo
            {
                FileName = filename,
                Arguments = arguments,
            };
            Process.Start(startInfo);
            //アプリケーションを終了する
            EditorApplication.Exit(0);
#endif
        }

        //汎用テキストスタイル
        GUIStyle TextLabelStyle;
        //タイトル
        GUIStyle TitleStyle;
        //概要
        GUIStyle OverViewStyle;
        //動作環境
        GUIStyle RequirementSystemStyle;
        //使い方
        GUIStyle UsageStyle;
        //必要アセット
        GUIStyle RequirementAssetsStyle;
        //リンク用
        GUIStyle LinkStyle;

        //初期化処理
        static bool isInit = false;
        //テキストサイズや色の設定
        void Init()
        {
            if (isInit)
                return;

            TextLabelStyle = new GUIStyle(EditorStyles.label);
            //折り返し有効
            TextLabelStyle.wordWrap = true;
            //サイズ指定
            TextLabelStyle.fontSize = 14;

            //各、汎用テキスト設定をコピーし文字サイズだけ変更
            TitleStyle = new GUIStyle(TextLabelStyle);
            TitleStyle.fontSize = 22;

            OverViewStyle = new GUIStyle(TextLabelStyle);
            OverViewStyle.fontSize = 10;

            RequirementSystemStyle = new GUIStyle(TextLabelStyle);
            RequirementSystemStyle.fontSize = 8;

            UsageStyle = new GUIStyle(TextLabelStyle);
            UsageStyle.fontSize = 14;

            RequirementAssetsStyle = new GUIStyle(TextLabelStyle);
            RequirementAssetsStyle.fontSize = 12;

            //リンクのテキスト
            LinkStyle = new GUIStyle(EditorStyles.label);
            TextLabelStyle.wordWrap = false;
            LinkStyle.normal.textColor = new Color(0x00 / 255f, 0x78 / 255f, 0xDA / 255f, 1f);
            //下線が水平方向に伸縮しないように
            LinkStyle.stretchWidth = false;
            LinkStyle.fontSize = 10;

            isInit = true;
        }

        //リンク付きラベル
        bool LinkLabel(GUIContent label, params GUILayoutOption[] options)
        {
            var position = GUILayoutUtility.GetRect(label, LinkStyle, options);

            Handles.BeginGUI();
            Handles.color = LinkStyle.normal.textColor;
            Handles.DrawLine(new Vector3(position.xMin, position.yMax), new Vector3(position.xMax, position.yMax));
            Handles.color = Color.white;
            Handles.EndGUI();

            EditorGUIUtility.AddCursorRect(position, MouseCursor.Link);

            return GUI.Button(position, label, LinkStyle);
        }

        //Define定義されてるかどうか
        public static bool IsExistDefineSymbols(BuildTargetGroup targetGroup, string newDefine)
        {
            //Defineを取得する
            string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
            //文字列を分割
            string[] symbolArray = symbols.Split(';');
            //配列に検索文字列が含まれているかどうか返す
            return 0 <= Array.IndexOf(symbolArray, newDefine);
        }


        public static void AddDefineSymbols(BuildTargetGroup targetGroup, string newDefine)
        {
            //Defineを取得する
            string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
            string[] symbolArray = symbols.Split(';');

            //既に存在するなら何もしない
            if (IsExistDefineSymbols(targetGroup, newDefine))
                return;

            //Define追加
            List<string> symbolList = new List<string>();
            symbolList.AddRange(symbolArray);
            symbolList.Add(newDefine);
            symbols = string.Join(";", symbolList.ToArray());
            PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, symbols);
        }
    }
}

