using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keyboard : MonoBehaviour
{
    public string currentText;
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void UpdateText()
    {
        if (text != null)
            text.text = currentText;
    }

    public void AddText(char s)
    {
        currentText += s;
        UpdateText();
    }
    public void Enter()
    {
        currentText += "\r\n";
    }
    public void Backspace()
    {
        currentText = currentText.Substring(0, currentText.Length - 1);
    }
}
