using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Reflection;

public static class GameMaker
{
    [MenuItem("Tool/AutoBuild")]
    static void BuildApp()
    {

        //切换登录方式
        string[] files = Directory.GetFiles(Application.dataPath + "/GUI/icon");
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].EndsWith(".meta")) continue;
            string name = files[i].Replace("\\", "/");
			name = name.Substring (name.LastIndexOf ("/") + 1);
			if (name == ".DS_Store")
				continue;
            string pre = name.Substring(0, 2);
            string gamename = name.Replace(pre, "");
            gamename = gamename.Substring(0, gamename.IndexOf("."));
			Debug.Log ("name = " + name + " pre = " + pre + " gamename = " + gamename);
            string path = files[i].Replace(Application.dataPath, "Assets");
            Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            string url = "http://lebogame.co/app.php?code=" + pre;
            FileStream fs = File.OpenWrite(Application.streamingAssetsPath + "/URL.txt");
            StreamWriter sw = new StreamWriter(fs);
            sw.Flush();
            sw.Write(url);
            sw.Close();
            fs.Close();

            //AssetDatabase.DeleteAsset("Assets/Resources/URL.txt");
            if (pre != "a7") continue;
          //  if(!isExistList(pre)) continue;
            //Debug.Log("ok = "+pre);
            AssetDatabase.CopyAsset("Assets/StreamingAssets/URL.txt", "Assets/Resources/URL.txt");
            Build(tex, pre, gamename);

        }
    }
    static string[] readyList = new string[6] { "e6", "d7", "f6", "i7", "l9", "m6" };//new string[35] { "a7", "a8", "b2", "b5", "b9", "c6", "c8", "d8", "e2", "e5", "f2", "f8", "h1", "h7", "i0", "i5", "i6", "j0", "k2", "k7", "l0", "m7", "m8", "n2", "o1", "o5", "o9", "p3", "d7", "e4", "l9", "o7", "c3", "k6", "f6" };
    static bool isExistList(string pre)
    {
        for (int i = 0; i < readyList.Length; i++)
        {
            if (pre == readyList[i])
                return true;
        }
        return false;
    }
    static void Build(Texture2D tex, string pre, string gamename)
    {
        PlayerSettings.productName = gamename;
        PlayerSettings.applicationIdentifier = "com.lebo." + pre;
        BuildTargetGroup tar = BuildTargetGroup.Android;

#if UNITY_ANDROID
        tar = BuildTargetGroup.Android;
#elif UNITY_IOS
		tar = BuildTargetGroup.iOS;
#endif
        int[] sizelist = PlayerSettings.GetIconSizesForTargetGroup(tar);
        Texture2D[] texs = new Texture2D[sizelist.Length];
        for (int i = 0; i < sizelist.Length; i++)
        {
            texs[i] = tex;
        }
#if UNITY_ANDROID
        PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Android, texs);
		#elif UNITY_IOS
        PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.iOS, texs);
#endif
		PlayerSettings.defaultInterfaceOrientation = UIOrientation.AutoRotation;
        PlayerSettings.allowedAutorotateToLandscapeLeft = true;
        PlayerSettings.allowedAutorotateToLandscapeRight = true;

        string bagname = "";
        string path = "";
#if UNITY_ANDROID
        bagname = "lebogaming_android_" + pre+".apk";
        path = Application.dataPath.Replace("Assets", "/Publish/");
		#elif UNITY_IOS
        bagname = "lebogaming_ios_"+pre;
		path = Application.dataPath.Replace("Assets", "Publish/");
#endif

		path += bagname;
        Debug.Log("path = " + path);
        BuildPlayerOptions op = new BuildPlayerOptions();
        op.scenes = null;
        op.locationPathName = path;
#if UNITY_ANDROID
        op.target = BuildTarget.Android;
		#elif UNITY_IOS
        op.target = BuildTarget.iOS;
#endif
		op.options = BuildOptions.None;

        BuildPipeline.BuildPlayer(op);
    }

}
