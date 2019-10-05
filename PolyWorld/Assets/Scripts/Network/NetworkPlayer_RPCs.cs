using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkPlayer_RPCs : Photon.MonoBehaviour {

	public NetworkPlayer player;
	public PhotonView photonview;
	public void SetView(PhotonView v){
        this.photonview=v;
    }

    [PunRPC]
    public void RPC_UpdateName(int view, string name)
    {
        if (view != photonview.viewID)
            return;
        player.SetPlayerName(name);
    }
 	[PunRPC]
    public void RPC_SetPlayerType(int view, int type)
    {
        if (view != photonview.viewID)
            return;
        player.setPlayerTypebyInt(type);
    }
    public void OnClick_SendMsg()
    {
        string msg = player.canvas.GetMsg();
        photonview.RPC("RPC_UpdateMSG", PhotonTargets.AllViaServer, photonview.viewID, msg);
    }

    [PunRPC]
    public void RPC_UpdateMSG(int view, string MSG)
    {
        if (view == photonview.viewID)
        {
            player.getChat().AddMsg(player.playerName + ":  " + MSG);
        }
    }
    public void SpawnBullet(){
     //   photonview.RPC("RPC_FireBullet", PhotonTargets.AllViaServer, photonview.viewID, this.transform.position + this.transform.forward, this.transform.rotation);

    }
    //[PunRPC]
    //public void RPC_FireBullet(int view, Vector3 pos, Quaternion rot)
    //{
    //    if (view == photonview.viewID)
    //    {
    //        GameObject bullet = Instantiate(player.bulletPrefab, pos, rot);
    //        bullet.GetComponent<Bullet>().Initiate(player);
    //    }
    //}
    
    [PunRPC]
    public void RPC_HitByBullet(int view)
    {
        if (view == photonview.viewID)
        {
           
        	PhotonPlayer p = photonview.owner;

			Hashtable hash = new Hashtable();
			hash.Add("Kills", 0);
			p.SetCustomProperties(hash);
			//Debug.Log("gain kill  : "+ photonview.viewID + " " + playerName+":"+viewID);
			this.transform.position = new Vector3(1,0,1) *Random.Range(-20,20) + new Vector3(0,2,0);
        }
    }

    [PunRPC]
    public void RPC_EarnKill(int view)
    {
    	if (view == photonview.viewID){
    		PhotonPlayer p = photonview.owner;

        	int score = (int)p.customProperties["Kills"];
			score++;
			Hashtable hash = new Hashtable();
			hash.Add("Kills", score);
			p.SetCustomProperties(hash);

        }
    }
}
