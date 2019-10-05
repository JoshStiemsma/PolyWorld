using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMenu : MonoBehaviour
{
    public GameObject WorldsPanel, ObjectsPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OpenWorlds() {
        WorldsPanel.SetActive(true);
        ObjectsPanel.SetActive(false);

    }
    public void OpenObjects() {
        WorldsPanel.SetActive(false);
        ObjectsPanel.SetActive(true);
    }
}
