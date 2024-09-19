using UnityEngine;
using UnityEditor;


[InitializeOnLoad]
public class BuildPipeline
{

    static BuildPipeline()
    {
        // Signing Keystore
        PlayerSettings.Android.keystorePass = "faris@123";
        PlayerSettings.Android.keyaliasName = "farisjastania";
        PlayerSettings.Android.keyaliasPass = "faris@123";
    }

}
