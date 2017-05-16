using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.Xml;
#endif
using System.IO;


public static class OneWaySDKSetting
{
		[PostProcessBuildAttribute (100)]
		static void OnPostprocessBuild(BuildTarget target, string pathToBuildProject){

		if (target != BuildTarget.iOS) {
			Debug.LogWarning ("Target is not iPhone. XCodePostProcess will not run");
			return;
		}

		string _projPath = PBXProject.GetPBXProjectPath (pathToBuildProject);
		PBXProject _pbxProj = new PBXProject ();

		_pbxProj.ReadFromString (File.ReadAllText (_projPath));
		string _targetGuid = _pbxProj.TargetGuidByName ("Unity-iPhone");

		//*******************************设置buildsetting*******************************//
		_pbxProj.SetBuildProperty (_targetGuid, "OTHER_LDFLAGS", "-all_load");  

		File.WriteAllText (_projPath, _pbxProj.WriteToString ());
	}
}