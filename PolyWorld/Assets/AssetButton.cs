using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PolyToolkit;
using UnityEngine.Networking;
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

       
        StartCoroutine(GetTexture(asset.thumbnail.url));

        if (asset.thumbnailTexture != null)
        {
            Debug.Log("Adding thumbnail");

            thumbTex = asset.thumbnailTexture;
            thumbnail.sprite = Sprite.Create(thumbTex, new Rect(0.0f, 0.0f, thumbTex.width, thumbTex.height), new Vector2(0.5f, 0.5f), 100.0f);
        }
    }
    public void OnClick()
    {


        ObjectManager.instance.SpawnObject(asset);

    }



    IEnumerator GetTexture(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture thumbTex = ((DownloadHandlerTexture)www.downloadHandler).texture;
            thumbnail.sprite = Sprite.Create((Texture2D)thumbTex, new Rect(0.0f, 0.0f, thumbTex.width, thumbTex.height), new Vector2(0.5f, 0.5f), 100.0f);

        }
    }
}
