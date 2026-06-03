using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class UnityBuildScript
{
    public static void PerformBuild()
    {
        // Define build output path
        string buildPath = "Builds/Windows/MyGame.exe";

        // Get all scenes from Build Settings
        string[] scenes = GetEnabledScenes();

        // Configure build options
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = buildPath,
            target = BuildTarget.StandaloneWindows64,   // Target 64-bit Windows
            options = BuildOptions.None                 // Standard release build
        };

        // Start the build process
        Debug.Log($"Starting build for Windows. Output path: {buildPath}");
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);

        // Evaluate build result
        if (report.summary.result == BuildResult.Succeeded)
        {
            Debug.Log($"Build succeeded! Size: {report.summary.totalSize} bytes");
        }
        else if (report.summary.result == BuildResult.Failed)
        {
            Debug.LogError("Build failed!");
            // Important: Return a non-zero exit code so the CI knows the build failed
            EditorApplication.Exit(1);
        }
        
        // Optional: Exit Unity after build to clean up
        EditorApplication.Exit(0);
    }

    private static string[] GetEnabledScenes()
    {
        // Gather all scenes that are enabled in your Build Settings window
        var scenes = EditorBuildSettings.scenes;
        var enabledScenes = System.Array.FindAll(scenes, scene => scene.enabled);
        var scenePaths = System.Array.ConvertAll(enabledScenes, scene => scene.path);
        return scenePaths;
    }
}
