using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallTypes { Iron, Ice, Magnet, Thunder, Speed, Fire, Red, Yellow, Blue, Green,  none}
[CreateAssetMenu(fileName = "New ball Scriptable", menuName = "Ball Scriptable")]
public class BallScripatable : ScriptableObject
{
    public GameObject ballPrefab;
    public int scoring;
    public BallTypes ballType;
    public bool isColoured;
}
