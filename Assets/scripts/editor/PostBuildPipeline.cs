using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using AssetBundles;

public class MyBuildPostprocessor
{
    [PostProcessBuildAttribute(1)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        string targetPath;
        if (Utility.GetPlatformName() == "OSX")
        {
            targetPath = pathToBuiltProject + "/";
        }
        else
        {
            targetPath = pathToBuiltProject.Substring(0, pathToBuiltProject.LastIndexOf('/') + 1);
        }
        var sourcePath = Application.dataPath + "/../AssetBundles";
        Debug.Log("Copying AssetBundles to " + targetPath);
        FileUtil.DeleteFileOrDirectory(targetPath + "AssetBundles");
        FileUtil.CopyFileOrDirectory(sourcePath, targetPath + "AssetBundles");
    }
}