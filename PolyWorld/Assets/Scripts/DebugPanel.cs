using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugPanel : MonoBehaviour
{
    string log;

    public Text LogText;
    //public Text Field01, Field02, Field03, Field04;
    public List<Text> Fields = new List<Text>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Log(string s)
    {
        LogText.text += "\n" + s;
    }
   
    public void SetField(int i, string s) {
       // Debug.Log("Call Set field " + i + " to " + s);

        if (Fields.Count > i && Fields[i] != null)
        {
            Debug.Log("Set field " + i + " to " + s);
            Fields[i].text = s;
        }
    }
}
