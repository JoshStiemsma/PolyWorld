using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyToolkit;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager instance;
    public GameObject LocalPlayer;
    public GameObject ObjectPrefab;

    public Material HighlightMaterial;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            TestSpawn();
        }
    }

    void TestSpawn()
    {
        PolyListAssetsRequest req = new PolyListAssetsRequest();
        // Search by keyword:
        req.keywords = "cat";
        // Only curated assets:
        req.curated = true;
        // Limit complexity to medium.
        req.maxComplexity = PolyMaxComplexityFilter.MEDIUM;
        // Only Blocks objects.
        req.formatFilter = PolyFormatFilter.BLOCKS;
        // Order from best to worst.
        req.orderBy = PolyOrderBy.BEST;
        // Up to 20 results per page.
        req.pageSize = 10;
        // Send the request.
        PolyApi.ListAssets(req, ListCallback);
    }
    void ListCallback(PolyStatusOr<PolyListAssetsResult> result)
    {
        if (!result.Ok)
        {
            // Handle error.
            return;
        }

        PolyApi.Import(result.Value.assets[0], PolyImportOptions.Default(), ImportCallback);


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
        Vector3 position = LocalPlayer.transform.position + LocalPlayer.transform.forward * 4f;
       //PolyObject pObj = Instantiate(ObjectPrefab, position, Quaternion.identity, this.transform).GetComponent<PolyObject>();

        result.Value.gameObject.transform.position = position;

        foreach (MeshFilter filter in result.Value.gameObject.GetComponentsInChildren<MeshFilter>())
        {
            MeshCollider coll = filter.gameObject.AddComponent<MeshCollider>();
            coll.sharedMesh = filter.mesh;
            coll.convex=true;
        }
        Rigidbody rb =result.Value.gameObject.AddComponent<Rigidbody>();
        result.Value.gameObject.AddComponent<InteractableObject>().Active = true;
         PolyObject pObj = result.Value.gameObject.AddComponent<PolyObject>();

        result.Value.gameObject.transform.SetParent(pObj.transform);

        pObj.Initialize(asset);
    }
}
