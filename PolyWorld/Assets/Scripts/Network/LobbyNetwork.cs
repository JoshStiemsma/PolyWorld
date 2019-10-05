using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyNetwork : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("Connecting to server...");
        //PhotonNetwork.ConnectUsingSettings("0.0.0");
        OnConnectedToMaster();

    }
	void OnConnectedToMaster()
    {
        Debug.Log("Connecting to Master...");

        PhotonNetwork.playerName = PlayerNetwork.instance.PlayerName;

        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }
    public void OnClickJoinRoom(string roomName)
    {

    }
    void OnJoinedLobby()
    {
      
        Debug.Log("Joined Lobby");
    }
    public void OnClickStartSync()
    {
        Debug.Log("Start join ");
    }
}
