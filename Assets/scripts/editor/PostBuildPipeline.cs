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

        string targetPath = pathToBuiltProject.Substring(0, pathToBuiltProject.LastIndexOf('/') + 1);
        if (File.Exists(targetPath + "/README.md"))
        {
            FileUtil.DeleteFileOrDirectory(targetPath + "/README.md");
        }
        FileUtil.CopyFileOrDirectory(Application.dataPath + "/../README.md", targetPath + "/README.md");

        if (Utility.GetPlatformName() == "OSX")
        {
            targetPath = pathToBuiltProject + "/";
        }

        var sourcePath = Application.dataPath + "/../AssetBundles/" + Utility.GetPlatformForAssetBundles(EditorUserBuildSettings.activeBuildTarget);
        Debug.Log("Copying AssetBundles to " + targetPath);
        FileUtil.DeleteFileOrDirectory(targetPath + "AssetBundles");
        if (!Directory.Exists(targetPath + "AssetBundles") )
            Directory.CreateDirectory (targetPath + "AssetBundles");

        FileUtil.CopyFileOrDirectory(sourcePath, targetPath + "AssetBundles/" + Utility.GetPlatformForAssetBundles(EditorUserBuildSettings.activeBuildTarget));
    }
}
