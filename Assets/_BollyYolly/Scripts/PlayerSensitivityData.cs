using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSensitivityData : MonoBehaviour
{
    PlayerMechanics playerMechanics;
    [SerializeField] Text sensitivityValuesInUI;
    [SerializeField] Slider sensitivitySlider;
    void Start()
    {
        playerMechanics = FindObjectOfType<PlayerMechanics>();
        if (PlayerPrefs.HasKey("PlSpeed"))
        {
            playerMechanics.PlayerSpeed = PlayerPrefs.GetFloat("PlSpeed");
            sensitivityValuesInUI.text = playerMechanics.PlayerSpeed.ToString();
        }
        else
        {
            PlayerPrefs.SetFloat("PlSpeed", playerMechanics.PlayerSpeed);
            sensitivityValuesInUI.text = playerMechanics.PlayerSpeed.ToString();
            sensitivitySlider.value = playerMechanics.PlayerSpeed;
        }
        sensitivitySlider.onValueChanged.AddListener( delegate { UpdateSensitivityValues(); });
    }
    private void OnEnable()
    {
        Time.timeScale = 0;
    }
    void UpdateSensitivityValues()
    {
        float value = sensitivitySlider.value;
        sensitivityValuesInUI.text = value.ToString();
        playerMechanics.PlayerSpeed = value;
        PlayerPrefs.SetFloat("PlSpeed", value);
        Debug.Log("Sensitivity Value is " + value);
    }
    public void ClosePanel()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
