using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

using Valve.VR.InteractionSystem;
[System.Serializable]
public class ControllerInteractionData{
    public bool Grabbable = true;
	public bool LockXAxis = false, LockYAxis = false, LockZAxis = false;
	public bool LockRotation = false, ChildObject = false;
	public bool StickToHand=false;

}
public class InteractableObject : MonoBehaviour {
	public ControllerInteraction controller;
	InteractableObject self;
	public bool Active = true;
	public Rigidbody rigidbody;

	//interaction variables
	public Transform interactionPoint;
	public bool EstablishedInteractionPoint = false;
	public Vector3 InteractionOffset;
	public bool currentlyInteracting = false;
	public ControllerInteractionData data;
	public GameObject ColliderParent;
	//Net stuff
	public PhotonView view;

	public bool getStickToHand(){return this.data.StickToHand;}
	//Visual
	public Material HighlightMaterial, InitMaterial,DeactiveMaterial;
	private MeshRenderer mesh;
	public bool highlighted = false;

    public OSVR.UI.TextScript textScript;
	// Use this for initialization
	public virtual void Start () {
		self = this.GetComponent<InteractableObject> ();
		mesh = GetComponent<MeshRenderer> ();
		if(mesh!=null)
		InitMaterial =	mesh.material;
		if(rigidbody==null)
			rigidbody= GetComponent<Rigidbody> ();
		//savedRB = GetComponent<Rigidbody> ();
		if (interactionPoint == null) {
			interactionPoint = new GameObject ().transform;
			interactionPoint.transform.parent = this.gameObject.transform;
		}
		if(view==null)
		view = GetComponent<PhotonView>();
        //InteractionOffset = this.transform.position-interactionPoint.transform.position;
        if(textScript==null)
            textScript = GetComponent<OSVR.UI.TextScript>();


    }
	public void LaserClick()
    {
        if(textScript != null)
            textScript.AddText();
    }
    // Update is called once per frame
    public virtual void Update () {
		if(mesh!=null)UpdateMeshHighlight();
        if (controller != null) MoveToController();
	}
	void UpdateMeshHighlight(){
		if(highlighted)
				mesh.material=HighlightMaterial;
			else
				mesh.material=InitMaterial;
	}
	public virtual void MoveToController(){
		if (data.ChildObject) {
			Vector3 current = transform.parent.InverseTransformPoint (transform.position);
			Vector3 delta = transform.parent.InverseTransformPoint (controller.center.transform.position);
			Vector3 newPos = new Vector3 (delta.x, delta.y, delta.z);

			this.transform.localPosition = newPos;
			//totalMovement += current - newPos;
		}else{
            if (data.Grabbable)
            {
                Vector3 pos = interactionPoint.transform.position;
                Vector3 newPos = controller.center.transform.position;
                if (data.LockXAxis)
                    newPos.x = pos.x;
                if (data.LockYAxis)
                    newPos.y = pos.y;
                if (data.LockZAxis)
                    newPos.z = pos.z;

                this.gameObject.transform.position = newPos;// +InteractionOffset;

                if (!data.LockRotation)
                    this.gameObject.transform.rotation = controller.center.transform.rotation;
                this.gameObject.transform.position += InteractionOffset;

                if (rigidbody != null)
                    rigidbody.angularVelocity = Vector3.zero;
            }
		}
	}


	
	public virtual void BeginInteraction(ControllerInteraction c) {
		controller = c;
		currentlyInteracting = true;
        if (data.Grabbable)
        {
            if (rigidbody != null)
            {
                rigidbody.useGravity = false;
                rigidbody.isKinematic = true;
            }
            // InteractionOffset = controller.gameObject.transform.position - transform.position;
            interactionPoint = this.transform;
        }
    }
	public virtual void EndInteraction( ) {
		if (controller!=null) {
			controller = null;
			currentlyInteracting = false;
			Unhighlight ();
            if (rigidbody != null)
            {
                rigidbody.useGravity = true;
                rigidbody.isKinematic = false;
            }
        }
	}


///////////////////////////////EXTRA STUFFFFFFFFFFF
	public virtual void Highlight()
	{
		highlighted = true;
		if(mesh!=null)
			mesh.material = HighlightMaterial;
	}
	public virtual void Unhighlight(){
		highlighted = false;
		if(mesh!=null)
			mesh.material=InitMaterial;
	}





//////////////Utility/Set/Get//////

	public MeshRenderer getMesh(){return this.mesh;}
	
	public PhotonView getView(){return this.view;}
	public virtual void SetColliders(bool b){
		ColliderParent.SetActive(b);
		rigidbody.isKinematic = false;

	}
	public bool IsInteracting(){
		return currentlyInteracting;
	}
	///////////////////////


	////////////////Debug//////////////
public bool colorDebug = false;

	///////////////////
}

