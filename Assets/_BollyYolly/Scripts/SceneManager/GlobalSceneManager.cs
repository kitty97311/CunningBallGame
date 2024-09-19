using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalSceneManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void ReloadScene()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        //gets current scene buildindex
        SceneManager.LoadScene(currentScene);
    }
    public void LoadPrivateRoom()
    {
        SceneManager.LoadScene("PrivateRoom");
    }
    public void LoadPrivateRoomCreateJoin()
    {
        SceneManager.LoadScene("PrivateRoomTwo");
    }
    public void LoadNextScene()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        //gets current scene buildindex
        SceneManager.LoadScene(currentScene + 1);
    }
    public void LoadStartScreen()
    //loads start screen
    {
        SceneManager.LoadScene(Constants.SCENE_START);
    }
    public void LoadLoginScreen()
    //loads home screen
    {
        SceneManager.LoadScene(Constants.SCENE_LOGIN);
    }
    public void LoadHomeScreen()
    //loads home screen
    {
        SceneManager.LoadScene(Constants.SCENE_START);
    }
    public void LoadLevelsScreen()
    //loads levels selection screen
    {
        SceneManager.LoadScene(Constants.SCENE_LEVEL_SELECTION);
    }
    public void LoadMultiplayerWaitScreen()
    {
        SceneManager.LoadScene("MultiplayerWaitScreen");
    }
    public string GetCurrentSceneName()
    //get current scene by name
    {
        return SceneManager.GetActiveScene().name;
    }
    public int GetCurrentLevelNumber()
    //get current stage/level number (digit)
    {
        string sceneName;
        sceneName = SceneManager.GetActiveScene().name;
        int levelNumber = (int)char.GetNumericValue(sceneName[6]);
        if (sceneName.Length > 7)
        {
            string levelNumb = new string(new char[] { sceneName[6], sceneName[7] });
            levelNumber = int.Parse(levelNumb);
        }
        return levelNumber;
    }
}
