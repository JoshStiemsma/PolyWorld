using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PolyToolkit;

public class SearchPanel : MonoBehaviour
{
    public static SearchPanel instance;
    public RectTransform ContentPanel;
    public GameObject ButtonPrefab;
    List<GameObject> Buttons = new List<GameObject>();
    public InputField searchInput;
    PolySpawner spawner;
    public ItemPanel itemPanel;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        spawner = PolySpawner.instance;


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SearchAssetsButton()
    {
        if(searchInput!=null)
        PopulateList(searchInput.text);
    }

    public void PopulateList(string search)
    {
        PolyListAssetsRequest req = new PolyListAssetsRequest();
        // Search by keyword:
        req.keywords = search;
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
        PolyApi.ListAssets(req, MyCallback);
    }

    public List<PolyAsset> foundAssets = new List<PolyAsset>();
    void MyCallback(PolyStatusOr<PolyListAssetsResult> result)
    {
        if (!result.Ok)
        {
            // Handle error.
            return;
        }
        foundAssets.Clear();
        foreach (PolyAsset asset in result.Value.assets)
        {
            foundAssets.Add(asset);
        }


        LoadAssetsToList(foundAssets);

    }

    public void LoadAssetsToList(List<PolyAsset> assets)
    {

        foreach(GameObject obj in Buttons)
        {
            DestroyImmediate(obj);
        }
        foreach (PolyAsset asset in assets)
        {
            GameObject btn = Instantiate(ButtonPrefab, ContentPanel.position, ContentPanel.rotation, ContentPanel);
            btn.GetComponent<AssetButton>().Initiate(itemPanel,asset);
            Buttons.Add(btn);

        }
    }
}
