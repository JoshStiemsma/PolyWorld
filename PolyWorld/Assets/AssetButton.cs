using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PolyToolkit;

public class AssetButton : MonoBehaviour
{
    public Text Title;
    public Image thumbnail;
    PolyAsset asset;
    Texture2D thumbTex;
    public void Initiate(PolyAsset _asset)
    {
        asset = _asset;
        Title.text = asset.displayName;
    }
    public void OnClick()
    {
        PolyApi.Import(asset, PolyImportOptions.Default(), ImportCallback);

    }
    void ImportCallback(PolyAsset asset, PolyStatusOr<PolyImportResult> result)
    {
        if (!result.Ok)
        {
            return;
        }
        result.Value.gameObject.transform.position = new Vector3(1, 0, 0) ;
        thumbTex = asset.thumbnailTexture;
        thumbnail.sprite  = Sprite.Create(thumbTex, new Rect(0.0f, 0.0f, thumbTex.width, thumbTex.height), new Vector2(0.5f, 0.5f), 100.0f);
    }
}
