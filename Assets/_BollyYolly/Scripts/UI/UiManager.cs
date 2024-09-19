using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum UiScreens
{
    HomeScreen,
    CoinsPurcahse,
    GemsPurcahse,
    ProfilePanel,
    SettingScreen,
    SharePanel,
    ConfirmationPopup,
    ComingSoonPanel,
    MultiplayerWaitPanel,
    MultiplayerWaitPanelNonPrivate,
    PrivateRoomCreateJoinPanel,
    JoinPrivateRoomPanel
}

public class UiManager : MonoBehaviour
{
    #region Variables

    [Header("UI Screens References")]
    [SerializeField] private GameObject multiplayerWaitPanelNonPrivate;
    [SerializeField] private GameObject privateRoomCreateJoinPanel;
    [SerializeField] private GameObject joinPrivateRoomPanel;
    [SerializeField] private GameObject multiplayerWaitPanel;
    [SerializeField] private GameObject coinsPurchasePanel;
    [SerializeField] private GameObject gemsPurchasePanel;
    [SerializeField] private GameObject noAdsPromptPanel;
    [SerializeField] private GameObject comingSoonPanel;
    [SerializeField] private GameObject homeScreenPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject profilePanel;
    [SerializeField] private GameObject sharePanel;

    [Header("Text lables")]
    [SerializeField] private TextMeshProUGUI playerNameHomescreen;

    internal UiScreens UiScreenStates;

    #endregion

    #region Init

    void Start()
    {
        if (PlayerInstance.playerInstance.playerData.playerName == null) return;
        playerNameHomescreen.text = PlayerInstance.playerInstance.playerData.playerName.ToString();
        UiScreenStates = UiScreens.HomeScreen;
        GoogleAdsMediation.OnNoAdsAvailable += ShowNoADsPanel;
    }
    private void OnEnable()
    {
        if (PlayerInstance.playerInstance.playerData.playerName == null) return;
        playerNameHomescreen.text = PlayerInstance.playerInstance.playerData.playerName.ToString();
    }
    private void OnDisable()
    {
        GoogleAdsMediation.OnNoAdsAvailable -= ShowNoADsPanel;
    }
    #endregion

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            //if on home screen
            //Ask for quck button
        }
    }

    private void HomeScreen(bool state)
    {
        DisableAllScreens();
        homeScreenPanel.SetActive(state);
        UiScreenStates = UiScreens.HomeScreen;
    }


    #region Store Screen

    public void CoinPanel(bool state)
    {
        DisableAllScreens();
        coinsPurchasePanel.SetActive(state);
        UiScreenStates = UiScreens.CoinsPurcahse;
    }

    public void GemsPanel(bool state)
    {
        DisableAllScreens();
        gemsPurchasePanel.SetActive(state);
        UiScreenStates = UiScreens.GemsPurcahse;
    }
    #endregion

    #region BackButton
    public void BackButton()
    {
        switch (UiScreenStates)
        {
            case UiScreens.HomeScreen:
                //TODO: Exit popup
                break;
            case UiScreens.CoinsPurcahse:
            case UiScreens.GemsPurcahse:
                HomeScreen(true);
                break;
            case UiScreens.ProfilePanel:
                HomeScreen(true);
                break;
            case UiScreens.SettingScreen:
                HomeScreen(true);
                break;
            case UiScreens.ConfirmationPopup:
                break;
            case UiScreens.SharePanel:
                HomeScreen(true);
                break;
            case UiScreens.ComingSoonPanel:
                HomeScreen(true);
                break;
            case UiScreens.MultiplayerWaitPanel:
                HomeScreen(true);
                break;
            case UiScreens.MultiplayerWaitPanelNonPrivate:
                HomeScreen(true);
                break;
            case UiScreens.PrivateRoomCreateJoinPanel:
                HomeScreen(true);
                break;
            default:
                break;
        }

    }


    #endregion


    public void SharePanel(bool state)
    {
        DisableAllScreens();
        sharePanel.SetActive(state);
        UiScreenStates = UiScreens.SharePanel;
    }
    public void ProfilePanel(bool state)
    {
        DisableAllScreens();
        profilePanel.SetActive(state);
        UiScreenStates = UiScreens.ProfilePanel;
    }
    public void SettingsScreen(bool state)
    {
        DisableAllScreens();
        settingsPanel.SetActive(state);
        UiScreenStates = UiScreens.SettingScreen;
    }
    public void ComingSoonPanel(bool state)
    {
        DisableAllScreens();
        comingSoonPanel.SetActive(state);
        UiScreenStates = UiScreens.ComingSoonPanel;
    }
    public void MultiplayerWaitPanel(bool state)
    {
        DisableAllScreens();
        multiplayerWaitPanel.SetActive(state);
        UiScreenStates = UiScreens.MultiplayerWaitPanel;
    }
    public void MultiplayerWaitPanelNonPrivate(bool state)
    {
        DisableAllScreens();
        multiplayerWaitPanelNonPrivate.SetActive(state);
        UiScreenStates = UiScreens.MultiplayerWaitPanelNonPrivate;
    }
    public void PrivateRoomCreateJoinPanel(bool state)
    {
        DisableAllScreens();
        privateRoomCreateJoinPanel.SetActive(state);
        UiScreenStates = UiScreens.PrivateRoomCreateJoinPanel;
    }
    public void JoinPrivateRoomPanel(bool state)
    {
        DisableAllScreens();
        joinPrivateRoomPanel.SetActive(state);
        UiScreenStates = UiScreens.PrivateRoomCreateJoinPanel;
    }
    void ShowNoADsPanel()
    {
        noAdsPromptPanel.SetActive(true);
    }
    public void HideNoADsPanel()
    {
        noAdsPromptPanel.SetActive(false);
    }
    public void DisableAllScreens()
    {
        multiplayerWaitPanelNonPrivate.SetActive(false);
        privateRoomCreateJoinPanel.SetActive(false);
        joinPrivateRoomPanel.SetActive(false);
        multiplayerWaitPanel.SetActive(false);
        coinsPurchasePanel.SetActive(false);
        gemsPurchasePanel.SetActive(false);
        homeScreenPanel.SetActive(false);
        comingSoonPanel.SetActive(false);
        settingsPanel.SetActive(false);
        profilePanel.SetActive(false);
        sharePanel.SetActive(false);
    }
}
