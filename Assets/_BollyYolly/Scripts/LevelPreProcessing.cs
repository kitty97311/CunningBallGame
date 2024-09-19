using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPreProcessing : MonoBehaviour
{
    ////bool[] arrayOfProcess = new bool[1];
    //private void Start()
    //{
    //    //SetLevelReadyStatus(false);
    //    //StartCoroutine(PreProcessingStatusChecker());
    //}
    //IEnumerator PreProcessingStatusChecker()
    //{
    //    Debug.Log("Checking001");
    //    bool ready = CheckIfAllProcessComplete();
    //    while (!ready)
    //    {
    //        Debug.Log("Checking");
    //        if (FindObjectOfType<GoalGrillShrinker>() == null)
    //        {
    //            SetLevelReadyStatus(true);
    //            yield break;
    //        }
            
    //        {
    //            GoalGrillShrinker go = FindObjectOfType<GoalGrillShrinker>();
    //            arrayOfProcess = new bool[1];
    //            //for (int i = 0; i < go.Length; i++)
    //            //{
    //            //    arrayOfProcess[i] = go[i].IP1();
    //            //}
    //            arrayOfProcess[0] = go.IP1();
    //        }
    //        ready = CheckIfAllProcessComplete();
    //    }

    //    SetLevelReadyStatus(true);

    //    Debug.Log("Level Start");
    //}
    //bool CheckIfAllProcessComplete()
    //{
    //    for (int i = 0; i < arrayOfProcess.Length; i++)
    //    {
    //        if (!arrayOfProcess[i])
    //        {
    //            return arrayOfProcess[i];
    //        }
    //    }
    //    return true;
    //}
    //public void SetLevelReadyStatus(bool isReady)
    //{
    //    LevelReadyCheck.isLevelReady = isReady;
    //}
}
