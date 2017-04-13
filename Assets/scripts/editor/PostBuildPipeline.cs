using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using AssetBundles;
using System.IO;

public class MyBuildPostprocessor
{
    [PostProcessBuildAttribute(1)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        //TODO: Find out why this crashes the Unity editor!
        //BuildScript.BuildTargetAssetBundles();

        string targetPath;
        if (Utility.GetPlatformName() == "OSX")
        {
            targetPath = pathToBuiltProject + "/";
        }
        else
        {
            targetPath = pathToBuiltProject.Substring(0, pathToBuiltProject.LastIndexOf('/') + 1);
        }
        var sourcePath = Application.dataPath + "/../AssetBundles/" + Utility.GetPlatformForAssetBundles(EditorUserBuildSettings.activeBuildTarget);
        Debug.Log("Copying AssetBundles to " + targetPath);
        FileUtil.DeleteFileOrDirectory(targetPath + "AssetBundles");
        if (!Directory.Exists(targetPath + "AssetBundles") )
            Directory.CreateDirectory (targetPath + "AssetBundles");

        FileUtil.CopyFileOrDirectory(sourcePath, targetPath + "AssetBundles/" + Utility.GetPlatformForAssetBundles(EditorUserBuildSettings.activeBuildTarget));
    }
}
