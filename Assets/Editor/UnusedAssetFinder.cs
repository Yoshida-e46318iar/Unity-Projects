using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class UnusedAssetFinder
{
    [MenuItem("Tools/Find Unused Assets")]
    public static void FindUnusedAssets()
    {
        // �v���W�F�N�g���̑S�A�Z�b�g���擾
        string[] allAssetPaths = AssetDatabase.GetAllAssetPaths()
            .Where(path => path.StartsWith("Assets/"))
            .ToArray();

        // �V�[����Prefab�ȂǂɊ܂܂��ˑ��֌W�����W
        HashSet<string> usedAssets = new HashSet<string>();

        // �V�[����Prefab��Ώۂɂ���
        string[] searchTargets = AssetDatabase.FindAssets("t:Scene t:Prefab");

        foreach (string guid in searchTargets)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string[] dependencies = AssetDatabase.GetDependencies(path, true);
            foreach (string dep in dependencies)
            {
                if (dep.StartsWith("Assets/"))
                    usedAssets.Add(dep);
            }
        }

        // ���g�p�A�Z�b�g�𒊏o
        List<string> unusedAssets = new List<string>();
        foreach (string asset in allAssetPaths)
        {
            if (!usedAssets.Contains(asset))
            {
                unusedAssets.Add(asset);
            }
        }

        // ���ʏo��
        Debug.Log($"���g�p�A�Z�b�g��: {unusedAssets.Count}");
        foreach (string asset in unusedAssets)
        {
            Debug.Log($"���g�p: {asset}", AssetDatabase.LoadAssetAtPath<Object>(asset));
        }
    }
}
