using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;
using System.IO;

public class BuildToolExample : MonoBehaviour
{
    [MenuItem("Build/Build Windows")]
    public static void MyBuild()
    {
        // ===== This sample is taken from the Unity scripting API here:
        // https://docs.unity3d.com/ScriptReference/BuildPipeline.BuildPlayer.html
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/Scenes/Main.unity"};
        var buildPath = "./Builds/Game.exe";
        buildPlayerOptions.locationPathName = buildPath;
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = BuildOptions.None;

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
    }
}