using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
public enum Charility { Left,Right}
public class ControllerInput : MonoBehaviour
{
    Valve.VR.InteractionSystem.ControllerInteraction CI;
    public Charility charility;



     bool triggerPress = false;
     bool trigger = false;
     bool previoseTrigger = false;
     bool triggerRelease = false;

    public bool TriggerPress { get { return triggerPress; } set { triggerPress = value; } }
    public bool TriggerRelease { get{ return triggerRelease; }  set{ triggerRelease = value; } }
    public bool Trigger { get { return trigger; } set { trigger = value; } }

    // public variable that can be set to LTouch or RTouch in the Unity Inspector
    public OVRInput.Controller controller;

    private void Update()
    {//pecified by the controller variable.
     
    }
    // Start is called before the first frame update
    void Start()
    {
        CI = GetComponent<Valve.VR.InteractionSystem.ControllerInteraction>();


    }
    private void FixedUpdate()
    {
        if (charility == Charility.Left)
        {
            //Debug.Log("Left: " + Input.GetAxis("Oculus_CrossPlatform_PrimaryIndexTrigger"));

            trigger = Input.GetAxis("Oculus_CrossPlatform_PrimaryIndexTrigger") > .9f;
        }
        else
        {
           // Debug.Log("Right:" + Input.GetAxis("Oculus_CrossPlatform_SecondaryIndexTrigger"));

            trigger = Input.GetAxis("Oculus_CrossPlatform_SecondaryIndexTrigger") > .9f;
        }
        triggerPress = (trigger && !previoseTrigger);
        TriggerRelease = (!trigger && previoseTrigger);


        if (TriggerPress) Debug.Log("Trigger presss");

        previoseTrigger = trigger;

       // Debug.Log(OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger));
    }
    private void LateUpdate()
    {
        
    }
    //public void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    //{
    //    Debug.Log("Trigger is up");
    //          CI.OnTriggerUp();
    //}
    //public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    //{
    //    Debug.Log("Trigger is down");
    //    CI.OnTriggerDown();
    //}



}
