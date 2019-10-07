using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class JoinRoomPanel : MonoBehaviour {
	//private Transform panel;
	private List<GameObject> serverList;
	private GameObject selectedObject;
	public GameObject NextButton,LastButton;
	private Color unselectedColor;
	//private string roomName = "TestRoom";

	//public Button CreateRoomBtn,CreateBattleBtn;
	private int pageNumber = 0;
	private int gamesPerPage = 6;


	public GameObject OpenGamesPanel;

	private NetworkPlayer net;
    public DebugPanel debugPanel;

    string roomName = "";
	public virtual void Awake(){
		net = GameObject.FindGameObjectWithTag ("Player").GetComponent<NetworkPlayer>();
		PhotonNetwork.autoJoinLobby = true;
	}

	public void OnEnable(){

	}
	public void OnDisable(){
		CancelInvoke ();
	}

	public void JoinThisServer(string sName,RoomOptions options){
		JoinRoom (sName,options);
	}



    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.J))
        {
            RoomOptions roomOptions = new RoomOptions() { isVisible = true, maxPlayers = 64 };
            roomName = "TestRoomName";
            JoinRoomWorking(roomName, roomOptions);
        }


    }

    public void OnReceivedRoomListUpdate(){
        //CreateRoomBtn.interactable = true;
        //CreateBattleBtn.interactable = true;
        Debug.Log(" OnReceivedRoomListUpdate");
        if (serverList == null) {
			Debug.Log ("Maker serverList");
			//panel = transform.FindChild ("Panel");
			serverList = new List<GameObject> ();
			unselectedColor = new Color (171 / 255.0f, 164 / 255.0f, 182 / 255.0f, 1);
		}
		InvokeRepeating ("PopulateServerList", 0, 2);
	}

	public void NextPageButton(){
		pageNumber++;
	}
	public void LastPageButton(){
		pageNumber--;
	}
    void OnCreatedRoom()
    {
       // Debug.Log("Room create succesfull");
        //PhotonNetwork.LoadLevel(1);
        debugPanel.Log("Room create succesfull");

    }
    void OnJoinedRoom()
    {


        debugPanel.SetField(2, roomName);

        debugPanel.Log("JoinRoomPanel: Room join succesfull");
    }
    //This works
    public void JoinRoomWorking(string server,RoomOptions options){
		if (PhotonNetwork.connected) {
			//net.inGame = true;
			Debug.Log ("Attemp Joined Room, else create");

			RoomOptions roomOptions = new RoomOptions();
			roomOptions.MaxPlayers = 8;//THIS JUST SAYS 8
			roomOptions.IsVisible = true;
			roomOptions.IsOpen = true;
			//Properties readable in thew lobby
			string[] lobbyprops = new string[1];
			lobbyprops [0] = server;
			//lobbyprops [1] = options.CustomRoomProperties["map"].ToString();

			//Properties readable in the game i think
			ExitGames.Client.Photon.Hashtable roomProps = new ExitGames.Client.Photon.Hashtable();
			roomProps.Add ("server", server);
			//roomProps.Add ("map", options.CustomRoomProperties["map"].ToString());

			//Add them to room options
			roomOptions.CustomRoomPropertiesForLobby = lobbyprops;
			roomOptions.CustomRoomProperties = roomProps;
            roomName = server;
			TypedLobby defaultLobby = new TypedLobby ("Lobby",LobbyType.Default);
			PhotonNetwork.JoinOrCreateRoom(server, roomOptions, defaultLobby);
			//Network.isMessageQueueRunning =false;
			//PhotonNetwork.LoadLevel(options.CustomRoomProperties["map"].ToString());
		}
	}


    public void JoinRoom(string server,RoomOptions options){
		if (PhotonNetwork.connected) {
			//net.inGame = true;
			Debug.Log ("Attemp Joined Room, else create");

			RoomOptions roomOptions = new RoomOptions();
			roomOptions.MaxPlayers = options.MaxPlayers;
			roomOptions.IsVisible = true;
			roomOptions.IsOpen = true;
			//Properties readable in thew lobby
			string[] lobbyprops = new string[0];
            //lobbyprops.Add("server", server);

            //lobbyprops [0] = "mode";
            //lobbyprops [1] = "map";
            //lobbyprops [2] = "laps";

            //Properties readable in the game i think
            ExitGames.Client.Photon.Hashtable roomProps = new ExitGames.Client.Photon.Hashtable();
			//roomProps.Add ("laps", options.CustomRoomProperties["laps"].ToString());
			//roomProps.Add ("map", options.CustomRoomProperties["map"].ToString());
			//roomProps.Add ("mode", options.CustomRoomProperties["mode"].ToString());
			//Add them to room options
			roomOptions.CustomRoomPropertiesForLobby = lobbyprops;
			roomOptions.CustomRoomProperties = roomProps;
            roomName = server;

            //TypedLobby defaultLobby = new TypedLobby (options.CustomRoomProperties["map"].ToString(),LobbyType.Default);
            TypedLobby defaultLobby = new TypedLobby("Lobby", LobbyType.Default);

            PhotonNetwork.JoinOrCreateRoom(server, roomOptions, defaultLobby);
			//Network.isMessageQueueRunning =false;
			//PhotonNetwork.LoadLevel(options.CustomRoomProperties["map"].ToString());
		}
	}



	public void PopulateServerList(){
        Debug.Log("Populate list " );

        if (!PhotonNetwork.insideLobby)
			return;
		
		int i = 0+(pageNumber*gamesPerPage);
		RoomInfo[] hostData = PhotonNetwork.GetRoomList ();
		
		if (hostData.Length > gamesPerPage + (pageNumber * gamesPerPage)) {
			NextButton.GetComponent<Button> ().interactable = true;
		} else {
			NextButton.GetComponent<Button> ().interactable = false;
		}
		if (pageNumber > 0) {
			LastButton.GetComponent<Button> ().interactable = true;
		} else {
			LastButton.GetComponent<Button> ().interactable = false;
		}

		for (int j = 0; j < serverList.Count; j++) {
			Destroy (serverList [j]);
		}
        Debug.Log("Populate list " + hostData.Length);

        serverList.Clear ();
		if (hostData != null) {
			Debug.Log(hostData.Length);
			int firstGameOnList = 0 + (pageNumber * gamesPerPage);
			int lastGameOnList = hostData.Length;
			if(lastGameOnList > firstGameOnList + gamesPerPage)
				lastGameOnList = firstGameOnList + gamesPerPage;

			//int count = 0;
			float min = .9f;
			float max = 1f;
            OpenGamesPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, -OpenGamesPanel.GetComponent<RectTransform>().rect.height);

            for (i = firstGameOnList; i <lastGameOnList; i++) {
				//if (!hostData [i].IsOpen)
				//	continue; 

                ExitGames.Client.Photon.Hashtable customProp = hostData[i].CustomProperties;


                string[] lobbyprops = new string[0];
				//lobbyprops [0] = "mode";
				//lobbyprops [1] = "map";
				//lobbyprops [2] = "laps";

				//Properties readable in the game i think
				ExitGames.Client.Photon.Hashtable roomProps = new ExitGames.Client.Photon.Hashtable();

				//roomProps.Add ("map", customProp["map"].ToString());
				//roomProps.Add ("laps", customProp["laps"].ToString());
				//roomProps.Add ("mode", hostData[i].CustomProperties ["mode"].ToString());

				RoomOptions roomOptions = new RoomOptions(){
					MaxPlayers = hostData[i].MaxPlayers,
					CustomRoomPropertiesForLobby = lobbyprops,
					CustomRoomProperties = roomProps,
				};
                Debug.Log(hostData.Length);

                GameObject obj = (GameObject)Instantiate (Resources.Load ("JoinWorldButtonPrefab"));
				serverList.Add (obj);
               	//obj.GetComponent<JoinWorldButton>().map = customProp["map"].ToString();
                obj.GetComponent<JoinWorldButton> ().Initiate(hostData [i].Name);
                //obj.GetComponent<ServerMenuObject> ().roomOptions = roomOptions;

                //Make button
                OpenGamesPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 128);

                obj.transform.SetParent (OpenGamesPanel.transform, false);
                obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,-128 * i -128);

                obj.transform.Find ("ServerText").GetComponent<Text> ().text = hostData [i].Name;
				//obj.transform.Find ("MapText").GetComponent<Text> ().text = customProp["map"].ToString();
				//obj.transform.Find ("PlayerText").GetComponent<Text> ().text = hostData [i].PlayerCount + "/" + hostData[i].MaxPlayers;
                obj.transform.Find("PlayerText").GetComponent<Text>().text = hostData[i].PlayerCount.ToString() ;


                //            obj.GetComponent<RectTransform> ().anchorMin = new Vector2(.1f,min);
                //obj.GetComponent<RectTransform> ().anchorMax = new Vector2(.9f,max);
                //obj.GetComponent<RectTransform> ().sizeDelta = Vector2.zero;
                //obj.GetComponent<RectTransform> ().localPosition = Vector2.zero;
                //obj.GetComponent<RectTransform> ().offsetMin = new Vector2 (obj.GetComponent<RectTransform> ().offsetMin.x, 0);
                //obj.GetComponent<RectTransform> ().offsetMax = new Vector2 (obj.GetComponent<RectTransform> ().offsetMax.x, 0);
                //min -= .1f;
                //max -= .1f;
            }
        }
        else{

			Debug.Log("Host data Empty");
		}
	}

}

