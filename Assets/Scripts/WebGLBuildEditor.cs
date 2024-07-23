#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
public class WebGLBuildEditor : MonoBehaviour
{
    static string buildPath = "/Desktop";
    static string[] scenes = new string[] { "Assets/Scenes/Sample.unity" };
    static string companyName = "YourCompanyName";
    static string productName = "YourProductName";

    [MenuItem("Tools/WebGL/Release Build")]
    public static void BuildReleaseVersion()
    {
        ReleaseBuild();
        if (!Directory.Exists(buildPath)) Directory.CreateDirectory(buildPath);// Create the build directory if it doesn't exist
        BuildPipeline.BuildPlayer(scenes, buildPath, BuildTarget.WebGL, BuildOptions.None);
        Debug.Log("Build completed: " + buildPath);
    }
    [MenuItem("Tools/WebGL/Debug Build")]
    public static void BuildDebugVersion()
    {
        DebugBuild();
        if (!Directory.Exists(buildPath)) Directory.CreateDirectory(buildPath);// Create the build directory if it doesn't exist
        BuildPipeline.BuildPlayer(scenes, buildPath, BuildTarget.WebGL, BuildOptions.None);
        Debug.Log("Build completed: " + buildPath);
    }
    public static void ReleaseBuild()
    {
        Debug.Log("[WebGLBuildScript][OptimizeWebGLSetting] used OptimizeWebGLSetting settings.");
        //platform settings
        EditorUserBuildSettings.development = false;
        EditorUserBuildSettings.SetPlatformSettings(
            "WebGL",
            "CodeOptimization",
            "SizeWithLTO"
        );

        //player setting override
        PlayerSettings.companyName = companyName;
        PlayerSettings.productName = productName;
        PlayerSettings.bundleVersion = IncrementVersion(PlayerSettings.bundleVersion); ;

        //other setting
        PlayerSettings.colorSpace = ColorSpace.Linear;
        PlayerSettings.graphicsJobs = true;
        PlayerSettings.SetIl2CppCodeGeneration(NamedBuildTarget.WebGL, Il2CppCodeGeneration.OptimizeSpeed);
        PlayerSettings.SetIl2CppCompilerConfiguration(NamedBuildTarget.WebGL, Il2CppCompilerConfiguration.Release);
        PlayerSettings.SetScriptingDefineSymbols(NamedBuildTarget.WebGL, "DEVELOP;WebGL;");
        PlayerSettings.SetManagedStrippingLevel(NamedBuildTarget.WebGL, ManagedStrippingLevel.Low);
        PlayerSettings.stripUnusedMeshComponents = true;
        PlayerSettings.mipStripping = true;

        //publish setting
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Brotli;
        PlayerSettings.WebGL.dataCaching = true;
        PlayerSettings.WebGL.debugSymbolMode = WebGLDebugSymbolMode.Off;
        PlayerSettings.WebGL.powerPreference = WebGLPowerPreference.LowPower;
        PlayerSettings.WebGL.exceptionSupport = WebGLExceptionSupport.None;
    }
    public static void DebugBuild()
    {
        Debug.Log("[WebGLBuildScript][OptimizeWebGLSetting] used OptimizeWebGLSetting settings.");
        //platform settings
        EditorUserBuildSettings.development = false;
        EditorUserBuildSettings.SetPlatformSettings(
            "WebGL",
            "CodeOptimization",
            "ShorterBuildTime"
        );

        //player setting override
        PlayerSettings.companyName = companyName;
        PlayerSettings.productName = productName;
        PlayerSettings.bundleVersion = IncrementVersion(PlayerSettings.bundleVersion);

        //other setting
        PlayerSettings.colorSpace = ColorSpace.Linear;
        PlayerSettings.graphicsJobs = true;
        PlayerSettings.SetIl2CppCodeGeneration(NamedBuildTarget.WebGL, Il2CppCodeGeneration.OptimizeSize);
        PlayerSettings.SetIl2CppCompilerConfiguration(NamedBuildTarget.WebGL, Il2CppCompilerConfiguration.Release);
        PlayerSettings.SetScriptingDefineSymbols(NamedBuildTarget.WebGL, "DEVELOP");
        PlayerSettings.SetManagedStrippingLevel(NamedBuildTarget.WebGL, ManagedStrippingLevel.Low);
        PlayerSettings.stripUnusedMeshComponents = true;
        PlayerSettings.mipStripping = true;

        //publish setting
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;
        PlayerSettings.WebGL.dataCaching = true;
        PlayerSettings.WebGL.debugSymbolMode = WebGLDebugSymbolMode.Off;
        PlayerSettings.WebGL.powerPreference = WebGLPowerPreference.LowPower;
        PlayerSettings.WebGL.exceptionSupport = WebGLExceptionSupport.None;
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
}
#endif