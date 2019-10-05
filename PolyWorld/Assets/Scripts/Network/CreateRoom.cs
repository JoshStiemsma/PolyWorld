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
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            OnClick_Connect();
        }
       // connectPanel.GetComponent<Image>().color = new Color(Random.Range(0,255), Random.Range(0, 255), Random.Range(0, 255)); ;
        if (PhotonNetwork.connected && connectPanelOn)
        {
            Debug.Log("Connect");

            connectPanelOn = false;
            connectPanel.SetActive(connectPanelOn);
        }else if (!PhotonNetwork.connected && !connectPanelOn)
        {
            Debug.Log("disconnect");

            connectPanelOn = true;
            connectPanel.SetActive(true);
        }
        Debug.Log(PhotonNetwork.connected +"  " + connectPanelOn);

    }
    public void OnClick_Connect()
    {
        Debug.Log("Connect");
        PhotonNetwork.ConnectUsingSettings("0.0.0");
       PlayerNetwork.instance.PlayerName = "Josh " + Random.Range(0, 100);


        TypedLobby defaultLobby = new TypedLobby("Lobby", LobbyType.Default);

        PhotonNetwork.JoinLobby(defaultLobby);
       

      
    }
 
    void OnJoinedLobby()
    {

        Debug.Log("Joined lobby");
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
        Debug.Log("creat room failed with error: " + codeAndMsg[1]);

    }
    void OnCreatedRoom()
    {
        Debug.Log("Room create succesfull");
        //PhotonNetwork.LoadLevel(1);

    }
    void OnJoinedRoom()
    {
      // PhotonNetwork.LoadLevel(1);
      


    }
}
