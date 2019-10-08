using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using PolyToolkit;

public class ObjectInformation
{
    public ObjectInformation(PolyObject pObject)
    {
        Name = pObject.Asset.displayName;
        Author = pObject.Asset.authorName;
        Position = pObject.transform.position;

        Scale = pObject.transform.localScale;
        Rotation = pObject.transform.rotation;


    }
    public string Name;

    public string Author;
    public Vector3 Position;
    public Vector3 Scale;

    public Quaternion Rotation;

}
public class EditObjectPanel : MonoBehaviour
{
    public static EditObjectPanel instance;
    PolyObject currentPolyObject;

    public Text ObjectName,objectAuthor, positionText,rotationText,scaleText;
    public HandMenu menu;
    
    void Awake()
    {
        instance = this;
        menu = GetComponentInParent<HandMenu>();
    }
    public void ViewPolyObject(PolyObject pObject)
    {
        menu.OpenEdit();
        currentPolyObject = pObject;
        FillInfo(new ObjectInformation(pObject));

    }
   void FillInfo(ObjectInformation objInfo)
    {
        ObjectName.text = objInfo.Name;

        objectAuthor.text = objInfo.Author;

        positionText.text = objInfo.Position.ToString() ;
        scaleText.text = objInfo.Scale.ToString();
        rotationText.text = objInfo.Rotation.ToString();
    }
    public void CloneButton() { }

    public void FreezeButton() { }

    public void DeleteButton() { }

    public void ExtraButton() { }
}
