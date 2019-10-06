using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoom : MonoBehaviour {
    public static CreateRoom instance;
    [SerializeField]
    private Text _roomName;
    public InputField RoomNameInput;
    public GameObject connectPanel;
    bool connectPanelOn = true;
    public DebugPanel debugPanel;
    private Text RoomName
    {
        get { return _roomName; }
    }
    public string PlayerPrefab = "PlayerPrefab";
  
    void Awake(){
        instance = this.GetComponent<CreateRoom>();
        
            
    }
    private void Start()
    {
        //OnConnectedToMaster();
        connectPanel.SetActive(true);
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            OnClick_Connect();
        }

        if (PhotonNetwork.connected && connectPanelOn)
        {

            debugPanel.Log("Connected");

            connectPanelOn = false;
            connectPanel.SetActive(connectPanelOn);
        }else if (!PhotonNetwork.connected && !connectPanelOn)
        {

            debugPanel.Log("Disonnected");

            connectPanelOn = true;
            connectPanel.SetActive(true);
        }
        debugPanel.SetField(0,PhotonNetwork.connectionState.ToString());
      //  Debug.Log(PhotonNetwork.connected +"  " + connectPanelOn);

    }
    public void OnClick_Connect()
    {
        debugPanel.Log(" Attempt Connect...");
        PhotonNetwork.ConnectUsingSettings("0.0.0");
       PlayerNetwork.instance.PlayerName = "Josh " + Random.Range(0, 100);

        debugPanel.SetField(1, "Lobby");

        TypedLobby defaultLobby = new TypedLobby("Lobby", LobbyType.Default);

        PhotonNetwork.JoinLobby(defaultLobby);
       

      
    }
 
    void OnJoinedLobby()
    {
        debugPanel.Log("Joined lobby");

       // Debug.Log("Joined lobby");
        //RoomOptions roomOptions = new RoomOptions() { isVisible = true, maxPlayers = 64 };
        //if (PhotonNetwork.JoinOrCreateRoom("TestRoom", roomOptions, TypedLobby.Default))
        //{
        //    Debug.Log("Room creation sent");
        //}
        //else
        //{
        //    Debug.Log("Room creation failed to send");
        //}
    }


    private void OnPhotonCreatRoomFailed(object[] codeAndMsg)
    {
        debugPanel.Log("creat room failed with error: " + codeAndMsg[1]);

    }
    void OnCreatedRoom()
    {
        debugPanel.Log("Room create succesfull");
        //PhotonNetwork.LoadLevel(1);

    }
    void OnJoinedRoom()
    {
      // PhotonNetwork.LoadLevel(1);
      


    }
}
