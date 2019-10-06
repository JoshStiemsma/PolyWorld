using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keyboard : MonoBehaviour
{
    public string currentText;
    public Text text;
    public InputField currentInputField;
    public RectTransform MainPanel;
    // Start is called before the first frame update
    void Start()
    {
        SetMainPanel(false);
    }
     void Update()
    {
        MainPanel.anchoredPosition *= .9f;
    }
    // Update is called once per frame
    void UpdateText()
    {
        if (currentText != null && currentInputField != null)
        {
           // text.text = currentText;
            Debug.Log("update input field");

            currentInputField.text +=currentText;
        }

    }
    bool MainPanelOpen = false;
    public void TogglePanel()
    {
        MainPanelOpen = !MainPanelOpen;
        SetMainPanel(MainPanelOpen);
       
    }
    void SetMainPanel(bool on)
    {
        if (!on)
        {
            MainPanel.anchorMax = new Vector2(0, 1);
            MainPanel.anchorMin = new Vector2(-1, 0);
            MainPanelOpen = false;
        }
        else
        {
            MainPanel.anchorMax = new Vector2(1, 1);
            MainPanel.anchorMin = new Vector2(0, 0);
            MainPanelOpen = true;
        }
    }
    
    public void AddText(string s)
    {
        currentText = s;
      //  currentText += s;
        UpdateText();
    }
    public void AddText(char s)
    {
        currentText = s.ToString();

        //currentText += s;
        UpdateText();
    }
    public void Enter()
    {
        currentText = "\r\n";

       // currentText += "\r\n";
    }
    public void Backspace()
    {
        if(currentInputField != null)
        currentInputField.text = currentInputField.text.Substring(0, currentInputField.text.Length - 1);

        //currentText = currentText.Substring(0, currentText.Length - 1);
    }
    public void SetInputField(InputField field)
    {
        currentInputField = field;
    }
}
