using UnityEditor;
using UnityEngine;

public class myTool
{
    [MenuItem("Tools/Make AB")]
    public static void MakeAssetBundle()
    {
        Debug.Log("打包Ab");
        string outPath = "Assets/AssetBundles";
        if (!System.IO.Directory.Exists(outPath))
            System.IO.Directory.CreateDirectory(outPath);
        BuildPipeline.BuildAssetBundles(outPath, 0, BuildTarget.StandaloneWindows64);
    }
}
