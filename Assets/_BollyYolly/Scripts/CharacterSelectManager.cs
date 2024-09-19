using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class CharacterSelectManager : MonoBehaviour
{
    int player1Selection = 0; //for singleplayer
    [SerializeField] GameObject[] loadingDisplay;

    [SerializeField] GameObject monkeySelectedGlow;
    [SerializeField] GameObject boySelectedGlow;
    [SerializeField] GameObject crocodileSelectedGlow;
    [SerializeField] GameObject girlSelectedGlow;

    bool playerSelected = false;

    GlobalSceneManager globalSceneManager;
    private void Start()
    {
        if(!PlayerPrefs.HasKey("SelectedLevel"))
        {
            PlayerPrefs.SetString("SelectedLevel", "Level 1");
        }
        globalSceneManager = FindObjectOfType<GlobalSceneManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            globalSceneManager.LoadHomeScreen();
            Debug.Log("Escape");
        }
    }

    public void SelectBoy()
    {
        if (playerSelected) return;
        playerSelected = true;
        HideAllSelectedPanelGlowStroke();
        boySelectedGlow.SetActive(true);
        player1Selection = 0;
        StartCoroutine(LoadLevel());
    }
    public void SelectCroc()
    {
        if (playerSelected) return;
        playerSelected = true;
        HideAllSelectedPanelGlowStroke();
        crocodileSelectedGlow.SetActive(true);
        player1Selection = 1;
        StartCoroutine(LoadLevel());
    }
    public void SelectGirl()
    {
        if (playerSelected) return;
        playerSelected = true;
        HideAllSelectedPanelGlowStroke();
        girlSelectedGlow.SetActive(true);
        player1Selection = 2;
        StartCoroutine(LoadLevel());
    }
    public void SelectMonkey()
    {
        if (playerSelected) return;
        playerSelected = true;
        HideAllSelectedPanelGlowStroke();
        monkeySelectedGlow.SetActive(true);
        player1Selection = 3;
        StartCoroutine(LoadLevel());
    }
    IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(1f);
        loadingDisplay[0].SetActive(false);
        loadingDisplay[1].SetActive(true);
        PlayerPrefs.SetInt("Player1Selection", player1Selection);
        SceneManager.LoadScene(PlayerPrefs.GetString("SelectedLevel"));
    }
    ///<summary>
    ///Hides other character panels' 
    ///glow stroke
    ///</summary>
    void HideAllSelectedPanelGlowStroke()
    {
        crocodileSelectedGlow.SetActive(false);
        monkeySelectedGlow.SetActive(false);
        girlSelectedGlow.SetActive(false);
        boySelectedGlow.SetActive(false);
    }
}
