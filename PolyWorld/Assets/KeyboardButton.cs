using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public enum KeyboardButtonType
{
    Key,Enter,Backspace
}
public class KeyboardButton : MonoBehaviour
{
    Keyboard keyboard;
    public string Letter;
    public TextMeshProUGUI tmpText;
    public KeyboardButtonType type = KeyboardButtonType.Key;
    private void Start()
    {
        keyboard = GetComponentInParent<Keyboard>();
        if( Letter != null && !Letter.Equals(" ") &&  type == KeyboardButtonType.Key)
        tmpText.text = Letter.ToUpper();
    }
    public void OnClick()
    {
        if (type == KeyboardButtonType.Key)
            keyboard.AddText(Letter.ToLower());
        else if (type == KeyboardButtonType.Enter)
            keyboard.Enter();
        else if (type == KeyboardButtonType.Backspace)
            keyboard.Backspace();
    }
}
