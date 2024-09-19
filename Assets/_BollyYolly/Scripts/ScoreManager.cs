using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScoreManager : MonoBehaviour
{
    public event EventHandler<OnGoalScoredEventArgs> OnGoalScored;
    public class OnGoalScoredEventArgs : EventArgs
    {
        public string playerTag;
        public string tagOfBall;
        public BallTypes typeOfBall;
        public int lastHitPlayer;
        public bool ifIsColored;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            OnGoalScored?.Invoke(this, new OnGoalScoredEventArgs { playerTag = gameObject.tag, typeOfBall = other.GetComponent<Ball>().ball.ballType, ifIsColored = other.GetComponent<Ball>().ball.isColoured});
        }
    } 
    IEnumerator AddBlockerAndLightningEffect()
    {
        yield return new WaitForSeconds(3.2f);
        transform.GetChild(0).gameObject.SetActive(true);
    }
}


//if (other.GetComponent<Ball>().ball.ballType == BallTypes.Thunder || other.GetComponent<Ball>().ball.ballType == BallTypes.Fire)
//{
//    StartCoroutine(AddBlockerAndLightningEffect());
//}