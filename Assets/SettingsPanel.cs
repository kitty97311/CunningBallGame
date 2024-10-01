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
    /*[SerializeField] TextMeshProUGUI playerNameHomescreen;*/
    //player name/username reference for the homescreen

    AudioSource[] musicAudioSource;
    AudioListener mainAudioListner;
    List<AudioSource> allAudioSources = new List<AudioSource>();

    [SerializeField] int sensitivityRoundTo = 2;

    [SerializeField] GameObject settingMenuButton;
    //settings menu button InLevel
    private void Start()
    {
        SetDataLoadFromPlayerInstance();
        StartNew();
        gameObject.SetActive(false);
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
        Time.timeScale = 0;
        if (settingMenuButton == null) return;
        settingMenuButton.SetActive(false);
    }
    private void OnDisable()
    {
        PlayerInstance.Instance.SaveSetting();
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
        sfxSlider.value = PlayerInstance.Instance.Setting.sfx ? 1 : 0;
        musicSlider.value = PlayerInstance.Instance.Setting.music ? 1 : 0;
        sensitivitySlider.value = PlayerInstance.Instance.Setting.sensitivity;
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
                PlayerInstance.Instance.Setting.sfx = false;
                //sets sfxSetting in playerInstance to 0 (off)

                if (levelGameManager != null)
                levelGameManager.HandleSfx();
                //Handles audio(goal) in LevelGameManager
            }
            PlayerInstance.Instance.Setting.sfx = false;
            //sets sfxSetting in playerInstance to 0 (off)
        }
        else
        //if slider in 1 (on)
        {
            if (mainAudioListner != null)
            //is no audioListner i.e. HomeScreen
            {
                GamePrefs.ifSfxEnabled = true;
                PlayerInstance.Instance.Setting.sfx = true;
                //sets sfxSetting in playerInstance to 1 (on)

                if (levelGameManager != null)
                levelGameManager.HandleSfx();
                //Handles audio(goal) in LevelGameManager
            }
            PlayerInstance.Instance.Setting.sfx = true;
            //sets sfxSetting in playerInstance to 1 (on)
        }
        //Debug.Log(PlayerInstance.Instance.Settttt.sfxSetting + "VAL");
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
                PlayerInstance.Instance.Setting.music = false;
                //sets sfxSetting in playerInstance to 0 (off)
                musicAudioSource[1].volume = 0;
                //sets volume to 0 (music)
            }
            PlayerInstance.Instance.Setting.music = false;
            //sets sfxSetting in playerInstance to 0 (off)
        }
        else
        //if slider in 1 (on)
        {
            if (musicAudioSource != null)
            {
                PlayerInstance.Instance.Setting.music = true;
                //sets sfxSetting in playerInstance to 1 (on)
                musicAudioSource[1].volume = 1;
                //sets volume to 0 (music)
            }
            PlayerInstance.Instance.Setting.music = true;
            //sets sfxSetting in playerInstance to 1 (on)
        }
        //Debug.Log(PlayerInstance.Instance.Settttt.musicSetting + "VAL");
    }


    void HandleSensitivity()
    {
        playerMechanics.playerSpeed = sensitivitySlider.value / (500f);

        sensitivityValue.text = sensitivitySlider.value.ToString();
        PlayerInstance.Instance.Setting.sensitivity = (int) sensitivitySlider.value;
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
