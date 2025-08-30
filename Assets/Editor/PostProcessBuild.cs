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
            // Info.plist �̃p�X
            string plistPath = Path.Combine(pathToBuiltProject, "Info.plist");
            PlistDocument plist = new PlistDocument();
            plist.ReadFromFile(plistPath);

            // ���[�g�������擾
            PlistElementDict rootDict = plist.root;

            // Google Mobile Ads App ID ��ݒ�
            rootDict.SetString("GADApplicationIdentifier", "ca-app-pub-1850040616476651~5193552047");

            // �㏑���ۑ�
            plist.WriteToFile(plistPath);
        }
    }
}
#endif
