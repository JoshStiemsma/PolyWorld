using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerCanvas : MonoBehaviour {
    public Text Name;
    //public Text Msg;
    public InputField msgIn;
    public string GetMsg()
    {
        return msgIn.text;
    }
    public void SetName(string n) {
        Name.text = n;
    }
    

}
