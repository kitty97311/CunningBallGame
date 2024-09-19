using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerWaitRoom : MonoBehaviour
{
    string roomCode;
    [SerializeField] GameObject privateHeader;
    public void DisablePrivateRoomFields()
    {
        privateHeader.SetActive(false);
    }
    public void ShareRoomCode()
    {
        ShareIntent.IntentShareText(roomCode, "Header");
    }
    public void CopyRoomCode()
    {
        CopyToClipboard(roomCode);
    }
    void CopyToClipboard(string str)
    {
        TextEditor textEditor = new TextEditor();
        textEditor.text = str;
        textEditor.SelectAll();
        textEditor.Copy();
    }
}
