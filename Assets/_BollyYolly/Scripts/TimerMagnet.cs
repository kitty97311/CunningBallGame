using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerMagnet : MonoBehaviour
{
    PlayerBallInteraction playerBallInteraction;
    [SerializeField] Text timerText;
    [SerializeField]Image swipeEffectImage;

    bool isSwiping;
    private void Start()
    {
        playerBallInteraction = GetComponentInParent<PlayerBallInteraction>();
        Debug.Log(playerBallInteraction);
    }
    void Update()
    {
        timerText.text = playerBallInteraction.timeLeftToReleaseBall.ToString();
        if (!isSwiping)
        {
            swipeEffectImage.fillAmount = 1;
            StartCoroutine(SwipeEffect(playerBallInteraction.timeLeftToReleaseBall));
        }
    }
    IEnumerator SwipeEffect(float totalTime)
    {
        isSwiping = true;
        float divisionValue = 360 / totalTime;
        totalTime /= 3;
        while (true)
        {
            yield return new WaitForSeconds((1/divisionValue) * 2);
            swipeEffectImage.fillAmount = totalTime;
            totalTime -= 1/divisionValue;
        }
    }
}
