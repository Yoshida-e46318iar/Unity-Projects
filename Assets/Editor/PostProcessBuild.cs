#if UNITY_IOS
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;

public class PostProcessBuild
{
    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
    {
        if (target == BuildTarget.iOS)
        {
            // Info.plist のパス
            string plistPath = Path.Combine(pathToBuiltProject, "Info.plist");
            PlistDocument plist = new PlistDocument();
            plist.ReadFromFile(plistPath);

            // ルート辞書を取得
            PlistElementDict rootDict = plist.root;

            // Google Mobile Ads App ID を設定
            rootDict.SetString("GADApplicationIdentifier", "ca-app-pub-1850040616476651~5193552047");

            // 上書き保存
            plist.WriteToFile(plistPath);
        }
    }
}
#endif
