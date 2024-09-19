using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class LevelScreenButtonDataSetter : MonoBehaviour
{
    GameOverDataPublisher gameOverDataPublisher;
    void Start()
    {
        gameOverDataPublisher = FindObjectOfType<GameOverDataPublisher>();
        SetData();
    }
    void SetData()
    {
        Debug.Log(gameOverDataPublisher);
        LevelsScriptable[] l = gameOverDataPublisher.levels;
        for (int i = 0; i < l.Length; i++)
        {
            transform.GetChild(1).GetChild(0).GetComponent<Text>().text = l[i].firstPlacePrize.ToString();
            transform.GetChild(2).GetChild(0).GetComponent<Text>().text = l[i].entryFees.ToString();
        }
    }
}
