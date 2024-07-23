#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
public class IOSBuildEditor : MonoBehaviour
{
    static string buildPath = "/Desktop";//build path, default is "Builds/WebGL"
    static string[] scenes = new string[] { "Assets/Scenes/Sample.unity" };
    static string companyName = "YourCompanyName";
    static string productName = "YourProductName";

    [MenuItem("Tools/IOS/Release Build")]
    public static void BuildReleaseVersion()
    {
        ReleaseBuild();
        if (!Directory.Exists(buildPath)) Directory.CreateDirectory(buildPath);// Create the build directory if it doesn't exist
        BuildPipeline.BuildPlayer(scenes, buildPath, BuildTarget.iOS, BuildOptions.None);
        Debug.Log("Build completed: " + buildPath);
    }

    public static void ReleaseBuild()
    {
        Debug.Log("[WebGLBuildScript][OptimizeWebGLSetting] used OptimizeWebGLSetting settings.");
        //platform settings
        EditorUserBuildSettings.development = false;

        //player setting override
        PlayerSettings.companyName = companyName;
        PlayerSettings.productName = productName;

        //other setting
        PlayerSettings.colorSpace = ColorSpace.Linear;
        PlayerSettings.graphicsJobs = true;
        PlayerSettings.SetScriptingDefineSymbols(NamedBuildTarget.iOS, "DEVELOP;IOS;");
        PlayerSettings.bundleVersion = IncrementVersion(PlayerSettings.bundleVersion);
        PlayerSettings.iOS.buildNumber = IncrementBuildNumber(PlayerSettings.iOS.buildNumber).ToString();
        PlayerSettings.iOS.appleDeveloperTeamID = "";
        PlayerSettings.iOS.appleEnableAutomaticSigning = true;
    }
    private static string IncrementVersion(string version)
    {
        string[] parts = version.Split('.');
        if (parts.Length == 3)
        {
            int major = int.Parse(parts[0]);
            int minor = int.Parse(parts[1]);
            int patch = int.Parse(parts[2]);
            patch++; // Increment patch version
            return $"{major}.{minor}.{patch}";
        }
        else
        {
            Debug.LogWarning("Version format is incorrect. Expected format: 'x.y.z'");
            return "0.1.0";
        }
    }
    private static int IncrementBuildNumber(string build)
    {
        int buildNumber;
        if (int.TryParse(build, out buildNumber))
        {
            buildNumber++; // Increment build number
            return buildNumber;
        }
        else
        {
            Debug.LogWarning("Build number format is incorrect. Expected an integer.");
            return 1; // Default to 1 if parsing fails
        }
    }
}
#endif