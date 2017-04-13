// C# example:
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class MyBuildPostprocessor
{
    [PostProcessBuildAttribute(1)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        var targetPath = pathToBuiltProject.Substring(0, pathToBuiltProject.LastIndexOf('/') + 1);
        var sourcePath = Application.dataPath + "/../AssetBundles";
        Debug.Log("Copying AssetBundles to " + targetPath);
        FileUtil.CopyFileOrDirectory(sourcePath, targetPath + "AssetBundles");
    }
}