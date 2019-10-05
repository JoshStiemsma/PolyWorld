//======= Copyright (c) Valve Corporation, All rights reserved. ===============

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace Valve.VR.InteractionSystem.Sample
{
    public class PlayerMovement_VR : MonoBehaviour
    {
        //public SteamVR_Action_Boolean plantAction;
       // public SteamVR_Action_In RTouch,LTouch;
       // public SteamVR_Action_In ThumbPress;


       // public Hand Lefthand,Righthand;

        public float SPEED = 10f;

        public Rigidbody RB;
        public bool rotateLeft = false, rotateRight = false;



        private void OnEnable()
        {
            Initialize();
        }
        void Initialize(){
            //LTouch.AddOnChangeListener(OnMovementLeft, Lefthand.handType);
            //RTouch.AddOnChangeListener(OnMovementRight, Righthand.handType);
            
            //ThumbPress.AddOnChangeListener(OnThumbPress_Left, Lefthand.handType);
            //ThumbPress.AddOnChangeListener(OnThumbPress_Right, Righthand.handType);
        }
        public void SetRigidbody(Rigidbody rb){
            RB = rb;
        }
        private void OnDisable()
        {
            //if (RTouch != null)
            //    RTouch.RemoveOnChangeListener(OnMovementLeft, Righthand.handType);
            //if (LTouch != null)
            //    LTouch.RemoveOnChangeListener(OnMovementRight, Lefthand.handType);

            //if (ThumbPress != null){
            //   // ThumbPress.RemoveOnChangeListener(OnThumbPress_Left, Lefthand.handType);
            //    //ThumbPress.RemoveOnChangeListener(OnThumbPress_Right, Righthand.handType);

            //}
        }
        public GameObject head;
        bool boost = false;
       //private void OnThumbPress_Left(SteamVR_Action_In actionIn){
            
       //     Vector2 d = SteamVR_Input._default.inActions.Move.GetAxis(Lefthand.handType);

       //     if(d.x < -.5f && !rotateLeft){
       //         rotateLeft = true;
       //     }else{
       //         rotateLeft = false;
       //     }
       //     if(d.x > .5f && !rotateRight){
       //         rotateRight = true;
       //     }else{
       //         rotateRight = false;
       //     }

       //     if(d.y >.5f && d.x<.3f && d.x > -.3f && !boost){
       //         boost = true;
       //     }else{
       //         boost = false;
       //     }
       //     //OnThumbPress(d);
       // }
       //private void OnThumbPress_Right(SteamVR_Action_In actionIn){
       //     rotateRight = !rotateRight;

       //     Vector2 d = SteamVR_Input._default.inActions.Move.GetAxis(Righthand.handType);
       //     if(d.x < -.5f && !rotateLeft){
       //         rotateLeft = true;
       //     }else{
       //         rotateLeft = false;
       //     }
       //     if(d.x > .5f && !rotateRight){
       //         rotateRight = true;
       //     }else{
       //         rotateRight = false;
       //     }
       //         //OnThumbPress(d);
       // }
        private void OnThumbPress(Vector2 d){
            
        }
        void Update(){
            if(rotateRight){
                RB.transform.RotateAround(this.transform.position,Vector3.up, 1f);
            }else if(rotateLeft){
                RB.transform.RotateAround(this.transform.position,Vector3.up, -1f);
            }
        }
        //private void OnMovementLeft(SteamVR_Action_In actionIn)
        //{  
        //// Debug.Log("Move on left controller");
        //    Vector2 d = SteamVR_Input._default.inActions.Move.GetAxis(Lefthand.handType);
        //    OnMovement(d,Lefthand);


        //}
        //private void OnMovementRight(SteamVR_Action_In actionIn)
        //{   Debug.Log("Move on right cont");

        //    Vector2 d = SteamVR_Input._default.inActions.Move.GetAxis(Righthand.handType);
        //    OnMovement(d,Righthand);
        //}
        private void OnMovement(Vector2 d)
        {   Debug.Log("Move final");
            if(rotateLeft || rotateRight)return;
            if(d.magnitude < .2f)
            return;

            Vector3 forward = this.gameObject.transform.forward;
            Vector3 side = this.transform.right * d.x;
            Vector3 force = (forward * d.y) + side;

            float angle = Mathf.Atan2(d.y, d.x) * Mathf.Rad2Deg;
            forward = Quaternion.AngleAxis(angle - 90f, -this.transform.up) * forward;

            forward.y = 0;
            if(boost)
                 RB.AddForce (forward * SPEED*2f);
            else
                RB.AddForce (forward * SPEED);
        }


        
    }
}