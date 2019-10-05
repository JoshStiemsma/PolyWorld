//======= Copyright (c) Valve Corporation, All rights reserved. ===============

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace Valve.VR.InteractionSystem.Sample
{
    public class PlayerMovement : MonoBehaviour
    {
        //public SteamVR_Action_Boolean plantAction;
        //public SteamVR_Action_In thumbIn;


        //public Hand hand;

        //public GameObject prefabToPlant;
        public float SPEED = 10f;

        
        
        private void OnEnable()
        {
          //  if (hand == null)
              //  hand = this.GetComponent<Hand>();

            //if (thumbIn == null)
            //{
            //    Debug.LogError("No movement action assigned");
            //    return;
            //}

            //thumbIn.AddOnChangeListener(OnMovement, hand.handType);
        }

        private void OnDisable()
        {
         //   if (thumbIn != null)
               // thumbIn.RemoveOnChangeListener(OnMovement, hand.handType);
        }
        public GameObject head;
   //     private void OnMovement(SteamVR_Action_In actionIn)
   //     {
   //         Vector2 d = SteamVR_Input._default.inActions.Move.GetAxis(hand.handType);
     


			//Vector3 forward = head.transform.forward;
			//Vector3 side = head.transform.right * d.x;
			//Vector3 force = (forward * d.y) + side;

		
			//float angle = Mathf.Atan2(d.y, d.x) * Mathf.Rad2Deg;
			//forward = Quaternion.AngleAxis(angle - 90f, Vector3.down) * forward;

			//forward.y = 0;
		
			//this.GetComponentInParent<Rigidbody>().AddForce (forward * SPEED);
			
	

   //     }


        
    }
}