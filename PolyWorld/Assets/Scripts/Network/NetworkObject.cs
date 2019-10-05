using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkObject : MonoBehaviour {
	public Text text;
	public PhotonView view;
	int ViewId=0;
	InteractableObject interactable;
	public NetworkObject_Update NetUpdate;

	

	void Awake(){
		view = this.GetComponent<PhotonView>();
		interactable = this.GetComponent<InteractableObject>();
		//ObjUpdate = this.GetComponent<NetworkObject_Update>();


		if(text==null) return;

		if(view.owner !=null)
			text.text = view.owner.name + "'s";
		else
			text.text = "Scene";
	}
	public void setParentShipView(PhotonView v){
		this.transform.SetParent(v.gameObject.transform);
	}
	void Update(){
		if(view.ownerId != ViewId)
			OnViewChange();


	}
	void OnViewChange(){
		ViewId = view.ownerId;
		if(view.owner==null)
			NetUpdate.OnReturnToScene();

		
			if(text==null) return;
		if(view.owner!=null){
			text.text = view.owner.name + "'s";
		}else{
			text.text = "Scene's";

		}

	}
	void OnTriggerEnter(Collider col){
		// if(col.transform.gameObject.GetComponent<PhotonView>() !=null){
		// 	//if interactable object
		// 	if(interactable && !interactable.IsInteracting()){
		// 		//only change if controller is null
		// 		PhotonView _view = col.transform.gameObject.GetComponent<PhotonView>();
		// 		if(_view.ownerId != view.ownerId){
		// 			view.TransferOwnership(_view.ownerId);
		// 		}
		// 	}
		// }
	}

	// void OnTriggerExit(Collider Col){
	// 	if(col.transform.gameObject.GetComponent<PhotonView>() !=null){
	// 		PhotonView _view = col.transform.gameObject.GetComponent<PhotonView>();
	// 		if(_view.ownerId != view.ownerId){
	// 			view.TransferOwnership(0);
	// 			text.text = "Scene";
	// 		}
	// 	}
	// }
}
