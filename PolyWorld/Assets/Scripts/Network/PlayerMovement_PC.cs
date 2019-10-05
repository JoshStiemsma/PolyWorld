//======= Copyright (c) Valve Corporation, All rights reserved. ===============

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_PC : MonoBehaviour
{
   public Rigidbody RB;
   public NetworkPlayer NetPlayer;
    //public GameObject prefabToPlant;
    public float SPEED = 10f;
    void Start(){
        RB = this.GetComponentInParent<Rigidbody>();
        //NetPlayer = this.transform.GetComponentInChildren<NetworkPlayer>();
    }
    public void Initialize(){
        RB = this.GetComponentInParent<Rigidbody>();
    }
    public void Initialize(Rigidbody rb){
        RB = rb;
    }


    public void SetRigidbody(Rigidbody rb){
        RB = rb;
     }
    public void Update(){
        Debug.Log("Handle input");

        if(NetPlayer == null) NetPlayer = this.transform.parent.GetComponentInChildren<NetworkPlayer>(); 
       
        // if(NetPlayer == null){
        //     HandleInputs();
        // }else if(!NetPlayer.gameObject.activeSelf || NetPlayer.photonview.isMine){
            HandleInputs();
        //}
    }
    void HandleInputs(){
        Debug.Log("Handle input 2");
        if (Input.GetKey(KeyCode.W))
        {
            RB.AddForce(RB.transform.forward * SPEED, ForceMode.Force);
        }else if (Input.GetKey(KeyCode.S)){
            RB.AddForce(-RB.transform.forward * SPEED, ForceMode.Force);
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            RB.AddTorque(-RB.transform.up * SPEED*10f );
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RB.AddTorque(RB.transform.up * SPEED * 10f );
        }



        // if(Input.GetKey(KeyCode.Space) && Time.time - LastFireTime > FireRate){
        //     LastFireTime=Time.time;
        //     NetPlayer.SpawnBullet();
        // }
    }

    
}
