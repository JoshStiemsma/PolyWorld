using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
//using Valve.VR.InteractionSystem;


public class NetworkPlayer : Photon.MonoBehaviour {
    public PhotonView photonview;
    public void SetView(PhotonView v){this.photonview=v;}



    public PlayerBody body;
    //public LocalPlayerBody LocalBody;
    public int viewID;
    public PlayerNetwork Network;
    private NetworkPlayer instance;
    public string playerName = "";

    public PlayerCanvas canvas;
    public MsgInputCanvas chat;
    public MsgInputCanvas getChat(){return this.chat;}

    public GameObject bulletPrefab;
    public int score = 0;

    public bool isLocal = false;


   //////////////////////////////////////////////// Movement type data
    //public PlayerMovement_PC pcMovement;
    // public Valve.VR.InteractionSystem.Sample.PlayerMovement_VR vrMovement;
    // public void SetMovementType(int i){
    //     if(i==1){
    //        vrMovement.enabled=false;
    //     }else if(i==0){
    //         vrMovement.enabled=true;
    //     }
    // }

    public void SetLocalBody(LocalPlayerBody lb){
        body.SetLocalBody(lb);
    }
    // public LocalPlayerBody GetLocalBody(){
      
    // }


   //////////////////////////////////////////////// Player type data
    private PlayerType pType = PlayerType.VR;
    public PlayerType getPlayertype(){return this.pType; }
    public void setPlayerTypebyInt(int pt){
        if(pt == 0)
            this.pType = PlayerType.VR;
        else if(pt == 1)
            this.pType = PlayerType.PC;

        //SetMovementType(pt);
    }
    public void setPlayerType(PlayerType pt){this.pType = pt; }

    // public LocalPlayerBody getLocalBody(){
    //     return this.LocalBody;
    // }
    // public void InitVR(){
    //     setPlayerType(PlayerType.VR);
    // }
    // public void InitPC(){
    //     setPlayerType(PlayerType.PC);
    // }
   //////////////////////////////////////////////// Initialization data
    bool initiated = false;
    public bool getInitiated(){return this.initiated;}
    public void setInitiated(bool b){ this.initiated = b;}


    // Use this for initialization
    void Awake () {
    	instance = this.GetComponent<NetworkPlayer>();
        photonview = this.transform.parent.GetComponent<PhotonView>();
       
        Network = PlayerNetwork.instance;
        viewID = photonview.viewID;
        isLocal = photonview.isMine;

        if (photonview.isMine){
            Network.localPlayer = instance;
        }
        if(chat == null){
            chat = MsgInputCanvas.instance;
        }


         if(canvas==null)
            canvas = this.transform.parent.GetComponentInChildren<PlayerCanvas>();
       //SetMovementType(1);
    }
    
  

    void Update(){
        if(Network.getInitializedInRoom() && !initiated){
            PhotonPlayer p = photonview.owner;
            int pt = (int)p.customProperties["PlayerType"];
            setPlayerTypebyInt(pt);
            setInitiated(true);
        }
    }
    public bool getIsLocal(){return this.isLocal;}
    public void SetPlayerName(string n)
    {
        playerName = n;
        canvas.SetName(n);
    }
    public void HitByBullet(NetworkPlayer bulletOwner){
        photonview.RPC("RPC_HitByBullet", PhotonTargets.All, photonview.viewID);
        bulletOwner.photonview.RPC("RPC_EarnKill", PhotonTargets.All, bulletOwner.viewID);
    }
}
