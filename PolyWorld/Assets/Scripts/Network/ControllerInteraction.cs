using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

using UnityEngine.EventSystems;


namespace Valve.VR.InteractionSystem
{
public class ControllerInteraction : MonoBehaviour, IPointerClickHandler, IPointerDownHandler
    {
	
	private ControllerInteraction self;

	private InteractableObject HighlightedObj;
	public InteractableObject GrabbedItem;
    public InteractableObject LaserGrabbedItem;

        private InteractableObject closestItem;

	public Vector3 HitLocation,lastContact;
	public GameObject endpoint;
	public GameObject center;
	public PhotonView playerview;
	public PhotonView interactingView;

	//HashSet<InteractableObject> objectsHoveringOver = new HashSet<InteractableObject> ();
	public PhotonView getView(){return this.playerview;}
	public void setView(PhotonView v){ this.playerview = v;}
        public ControllerInput input;
    private void OnEnable()
    {

            self = this.GetComponent<ControllerInteraction>();

            input = GetComponent<ControllerInput>();
            if (playerview == null)
                playerview = this.GetComponentInParent<PhotonView>();
           
        }
        public void OnPointerClick(PointerEventData data)
        {
            Debug.Log("Test click");
        }
       public void OnPointerDown(PointerEventData data)
        {
            Debug.Log("OnPointerDown: " + data.pointerCurrentRaycast.gameObject.name);

        }
        //list of colliders

        //Trigger to add and remove items from nearby items list
        //private void OnTriggerEnter(Collider col)
        //{
        //    InteractableObject collidedItem = col.GetComponent<InteractableObject>();
        //    if (collidedItem != null)
        //    {
        //        if (!collidedItem.currentlyInteracting)
        //        {
        //            found item and if its not being interacted with then we can posses it
        //            PhotonView itemView = col.GetComponent<PhotonView>();
        //            itemView.TransferOwnership(playerview.ownerId);
        //            interactingView = itemView;
        //        }
        //    }
        //}
        //private void OnTriggerExit(Collider col)
        //{
        //    InteractableObject collidedItem = col.GetComponent<InteractableObject>();
        //    if (collidedItem)
        //    {
        //        PhotonView itemView = col.GetComponent<PhotonView>();
        //        if (itemView == interactingView)
        //        {
        //            give possession back to server
        //            itemView.TransferOwnership(0);
        //        }
        //    }
        //}
        ////////////////////////////////////////////////////////////





        public Transform LaserHoldLocation;
        public void OnTriggerDown(){
            if (HighlightedObj != null)
            {
                LaserHoldLocation.position = HighlightedObj.transform.position;
                LaserGrabbedItem = HighlightedObj;
                LaserGrabbedItem.LaserGrab(this);
            }
            GraphicsRaycast();

            if (GrabbedItem==null){
			    GrabNearestItem ();
		    }else{
			    DropItem();
		    }
        }
       public void OnTriggerUp(){
    	    if(GrabbedItem!=null && !GrabbedItem.getStickToHand()){
			    DropItem ();
		    }
            if(LaserGrabbedItem != null)
            {
                LaserGrabbedItem.LaserUngrab();

                LaserGrabbedItem = null;
            }
        }
/////////////////////////////////////////////////////////////////////


    private void GrabNearestItem(){
		Debug.Log ("grab nearest Item ");
		Vector3 pos = center.transform.position;
		Collider[] hits = Physics.OverlapSphere(pos,.01f);
		//Instantiate (debugPrefab, pos, Quaternion.identity);
		int i = 0;
		float minDistance = float.MaxValue;
		float distance;
		//Debug.Log ("items hit with grab " + hits.Length +"  at: " + this.transform.position);
		while (i < hits.Length) {
			//look for interactableItem

			InteractableObject item = hits [i].GetComponent<InteractableObject> ();
			//look in parent for interactableItem
			if (item == null && hits [i].transform.parent!=null ) {
				item = hits [i].transform.parent.gameObject.GetComponent<InteractableObject> ();	
			}
			//look in children for interactableItem

			if (item == null) {
				foreach (InteractableObject _item in hits [i].GetComponentsInChildren<InteractableObject>()) {
					item = _item;
				}
			}

			if (item != null && !item.currentlyInteracting) {
				//Debug.Log ("item: " + item + "  " + hits [i] + "  " + i);
				distance = (item.transform.position - transform.position).sqrMagnitude;
				if (distance < minDistance) {
					minDistance = distance;
					closestItem = item;
				}				
			} 
			
			i++;
		}

		GrabbedItem = closestItem;
		//if we found an interactable item
		if (GrabbedItem) {
			//but its already being interacted with
			//end its current interaction
			//THIS WNOT BE CALLED BECAUSE WE DONT GRAB IF ITS INTERACTING
			//SO NO DOUBLE HAND STUFF YET
			if (GrabbedItem.IsInteracting ()) {
				GrabbedItem.EndInteraction ();
			}
			//Start an interaction with it
			GrabbedItem.BeginInteraction (this);
		}
	}
	private void DropItem ()
	{
		Debug.Log ("Drop item");
		//End the interaction with the II scripts
		GrabbedItem.EndInteraction ();

		//if the item has a sphere collider/ make it non-trigger
		// SphereCollider sphereCollider = GrabbedItem.GetComponent<SphereCollider> ();
		// if (sphereCollider != null && sphereCollider.isTrigger) {
		// 	sphereCollider.isTrigger = false;
		// }
		// BoxCollider b = GrabbedItem.GetComponent<BoxCollider> ();
		// if (b != null && b.isTrigger) {
		// 	b.isTrigger = false;
		// }


		GrabbedItem = null;
	}
	private void CheckReleaseDistance(){
		//Debug.Log (interactingItem.gameObject.name);
		float distance = (center.transform.position - GrabbedItem.interactionPoint.position).magnitude;
		if (distance >= 2f)
			DropItem ();
	}
    void Update(){
            Raycast();
            GraphicsRaycast();
            //if(GrabbedItem != null)CheckReleaseDistance();

            if (input.TriggerPress) OnTriggerDown();

            if (input.TriggerRelease) OnTriggerUp();
    }

        void GraphicsRaycast()
        {
            // Example: get controller's current orientation:
            Quaternion ori = this.transform.rotation;
            Vector3 vector = ori * Vector3.forward;
            

            PointerEventData pointerData = new PointerEventData(EventSystem.current);

            pointerData.position = vector; // use the position from controller as start of raycast instead of mousePosition.
            

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);


            Ray r = new Ray(this.transform.position, this.transform.forward);
            RaycastHit[] hits = Physics.RaycastAll(this.transform.position, this.transform.forward, float.MaxValue);

            foreach (RaycastHit result in hits)
            {
                Debug.Log("graphics Raycast " + result.collider.gameObject.name);

            }



            // Debug.DrawRay(this.transform.position, transform.forward, Color.green, 10f);
            foreach (RaycastResult result in results)
            {
                //WorldUI is my layer name
                Debug.Log("graphics Raycast " + result.gameObject.name);

                if (result.gameObject.layer == LayerMask.NameToLayer("UI"))
                {
                    string dbg = "Root Element: {0} \n GrandChild Element: {1}";
                    Debug.Log(string.Format(dbg, results[results.Count - 1].gameObject.name, results[0].gameObject.name));
                    //Debug.Log("Root Element: "+results[results.Count-1].gameObject.name);
                    //Debug.Log("GrandChild Element: "+results[0].gameObject.name);

                }

            }
        }
   void Raycast(){
		Ray r = new Ray (this.transform.position, this.transform.forward);
		RaycastHit hit;
		bool found = false;
		if (Physics.Raycast (r, out hit, float.MaxValue)) {
               Debug.Log("Raycast " + hit.collider.gameObject.name);
			InteractableObject e = hit.collider.GetComponent<InteractableObject> ();
            if(e== null)
                  e = hit.collider.GetComponentInParent<InteractableObject>();
            lastContact = hit.point;

			if (e != null &&  e != GrabbedItem && e.Active) {
				if (HighlightedObj == null) {
					Highlight (e);
				} else if (HighlightedObj != e) {
					Unhighlight ();
					Highlight (e);
				}
				found = true;
				endpoint.SetActive (true);
			} else if (e == null && e == GrabbedItem && HighlightedObj) {
				Unhighlight ();
			}
			HitLocation = hit.point;
		} else if (HighlightedObj) {
			Unhighlight ();
			//endpoint.SetActive (false);
			HitLocation = this.transform.up*2f ;
		} else {
			HitLocation = this.transform.forward * 2f;
		}

	//if (!found)
		//HitLocation = lastContact;
	}

	void Highlight(InteractableObject o){
		HighlightedObj = o;
		HighlightedObj.Highlight();
	}
	void Unhighlight(){
		HighlightedObj.Unhighlight();
		HighlightedObj = null;
	}
	public InteractableObject getInteractingItem(){
		return GrabbedItem;
	}
	public void setInteractingItem(InteractableObject newII){
		GrabbedItem = newII;
	}

}
}
