
using UnityEngine;
using System.Collections;
using System; //This allows the IComparable Interface
using System.Collections.Generic;


using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public enum PlayerType{
    VR, PC
}

public class PlayerNetwork : Photon.MonoBehaviour {
    public static PlayerNetwork instance;
    public string PlayerName;
    public LocalPlayerBody localBody;
    public NetworkPlayer localPlayer;
    private GameObject playerObj;
    bool playerSpawned = false;
    public PlayerType pType = PlayerType.VR;
   // public Valve.VR.InteractionSystem.Sample.PlayerMovement_VR vrMovement;
   // public PlayerMovement_PC pcMovement;

    bool initializedInRoom = false;





    void Awake () {
        instance = this.GetComponent<PlayerNetwork>();
        PlayerName = "Josh" + UnityEngine.Random.Range(0, 100);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;


        if(getPlayertype() == PlayerType.VR){
            //introplayer.InitVR();
            localBody.SetIntroBody(0);
            SetMovementType(0);
        }else{
            localBody.SetIntroBody(1);
            SetMovementType(1);
        }

    }
    void SetViews(PhotonView v){
        playerObj.GetComponentInChildren<NetworkPlayer>().SetView(v);
        playerObj.GetComponentInChildren<NetworkPlayer_RPCs>().SetView(v);
    }
    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }
      
    }
   
    public List<string> Scenes = new List<string>(){"MainScene"};
    bool isSceneARoom(string s){
        foreach(string scene in Scenes)
            if(s.Equals(scene))
                return true;
        return false;
    }
	private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (isSceneARoom(scene.name)  && !playerSpawned)
        {
            playerObj = PhotonNetwork.Instantiate("NetPlayer",Vector3.up+( new Vector3(1,0,1) * UnityEngine.Random.Range(-20,20)), Quaternion.identity, 0).gameObject;
            GameObject bd = localBody.transform.parent.gameObject;
            DontDestroyOnLoad(playerObj);
            localBody.transform.SetParent(playerObj.transform);
            localBody.transform.localPosition = Vector3.zero;
            localBody.transform.rotation = playerObj.transform.rotation;
            Destroy(bd);
            playerSpawned = true;

            for(int i = 0 ;i < playerObj.transform.childCount; i++){
                //script isnt active so must find it by name
                if(playerObj.transform.GetChild(i).name == "NetworkScripts"){
                    GameObject netObj = playerObj.transform.GetChild(i).gameObject;
                    netObj.SetActive(true);
                    SetViews(playerObj.GetComponent<PhotonView>());
                }
                if(playerObj.transform.GetChild(i).name == "Main Camera")
                   Destroy( playerObj.transform.GetChild(i).gameObject);
            }

           // pcMovement.SetRigidbody(playerObj.GetComponent<Rigidbody>());
            //vrMovement.SetRigidbody(playerObj.GetComponent<Rigidbody>());

            localPlayer = playerObj.GetComponentInChildren<NetworkPlayer>();
            localPlayer.SetLocalBody(localBody);

            PhotonView view =  playerObj.GetComponent<PhotonView>();

            localBody.setCotnrollersView(view);
            InitHashTable(view);
            InitRPCs(view);
            setInitializedInRoom(true);
          

        }
    }
    GameObject ship ;
    void InitHashTable(PhotonView view){
        Hashtable hash = new Hashtable();
            if(getPlayertype() == PlayerType.VR)
                hash.Add("PlayerType", 0);
            else
                hash.Add("PlayerType", 1);
            view.owner.SetCustomProperties(hash);
    }
    void InitRPCs(PhotonView view){
        view.RPC("RPC_UpdateName", PhotonTargets.AllBuffered,view.viewID, PlayerName);
        if(getPlayertype() == PlayerType.VR)
            view.RPC("RPC_SetPlayerType", PhotonTargets.AllBuffered,view.viewID, 0);
        else
            view.RPC("RPC_SetPlayerType", PhotonTargets.AllBuffered,view.viewID, 1);
    }
    void OnJoinedRoom()
    {
       
    }
    
    

    







     public LocalPlayerBody getLocalBody(){ return this.localBody;}
    
    public PlayerType getPlayertype(){
        return this.pType;
    }
    public void setPlayerTypebyInt(int pt){
        if(pt == 0)
            this.pType = PlayerType.VR;
        else if(pt == 1)
            this.pType = PlayerType.PC;
    }


    public void SetMovementType(int i){
        //if(i==1){
        //   vrMovement.enabled=false;
        //}else if(i==0){
        //    vrMovement.enabled=true;
        //}
    }
    public bool isPlayerVR(){return pType == PlayerType.VR;}
    public bool isPlayerPC(){return pType == PlayerType.PC;}

    public bool getInitializedInRoom(){return this.initializedInRoom; }
    public void setInitializedInRoom(bool b){this.initializedInRoom = b;}
}
