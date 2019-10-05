using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class MsgInputCanvas : MonoBehaviour {
    public static MsgInputCanvas instance;
    public PhotonView localView;

    public InputField input;

    public Text chatBox;
    void Awake(){
        instance = GetComponent<MsgInputCanvas>();
    }
    public void OnClick_SendMsg()
    {
        localView = PlayerNetwork.instance.localPlayer.photonview;
        if (input.text.Length <= 0)
            return;
        Debug.Log("Pun sending on view " + localView.viewID);

        string msg = input.text;
        localView.RPC("RPC_UpdateMSG", PhotonTargets.AllViaServer, localView.viewID, msg);
        input.text= "";
    }
    public void AddMsg(string s){
        if(chatBox.text.Length <500){
            chatBox.text = chatBox.text +'\n' +s; 
        }else{
            string _t = "";
            _t = chatBox.text.Substring(s.Length,chatBox.text.Length - s.Length);
        //    Array.Copy(chatBox.text,s.Length, _t,0,chatBox.text.Length - s.Length);
            chatBox.text = _t +'\n' +s;  

        }
    }
}
