using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShareIntent
{
    static AndroidJavaObject sendIntent;
    static AndroidJavaClass IntentClass;

    static AndroidJavaClass UnityPlayer;
    static AndroidJavaObject currentActivity;
    static AndroidJavaObject chooser;

    static bool IsInitialized = false;

    static void Initialize()
    {
        if (IsInitialized)
            return;
        IsInitialized = true;

        string className = "android.content.Intent";
        IntentClass = new AndroidJavaClass(className);
        sendIntent = new AndroidJavaObject(className);

        UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        chooser = IntentClass.CallStatic<AndroidJavaObject>("createChooser", sendIntent, "Invite friends to play");
    }
    public static void IntentShareText(string textMessage, string header)
    {
        Debug.Log("Intent called");
        if (Application.platform != RuntimePlatform.Android)
            return;

        Debug.Log("Intent called Android");
#if UNITY_ANDROID
        Debug.Log("Intent called Android 2");
        if (!IsInitialized)
            Initialize();

        sendIntent.Call<AndroidJavaObject>("setAction", IntentClass.GetStatic<string>("ACTION_SEND"));
        sendIntent.Call<AndroidJavaObject>("putExtra", IntentClass.GetStatic<string>("EXTRA_SUBJECT"), header);
        sendIntent.Call<AndroidJavaObject>("putExtra", IntentClass.GetStatic<string>("EXTRA_TEXT"), textMessage);
        sendIntent.Call<AndroidJavaObject>("setType", "text/plain");

        currentActivity.Call("startActivity", chooser);
#endif
    }
}
