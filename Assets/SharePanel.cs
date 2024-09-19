using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharePanel : MonoBehaviour
{
    string playStoreLink = "https://play.google.com/store/apps/details?id=com.farisjastania.cunningball";
    //placeholder app link

    /// <summary>
    /// Calls share text in share intent
    /// class
    /// </summary>
    public void FireLinkIntent()
    {
        ShareIntent.IntentShareText("Hey, try this exciting game I am playing!!! Download using this link: " + playStoreLink, "Header");
        //ShareIntent.IntentShareText("Hey, See I am playing this amazing game. You can also download it from here: " + playStoreLink);
    }
}
