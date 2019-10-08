using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyToolkit;
using System.Linq;
public class PolyObject : MonoBehaviour
{
    public PolyAsset Asset { set { asset = value; } get { return asset; } }

    PolyAsset asset;
    GameObject Obj;

    List<MeshRenderer> renderers = new List<MeshRenderer>();
    List<Material> materials = new List<Material>();
   public bool doHighlight = false, doUnhighlight = false;
    List<Collider> colliders = new List<Collider>();

    private void Update()
    {
        if (doHighlight)
        {
            Highlight();
            doHighlight = false;
        }else if (doUnhighlight)
        {
            Unhighlight();
            doUnhighlight = false;
        }
    }
    public void Initialize(PolyAsset _asset)
    {
        asset = _asset;
        Obj = this.transform.GetChild(0).gameObject;
        GetComponentInChildren<InteractableObject>().pObject = this;
        renderers = GetComponentsInChildren<MeshRenderer>().ToList();
        colliders = GetComponentsInChildren<Collider>().ToList();
        foreach (MeshRenderer r in renderers)
            materials.Add(r.sharedMaterial);
    }
    public void Highlight()
    {
        foreach (MeshRenderer r in renderers)
            r.material = ObjectManager.instance.HighlightMaterial;
    }
    public void Unhighlight()
    {
        for(int i = 0; i < renderers.Count; i++)
        {
            renderers[i].material = materials[i];
        }
       // foreach (MeshRenderer r in renderers)
        //    r.material = materials[renderers.IndexOf(r)];
    }
    public void SetColliders(bool value)
    {
        foreach (Collider r in colliders)
            r.enabled = value;
    }
}
