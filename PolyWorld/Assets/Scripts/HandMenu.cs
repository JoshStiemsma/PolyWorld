using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMenu : MonoBehaviour
{
    public GameObject WorldsPanel, ObjectsPanel, EditPanel;
    // Start is called before the first frame update
    void Start()
    {
        WorldsPanel.SetActive(true);
        ObjectsPanel.SetActive(true);
        EditPanel.SetActive(true);
        OpenWorlds();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OpenWorlds() {
        CloseAll();
        //WorldsPanel.SetActive(true);
        WorldsPanel.transform.localScale = Vector3.one;


    }
    public void OpenObjects() {
        CloseAll();
        //ObjectsPanel.SetActive(true);
        ObjectsPanel.transform.localScale = Vector3.one;

    }
    public void OpenEdit()
    {
        CloseAll();
       // EditPanel.SetActive(true);
        EditPanel.transform.localScale = Vector3.one;

    }
    public void CloseAll()
    {
        //WorldsPanel.SetActive(false);
        //ObjectsPanel.SetActive(false);
        //EditPanel.SetActive(false);
        WorldsPanel.transform.localScale = Vector3.zero;
        ObjectsPanel.transform.localScale = Vector3.zero;
        EditPanel.transform.localScale = Vector3.zero;
    }
}
