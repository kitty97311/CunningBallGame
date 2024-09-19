using Fusion;
using UnityEngine;
public class NetworkPlayer : NetworkBehaviour
{
    private NetworkCharacterControllerPrototype _cc;
    public static NetworkPlayer Local { get; set; }

    [SerializeField] GameObject multiplayerCamerasPlayer1;
    [SerializeField] GameObject multiplayerCamerasPlayer2;
    [SerializeField] GameObject multiplayerCamerasPlayer3;
    [SerializeField] GameObject multiplayerCamerasPlayer4;

    GameObject multiCamParent;
    PlayerInputHandler playerInputHandler;
    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterControllerPrototype>();
    }
    private void Start()
    {
        playerInputHandler = GetComponent<PlayerInputHandler>();
    }
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            Debug.Log("Start Get Input Network Player");
            transform.position = data.position;
            transform.eulerAngles = data.rotation;
        }
    }
    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Local = this;
            Debug.Log("Local Player");
        }
        else
        {
            //Object.gameObject.GetComponentInChildren<Camera>().enabled = false;
            Debug.Log("Remote PLayer");
        }
    }
    private void Update()
    {
        Debug.Log("My Pos " + transform.position);
    }
    //public override void Spawned()
    //{
    //    multiCamParent = transform.parent.parent.GetChild(5).gameObject;
    //    HandleMultiplayerCameras2(transform.parent.GetSiblingIndex());
    //}
    void HandleMultiplayerCameras2(int playerSequenceNumber)
    {
        switch (playerSequenceNumber)
        {
            case 1:
                multiplayerCamerasPlayer1.SetActive(true);
                multiplayerCamerasPlayer2.SetActive(false);
                multiplayerCamerasPlayer3.SetActive(false);
                multiplayerCamerasPlayer4.SetActive(false);
                break;
            case 2:
                multiplayerCamerasPlayer1.SetActive(false);
                multiplayerCamerasPlayer2.SetActive(true);
                multiplayerCamerasPlayer3.SetActive(false);
                multiplayerCamerasPlayer4.SetActive(false);
                break;
            case 3:
                multiplayerCamerasPlayer1.SetActive(false);
                multiplayerCamerasPlayer2.SetActive(false);
                multiplayerCamerasPlayer3.SetActive(true);
                multiplayerCamerasPlayer4.SetActive(false);
                break;
            case 4:
                multiplayerCamerasPlayer1.SetActive(false);
                multiplayerCamerasPlayer2.SetActive(false);
                multiplayerCamerasPlayer3.SetActive(false);
                multiplayerCamerasPlayer4.SetActive(true);
                break;
        }
    }
}
