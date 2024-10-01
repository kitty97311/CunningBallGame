using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreezePlayer : MonoBehaviour
{
    public Text timerText;
    public int playerNum;
    private bool isFreezing = false;

    // Start is called before the first frame update
    void Start()
    {
        timerText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoFreeze()
    {
        if (isFreezing) { return; }
        timerText.gameObject.SetActive(true);  // Activate the timer text
        StartCoroutine(FreezeCountDown());  // Start the countdown coroutine
    }

    // Coroutine to handle the countdown logic
    private IEnumerator FreezeCountDown()
    {
        int countdownValue = 10;  // Start countdown from 10

        while (countdownValue > 0)
        {
            isFreezing = true;
            GameObject.FindWithTag("Player" + playerNum).GetComponent<CPUAi>().isFrozen = true;
            timerText.text = countdownValue.ToString();  // Update the text with the countdown value
            yield return new WaitForSeconds(1f);  // Wait for 1 second
            countdownValue--;  // Decrease the countdown value
        }

        // Once the countdown is done
        this.gameObject.SetActive(false);
        GameObject.FindWithTag("Player" + playerNum).GetComponent<CPUAi>().isFrozen = false;
        isFreezing = false;
    }
}
