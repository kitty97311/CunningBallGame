using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public BallScripatable ball;
    public Colour colour;
    public int lastHitPlayer;
    private void Awake()
    {
        if (PlayerInstance.playerInstance.playerData.sfxSetting == 0)
        {
            GetComponent<AudioSource>().volume = 0;
        }
        else
        if (PlayerInstance.playerInstance.playerData.sfxSetting == 1)
        {
            GetComponent<AudioSource>().volume = 1;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (PlayerInstance.playerInstance.playerData.sfxSetting == 0)
        {
            GetComponent<AudioSource>().volume = 0;
        }
        else
        if (PlayerInstance.playerInstance.playerData.sfxSetting == 1)
        {
            GetComponent<AudioSource>().volume = 1;
        }

        string lastHitPlayerTag = collision.gameObject.tag;

        if (collision.gameObject.GetComponent<PlayerColourData>() == null) return;

        if (collision.gameObject.GetComponent<PlayerColourData>().colourOfThisPlayer == this.colour) return;


        switch (lastHitPlayerTag)
        {
            case "Player1":
                lastHitPlayer = 1;
                break;
            case "Player2":
                lastHitPlayer = 2;
                break;
            case "Player3":
                lastHitPlayer = 3;
                break;
            case "Player4":
                lastHitPlayer = 4;
                break;
        }
    }
}
