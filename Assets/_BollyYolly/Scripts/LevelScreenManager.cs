using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelScreenManager : MonoBehaviour
{
    [SerializeField] GameObject levelSelect;
    [SerializeField] GameObject loadingText;
    [SerializeField] Text Coins;
    [SerializeField] Text Gems;

    public LevelsScriptable[] level;
    public GameObject levelSelector;
    public GameObject notEnoughCoins;
    public GameObject levelNotUnlocked;

    [Header("Level buttons")]
    [SerializeField] private Button[] levelButtons;
    //public PlayerInstance playerInstance;

    private void Start()
    {
        VerifyLevelUnlockStatus();
    }

    private void VerifyLevelUnlockStatus()
    {
        /*int heightesLevelWon = PlayerInstance.Instance.Settttt.eligibleForLevel + 1;*/
        //kittymark
        int heightesLevelWon = 10;

        for (int i = 0; i < levelButtons.Length; i++)
        {
            /*
              Unlocked = None =0
                Value +1 = 1
             0 <1
            1<1 : F
            2<1 : F
            3<1 : F
            4<1 : F

            Unlocked = 1
            Value +1 = 2
            0<2 : T
            1<2 : T
            2<2
             */
            //levelButtons[i].interactable = (i < heightesLevelWon);
            //if (!(i < heightesLevelWon))
            //{
            //    LockLevel(i);
            //}
            //else
            //{
            //    UnlockLevel(i);
            //}

            UnlockLevel(i);
        }
    }
    /// <summary>
    /// Sets the play previous level image to visible
    /// and play buttons, coins gems info to invisible/disable
    /// </summary>
    /// <param name="level"></param>
    void LockLevel(int level)

    ///
    {
        levelButtons[level].gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        levelButtons[level].gameObject.transform.GetChild(2).gameObject.SetActive(false);
        levelButtons[level].gameObject.transform.GetChild(3).gameObject.SetActive(false);
        levelButtons[level].gameObject.transform.GetChild(4).gameObject.SetActive(false);
        levelButtons[level].gameObject.transform.GetChild(5).gameObject.SetActive(false);
        levelButtons[level].gameObject.transform.GetChild(6).gameObject.SetActive(false);
    }
    void UnlockLevel(int level)
    ///Sets the play previous level image to invisible
    ///and play buttons, coins gems info to visible/enable
    {
        levelButtons[level].gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        levelButtons[level].gameObject.transform.GetChild(2).gameObject.SetActive(true);
        levelButtons[level].gameObject.transform.GetChild(3).gameObject.SetActive(true);
        levelButtons[level].gameObject.transform.GetChild(4).gameObject.SetActive(true);
        levelButtons[level].gameObject.transform.GetChild(5).gameObject.SetActive(true);
        //levelButtons[level].gameObject.transform.GetChild(6).gameObject.SetActive(true);
    }

    //Assuming that level is getting passed with same number i.e. for level 1, passing int 1
    public void LoadLevel(int levelNumber)
    {
        if (PlayerInstance.Instance.Player.coin >= level[levelNumber - 1].entryFees)
        {
            PlayerInstance.Instance.CurrentLevelNumber = levelNumber;
            PlayerInstance.Instance.DeductCoins(level[levelNumber - 1].entryFees);
            //kittymark
            /*PlayerInstance.Instance.Settttt.currentLevelNumber = levelNumber;*/
            Coins.text = Utility.NumberToWordConverted(PlayerInstance.Instance.Player.coin);
            Gems.text = Utility.NumberToWordConverted(PlayerInstance.Instance.Player.gem);
            PlayerPrefs.SetString(Constants.PREF_SELECTED_LEVEL, "Level " + (levelNumber));
            SceneManager.LoadScene(Constants.SCENE_CHARACTER_SELECTION);
            levelSelect.SetActive(false);
            loadingText.SetActive(true);
        }
        else
        {
            NotEnoughCoins(true);
        }
    }

    public void UnLockAllLevels()
    {
        //kittymark
        /*PlayerInstance.Instance.Settttt.eligibleForLevel = 15;*/
        VerifyLevelUnlockStatus();
    }

    public void GoToHome()
    {
        SceneManager.LoadScene(Constants.SCENE_START);
    }

    private void Update()
    {
        //PlayerInstance.pb.data.levelUnlocked[0] = true;
        Coins.text = /*"Coins:" +*/ Utility.NumberToWordConverted(PlayerInstance.Instance.Player.coin);
        Gems.text = Utility.NumberToWordConverted(PlayerInstance.Instance.Player.gem);
        //Debug.Log(PlayerInstance.pb.data.currentLevelNumber);
    }

    public void NotEnoughCoins(bool state)
    {
        notEnoughCoins.SetActive(state);
    }

}
