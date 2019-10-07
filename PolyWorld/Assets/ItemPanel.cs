using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyToolkit;
using UnityEngine.Networking;
using UnityEngine.UI;
interface IPanelController
{
    void OpenPanel();
    void ClosePanel();
}
public class ItemPanel : MonoBehaviour, IPanelController
{
    public Image image;
    PolyAsset asset;
    public Text NameText,AuthorText,CreationDate,LicenseText;
    RectTransform rTrans;
    bool Open = false;
    // Start is called before the first frame update
    void Start()
    {
        rTrans = this.GetComponent<RectTransform>();

        ClosePanel();
    }

    // Update is called once per frame
    void Update()
    {
        rTrans.anchoredPosition *= .9f;

        if (asset != null && !Open) SetMainPanel(true);
        else if (asset == null && Open) SetMainPanel(false);

    }
    void SetMainPanel(bool on)
    {
        if (!on)
        {
              rTrans.anchorMax = new Vector2(1, 1);
             rTrans.anchorMin = new Vector2(1, 0);
            this.transform.localScale = Vector3.zero;
            Open = false;
        }
        else
        {
            this.transform.localScale = Vector3.one;

              rTrans.anchorMax = new Vector2(1.5f, 1);
             rTrans.anchorMin = new Vector2(1, 0);
            Open = true;
        }
    }
    public void ClosePanel()
    {
        asset = null;
        SetMainPanel(false);
    }
    public void OpenPanel()
    {
        SetMainPanel(true);
    }
    public void RemoveAsse()
    {
        asset = null;
        SetMainPanel(false);
    }
    public void SelectItem(PolyAsset _asset)
    {
        asset = _asset;
        StartCoroutine(GetTexture(asset.thumbnail.url));

        NameText.text= asset.displayName;
        AuthorText.text = asset.authorName;
        CreationDate.text = asset.createTime.ToString();
        LicenseText.text = asset.license.ToString();

    }

    public void SpawnItem()
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
            image.sprite = Sprite.Create((Texture2D)thumbTex, new Rect(0.0f, 0.0f, thumbTex.width, thumbTex.height), new Vector2(0.5f, 0.5f), 100.0f);

        }
    }
}
