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
        // プロジェクト内の全アセットを取得
        string[] allAssetPaths = AssetDatabase.GetAllAssetPaths()
            .Where(path => path.StartsWith("Assets/"))
            .ToArray();

        // シーンやPrefabなどに含まれる依存関係を収集
        HashSet<string> usedAssets = new HashSet<string>();

        // シーンとPrefabを対象にする
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

        // 未使用アセットを抽出
        List<string> unusedAssets = new List<string>();
        foreach (string asset in allAssetPaths)
        {
            if (!usedAssets.Contains(asset))
            {
                unusedAssets.Add(asset);
            }
        }

        // 結果出力
        Debug.Log($"未使用アセット数: {unusedAssets.Count}");
        foreach (string asset in unusedAssets)
        {
            Debug.Log($"未使用: {asset}", AssetDatabase.LoadAssetAtPath<Object>(asset));
        }
    }
}
