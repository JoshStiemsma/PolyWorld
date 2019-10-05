using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using PolyToolkit;

public class PolySpawner : MonoBehaviour
{


    public static PolySpawner instance;



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
        req.pageSize = 20;
        // Send the request.
        PolyApi.ListAssets(req, MyCallback);
    }
   
    public List<PolyAsset> foundAssets = new List<PolyAsset>();
    void MyCallback(PolyStatusOr<PolyListAssetsResult> result) { 
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


        SearchPanel.instance.LoadAssetsToList(foundAssets);

    }

  
}
