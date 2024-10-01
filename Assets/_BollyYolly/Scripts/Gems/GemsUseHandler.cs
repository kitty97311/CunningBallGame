using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void GemUseButtonClicked();
public class GemsUseHandler : MonoBehaviour
{
    public event GemUseButtonClicked OnGemUseButtonClicked;

    [SerializeField] Button useGemsButton;
    [SerializeField] Image diamondImage;
    [SerializeField] Image timeImage;
    [SerializeField] Text diamondText;

    bool ifGemsUsedOnce = false;

    private void Start()
    {
        if (PlayerInstance.Instance.Player.gem <= 0)
        {
            //useGemsButton.interactable = false;
            SetButtonNonInteractable();
        }
    }


    public void ProcessAddingTime()
    //mapped to use gems button
    {
        if (ifGemsUsedOnce) return;

        GemUseButtonClickedEvent();

        //useGemsButton.interactable = false;
        SetButtonNonInteractable();
        ifGemsUsedOnce = true;
    }
    void GemUseButtonClickedEvent()
    {
        if (OnGemUseButtonClicked != null)
        {
            OnGemUseButtonClicked();
        }
    }
    void SetButtonNonInteractable()
    {
        useGemsButton.interactable = false;
        diamondImage.color = new Color(255 / 255, 255 / 255, 255 / 255, 0.3f);
        timeImage.color = new Color(255 / 255, 255 / 255, 255 / 255, 0.3f);
        diamondText.color = new Color(255 / 255, 255 / 255, 255 / 255, 0.3f);
    }
}
