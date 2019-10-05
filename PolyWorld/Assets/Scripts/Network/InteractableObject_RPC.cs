using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class InteractableObject_RPC : Photon.MonoBehaviour {
	//public NetworkPlayer player;
	public PhotonView photonview;
	public InteractableObject item;
	public void OnClick_RandomColor()
    {
    	if(PhotonNetwork.connected)
       		photonview.RPC("RPC_RandomColorRPC", PhotonTargets.AllViaServer, photonview.viewID, new Vector3(Rand(255),Rand(255),Rand(255)));
    }

    [PunRPC]
    public void RPC_RandomColorRPC(int view, Vector3 rand)
    {
        if(view == photonview.viewID)
        {
        	item.getMesh().material.color = new Color(rand.x,rand.y,rand.z);
        }
    }


    




    int Rand(int i){
    	return Random.Range(0,i);
    }
}
