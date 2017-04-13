using UnityEngine;
#if UNITY_EDITOR	
using UnityEditor;
#endif

namespace AssetBundles
{
	public class Utility
	{
		public const string AssetBundlesOutputPath = "AssetBundles";
	
		public static string GetPlatformName()
		{
	#if UNITY_EDITOR
			return GetPlatformForAssetBundles(EditorUserBuildSettings.activeBuildTarget);
	#else
			return GetPlatformForAssetBundles(Application.platform);
	#endif
		}
	
	#if UNITY_EDITOR
		public static string GetPlatformForAssetBundles(BuildTarget target)
		{
			switch(target)
			{
			case BuildTarget.Android:
				return "Android";
			case BuildTarget.iOS:
				return "iOS";
			case BuildTarget.WebGL:
				return "WebGL";
			case BuildTarget.WebPlayer:
				return "WebPlayer";
			case BuildTarget.StandaloneWindows:
			case BuildTarget.StandaloneWindows64:
				return "Windows";
			case BuildTarget.StandaloneOSXIntel:
			case BuildTarget.StandaloneOSXIntel64:
			case BuildTarget.StandaloneOSXUniversal:
				return "OSX";
			case BuildTarget.StandaloneLinux:
			case BuildTarget.StandaloneLinux64:
			case BuildTarget.StandaloneLinuxUniversal:
				return "Linux";
			default:
				return null;
			}
		}
		#endif

		public static string GetPlatformForAssetBundles(RuntimePlatform platform)
		{
			switch(platform)
			{
				case RuntimePlatform.Android:
					return "Android";
				case RuntimePlatform.IPhonePlayer:
					return "iOS";
				case RuntimePlatform.WebGLPlayer:
					return "WebGL";
				case RuntimePlatform.OSXWebPlayer:
				case RuntimePlatform.WindowsWebPlayer:
					return "WebPlayer";
				case RuntimePlatform.WindowsPlayer:
					return "Windows";
				case RuntimePlatform.OSXPlayer:
					return "OSX";
				case RuntimePlatform.LinuxEditor:
				case RuntimePlatform.LinuxPlayer:
					return "Linux";
				default:
					return null;
			}
		}
	}
}
