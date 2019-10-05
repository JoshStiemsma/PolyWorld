using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames;

[System.Serializable]
public class VRPlayerObjects{
	public GameObject Head, Left, Right;
	public void SetControllerView(PhotonView view){
		Left.GetComponent<Valve.VR.InteractionSystem.ControllerInteraction>().setView(view);
		Right.GetComponent<Valve.VR.InteractionSystem.ControllerInteraction>().setView(view);
	}
}
public class LocalPlayerBody : MonoBehaviour {
	public GameObject Rig, PC_Cam;
	public VRPlayerObjects VRBody;
	public VRPlayerObjects getVRPlayerObjects(){return this.VRBody;}
	public void SetIntroBody(int i){
		if(i == 0){// vr
			Rig.SetActive(true);
			PC_Cam.SetActive(false);

		}else{
			PC_Cam.SetActive(true);
			Rig.SetActive(false);
		}
	}
	public void setCotnrollersView(PhotonView view){
		VRBody.SetControllerView(view);
 
	}
}
