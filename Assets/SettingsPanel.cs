using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsPanel : MonoBehaviour
{
    PlayerMechanics playerMechanics;
    LevelGameManager levelGameManager;
    GlobalSceneManager globalSceneManager;

    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sensitivitySlider;

    [SerializeField] Text sensitivityValue;
    [SerializeField] TextMeshProUGUI playerNameHomescreen;
    //player name/username reference for the homescreen

    AudioSource[] musicAudioSource;
    AudioListener mainAudioListner;
    List<AudioSource> allAudioSources = new List<AudioSource>();

    [SerializeField] int sensitivityRoundTo = 2;

    PlayerData playerDataToSet;

    [SerializeField] GameObject settingMenuButton;
    //settings menu button InLevel
    private void Start()
    {
        if (PlayerInstance.playerInstance.playerData.secondStart == false)
        {
            Debug.Log("First");
            SetInitialValues();
        }
        else
        {
            levelGameManager = FindObjectOfType<LevelGameManager>();
            SetDataLoadFromPlayerInstance();
        }
        StartNew();
        gameObject.SetActive(false);
    }
    void SetInitialValues()
    {
        Debug.Log("SetInitialValues");
        PlayerInstance.playerInstance.playerData.musicSetting = 1;
        PlayerInstance.playerInstance.playerData.sfxSetting = 1;
        PlayerInstance.playerInstance.playerData.sensitivityValue = 5 / 5000f;
        PlayerInstance.playerInstance.playerData.secondStart = true;
        PlayerInstance.playerInstance.SavePlayer();

        Debug.Log(PlayerInstance.playerInstance.playerData.sensitivityValue);

        sfxSlider.value = 1;
        musicSlider.value = 1;
        sensitivitySlider.value = 5;

        Debug.Log(sensitivitySlider.value);
    }
    void StartNew() 
    {
        if (FindObjectOfType<PlayerMechanics>() != null)
        {
            playerMechanics = FindObjectOfType<PlayerMechanics>();
        }
        if (FindObjectOfType<LevelGameManager>() != null)
        {
            musicAudioSource = FindObjectOfType<LevelGameManager>().GetComponents<AudioSource>();
        }

        if (Camera.main == null)
        {
            mainAudioListner = Camera.main.GetComponent<AudioListener>();
        }
        globalSceneManager = FindObjectOfType<GlobalSceneManager>();

        {
            sfxSlider.onValueChanged.AddListener(delegate { HandleSfx(); });
            musicSlider.onValueChanged.AddListener(delegate { HandleMusic(); });
            sensitivitySlider.onValueChanged.AddListener(delegate { HandleSensitivity(); });
        }

        HandleInitialValues();
        if (settingMenuButton == null) return;
    }
    /// <summary>
    /// Handles the values for music, sfx and sensitivity
    /// settings initially
    /// </summary>
    void HandleInitialValues()
    {
        HandleMusic();
        HandleSfx();
        HandleSensitivity();
    }
    private void OnEnable()
    {
        //Debug.Log(PlayerInstance.playerInstance.playerData.sensitivityValue * 2000);
        Time.timeScale = 0;
        if (PlayerInstance.playerInstance.playerData.secondStart)
        {
            //Debug.Log("2nd start First");
            SetDataLoadFromPlayerInstance();
            HandleInitialValues();
        }

        if (settingMenuButton == null) return;
        settingMenuButton.SetActive(false);
    }
    private void OnDisable()
    {
        //Debug.Log(PlayerInstance.playerInstance.playerData.musicSetting + " Music VAL");
        //Debug.Log(PlayerInstance.playerInstance.playerData.sfxSetting + " SFX VAL");
        //Debug.Log(PlayerInstance.playerInstance.playerData.sensitivityValue * 2000 + " Sensitivity VAL");
        //PlayerInstance.playerInstance.playerData = playerDataToSet;
        PlayerInstance.playerInstance.SavePlayer();
        Time.timeScale = 1;
        if (settingMenuButton != null)
        {
            settingMenuButton.SetActive(true);
        }
    }

    /// <summary>
    /// Loads the data from Player Instance
    /// and sets the values for the settings
    /// menu
    /// </summary>
    void SetDataLoadFromPlayerInstance()
    {
        PlayerInstance.playerInstance.LoadPlayer();
        playerDataToSet = PlayerInstance.playerInstance.playerData;

        ////
        ////Debug.Log(playerDataToSet.musicSetting + " Music VAL");
        ////Debug.Log(playerDataToSet.sfxSetting + " SFX VAL");
        ////Debug.Log(playerDataToSet.sensitivityValue + " Sensitivity VAL");
        ////
        ////sfxSlider.value = playerDataToSet.sfxSetting;
        ////musicSlider.value = playerDataToSet.musicSetting;
        ////
        
        sfxSlider.value = PlayerInstance.playerInstance.playerData.sfxSetting;
        //sets the value of SFX slider
        musicSlider.value = PlayerInstance.playerInstance.playerData.musicSetting;
        //sets the value of Music slider

        ////
        ////decimal roundTo = System.Math.Round((decimal)playerDataToSet.sensitivityValue * (decimal)(5000f), sensitivityRoundTo);
        ////sensitivityValue.text = roundTo.ToString();
        ////sensitivitySlider.value = playerDataToSet.sensitivityValue * (5000f);
        ////
        
        decimal roundTo = System.Math.Round((decimal)PlayerInstance.playerInstance.playerData.sensitivityValue * (decimal)(5000f), sensitivityRoundTo);
        //rounds the sensitivity to 2 digits
        sensitivityValue.text = roundTo.ToString();
        //shows sensitivity value on settings panel (not is use) 
        sensitivitySlider.value = PlayerInstance.playerInstance.playerData.sensitivityValue * (5000f);
        //sets the sensitivity value to the sensitivity handler

        if (playerMechanics == null) return;
        playerMechanics.PlayerSpeed = sensitivitySlider.value / (5000f);
    }

    /// <summary>
    /// Handles the sfx values and sets in GamePrefs
    /// and fires a method in levelGameManager HandleSfx()
    /// </summary>
    void HandleSfx()
    {
        //Debug.Log("Handling SFX");
        if (sfxSlider.value == 0)
        //if slider in 0 (off)
        {
            if(mainAudioListner != null)
            //is no audioListner i.e. HomeScreen
            {
                GamePrefs.ifSfxEnabled = false; 
                PlayerInstance.playerInstance.playerData.sfxSetting = 0;
                //sets sfxSetting in playerInstance to 0 (off)

                if (levelGameManager != null)
                levelGameManager.HandleSfx();
                //Handles audio(goal) in LevelGameManager
            }
            PlayerInstance.playerInstance.playerData.sfxSetting = 0;
            //sets sfxSetting in playerInstance to 0 (off)
        }
        else
        //if slider in 1 (on)
        {
            if (mainAudioListner != null)
            //is no audioListner i.e. HomeScreen
            {
                GamePrefs.ifSfxEnabled = true;
                PlayerInstance.playerInstance.playerData.sfxSetting = 1;
                //sets sfxSetting in playerInstance to 1 (on)

                if (levelGameManager != null)
                levelGameManager.HandleSfx();
                //Handles audio(goal) in LevelGameManager
            }
            PlayerInstance.playerInstance.playerData.sfxSetting = 1;
            //sets sfxSetting in playerInstance to 1 (on)
        }
        //Debug.Log(PlayerInstance.playerInstance.playerData.sfxSetting + "VAL");
    }

    /// <summary>
    /// Handles the music values and sets in GamePrefs
    /// and fires a method in levelGameManager HandleSfx()
    /// </summary>
    void HandleMusic()
    {
        if (musicSlider.value == 0)
        //if slider in 0 (off)
        {
            if (musicAudioSource != null)
            {
                PlayerInstance.playerInstance.playerData.musicSetting = 0;
                //sets sfxSetting in playerInstance to 0 (off)
                musicAudioSource[1].volume = 0;
                //sets volume to 0 (music)
            }
            PlayerInstance.playerInstance.playerData.musicSetting = 0;
            //sets sfxSetting in playerInstance to 0 (off)
        }
        else
        //if slider in 1 (on)
        {
            if (musicAudioSource != null)
            {
                PlayerInstance.playerInstance.playerData.musicSetting = 1;
                //sets sfxSetting in playerInstance to 1 (on)
                musicAudioSource[1].volume = 1;
                //sets volume to 0 (music)
            }
            PlayerInstance.playerInstance.playerData.musicSetting = 1;
            //sets sfxSetting in playerInstance to 1 (on)
        }
        //Debug.Log(PlayerInstance.playerInstance.playerData.musicSetting + "VAL");
    }


    void HandleSensitivity()
    {
        if (playerMechanics != null)
        {
            playerMechanics.PlayerSpeed = sensitivitySlider.value / (5000f);
        }

        decimal roundTo = System.Math.Round((decimal)sensitivitySlider.value, sensitivityRoundTo);
        sensitivityValue.text = roundTo.ToString();
        PlayerInstance.playerInstance.playerData.sensitivityValue = sensitivitySlider.value / (5000f);


        Debug.Log(PlayerInstance.playerInstance.playerData.sensitivityValue + "VAL");
    }


    public void ButtonToToggle(Slider slider)
    {
        if (slider.value == 0)
        {
            slider.value = 1;
        }
        else
        if (slider.value == 1)
        {
            slider.value = 0;
        }
    }
    void GetAllAudioSourceButMusic()
    {
        allAudioSources.AddRange(FindObjectsOfType<AudioSource>());

        foreach (AudioSource item in allAudioSources)
        {
            if (item.playOnAwake == false)
            {
                allAudioSources.Remove(item);
            }
        }
    }
    void ForceEndGame()
    {
        levelGameManager = FindObjectOfType<LevelGameManager>();
        levelGameManager.ForceEndGame();
        Time.timeScale = 1;
        globalSceneManager.LoadHomeScreen();
        gameObject.SetActive(false);
    }
    void ToggleSfx(int value)
    {
        foreach (AudioSource item in allAudioSources)
        {
            item.volume = value;
        }
    }
}
