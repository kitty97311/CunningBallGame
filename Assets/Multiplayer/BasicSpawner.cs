using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using UnityEngine.SceneManagement;

public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks 
{
    PlayerMechanics[] playerMechanicsAllPlayersArray = new PlayerMechanics[4];

    [SerializeField] private NetworkPrefabRef[] playerCarPrefabCollection;
    [SerializeField] private GameObject[] spawnPosArray;
    [SerializeField] NetworkPlayer networkPlayer;
    PlayerMechanics playerMech;
    private Dictionary<PlayerRef, NetworkObject> spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    private NetworkRunner runner;

    [SerializeField] GameObject multiplayerCamerasPlayer1;
    [SerializeField] GameObject multiplayerCamerasPlayer2;
    [SerializeField] GameObject multiplayerCamerasPlayer3;
    [SerializeField] GameObject multiplayerCamerasPlayer4;
    int totalNumberOfPlayers;

    PlayerInputHandler playerInputHandler;
    void Start()
    {
        StartGame(GameMode.AutoHostOrClient);
    }
    async void StartGame(GameMode mode)
    {
        runner = gameObject.AddComponent<NetworkRunner>();
        runner.ProvideInput = true;

        await runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        } );
    }
    NetworkPrefabRef DecideCharacterToSpawnAsFirst()
    {
        int selectedPlayer = PlayerPrefs.GetInt("Player1Selection");
        return playerCarPrefabCollection[selectedPlayer];
    }
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        Debug.Log("Start Input Recieved");
        if (playerInputHandler == null && NetworkPlayer.Local != null)
            playerInputHandler = NetworkPlayer.Local.GetComponent<PlayerInputHandler>();

        if (playerInputHandler != null && playerInputHandler.isUpdated)
            input.Set(playerInputHandler.GetNetworkInputData());

        //if (playerInputHandler != null)
        //{
        //    //Debug.Log(playerInputHandler.gameObject.transform.parent.name + " Name");
        //}
    }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer) 
        {
            switch (FindObjectsOfType<NetworkPlayer>().Length)
            {
                case 0:
                    SpawnPlayers(0, player);
                    break;
                case 1:
                    SpawnPlayers(1, player);
                    break;
                case 2:
                    SpawnPlayers(2, player);
                    break;
                case 3:
                    SpawnPlayers(3, player);
                    break;
                default:
                    return;
            }
        }
    }
    void SpawnPlayers(int playersAlreadyPresent, PlayerRef player)
    {
        NetworkObject networkPlayerObject = runner.Spawn(DecideCharacterToSpawnAsFirst(), spawnPosArray[playersAlreadyPresent].transform.position,Quaternion.identity, player);

        networkPlayerObject.transform.localEulerAngles = spawnPosArray[playersAlreadyPresent].transform.localEulerAngles;
        networkPlayerObject.transform.SetParent(spawnPosArray[playersAlreadyPresent].transform, true);
        networkPlayerObject.transform.position = spawnPosArray[playersAlreadyPresent].transform.position;

        playerMechanicsAllPlayersArray[playersAlreadyPresent] = networkPlayerObject.GetComponentInChildren<PlayerMechanics>();
        //Debug.Log(playersAlreadyPresent + "th number player added");
        spawnedCharacters.Add(player, networkPlayerObject); 
    }
    void HandlePlayerCameras(string playerNumber)
    {
        switch (playerNumber)
        {
            case "Player1":
                multiplayerCamerasPlayer1.SetActive(true);
                multiplayerCamerasPlayer2.SetActive(false);
                multiplayerCamerasPlayer3.SetActive(false);
                multiplayerCamerasPlayer4.SetActive(false);
                break;
            case "Player2":
                multiplayerCamerasPlayer1.SetActive(false);
                multiplayerCamerasPlayer2.SetActive(true);
                multiplayerCamerasPlayer3.SetActive(false);
                multiplayerCamerasPlayer4.SetActive(false);
                break;
            case "Player3":
                multiplayerCamerasPlayer1.SetActive(false);
                multiplayerCamerasPlayer2.SetActive(false);
                multiplayerCamerasPlayer3.SetActive(true);
                multiplayerCamerasPlayer4.SetActive(false);
                break;
            case "Player4":
                multiplayerCamerasPlayer1.SetActive(false);
                multiplayerCamerasPlayer2.SetActive(false);
                multiplayerCamerasPlayer3.SetActive(false);
                multiplayerCamerasPlayer4.SetActive(true);
                break;
        }
    }

    //void HandleMultiplayerCameras(int playerSequenceNumber)
    //{
    //    switch (playerSequenceNumber)
    //    {
    //        case 1:
    //            multiplayerCamerasPlayer1.SetActive(true);
    //            multiplayerCamerasPlayer2.SetActive(false);
    //            multiplayerCamerasPlayer3.SetActive(false);
    //            multiplayerCamerasPlayer4.SetActive(false);
    //            break;
    //        case 2:
    //            multiplayerCamerasPlayer1.SetActive(false);
    //            multiplayerCamerasPlayer2.SetActive(true);
    //            multiplayerCamerasPlayer3.SetActive(false);
    //            multiplayerCamerasPlayer4.SetActive(false);
    //            break;
    //        case 3:
    //            multiplayerCamerasPlayer1.SetActive(false);
    //            multiplayerCamerasPlayer2.SetActive(false);
    //            multiplayerCamerasPlayer3.SetActive(true);
    //            multiplayerCamerasPlayer4.SetActive(false);
    //            break;
    //        case 4:
    //            multiplayerCamerasPlayer1.SetActive(false);
    //            multiplayerCamerasPlayer2.SetActive(false);
    //            multiplayerCamerasPlayer3.SetActive(false);
    //            multiplayerCamerasPlayer4.SetActive(true);
    //            break;
    //    }
    //}
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            spawnedCharacters.Remove(player);
        }
    } 
    
    public void OnConnectedToServer(NetworkRunner runner)
    {
        totalNumberOfPlayers = runner.SessionInfo.PlayerCount;
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {

    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {

    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {

    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {

    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {

    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {

    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
        
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    { 
        
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        
    }
}

//if (playerMechanicsAllPlayersArray[0] == null) return;
//playerMechanicsAllPlayersArray[0].MouseControls();
////playerMechanicsAllPlayersArray[0].ShipIdleMovement();

//Debug.Log("00000");

//if (NetworkPlayer.Local != null)
//{
//    playerMech = NetworkPlayer.Local.GetComponentInChildren<PlayerMechanics>();
//    Debug.Log(playerMech.transform.parent.name + " is current player");
//}

//var data = new NetworkInputData();

//if (playerMech != null)
//{
//    data.position = playerMech.transform.gameObject.transform.position;
//    data.rotation = playerMech.transform.gameObject.transform.eulerAngles;
//}

//input.Set(data);