using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ScriptableLevels", menuName = "Levels")]
public class LevelsScriptable : ScriptableObject
{
    public float gameTime;
    public int spawnFrequency;
    public int maxBallsOnStage;
    public int howManyCanBePresentInCourt;
    public int ballToSave;
    public bool UnlockLevel;
    public List<BallTypes> typeOfBalls;
    public List<int> timeIntervalsToInstBalls;

    [Header("In Game Money$")]

    public int levelNumber;
    public long entryFees;
    public long firstPlacePrize;
}
   

