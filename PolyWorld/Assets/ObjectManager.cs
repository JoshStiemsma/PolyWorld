using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyToolkit;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager instance;
    public GameObject LocalPlayer;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpawnObject(PolyAsset asset)
    {
        
    PolyApi.Import(asset, PolyImportOptions.Default(), ImportCallback);

    }
    void ImportCallback(PolyAsset asset, PolyStatusOr<PolyImportResult> result)
    {
        if (!result.Ok)
        {
            return;
        }
        result.Value.gameObject.transform.position = LocalPlayer.transform.position + LocalPlayer.transform.forward*4f;
        foreach(MeshFilter filter in result.Value.gameObject.GetComponentsInChildren<MeshFilter>())
        {
            MeshCollider coll = filter.gameObject.AddComponent<MeshCollider>();
            coll.sharedMesh = filter.mesh;
            coll.convex=true;
        }
        Rigidbody rb =result.Value.gameObject.AddComponent<Rigidbody>();
        result.Value.gameObject.AddComponent<InteractableObject>();
    }
}
