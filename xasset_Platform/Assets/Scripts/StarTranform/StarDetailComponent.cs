using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using libx;

//挂载在子物体上的，用于接收父物体分发的数据，然后显示在UI上，同时还有下载图片的功能
[System.Serializable]
public class StarDetailComponent : MonoBehaviour
{
    public string mainScene;
    public string uploadDesc;
    public string logotypeImage;
    /// <summary>
    /// 资源存放目录
    /// </summary>
    public string ossParentPath;
    /// <summary>
    /// 资源id名
    /// </summary>
    public string id;

    public GameObject UITitle;
    public GameObject UIDetail;
    public Image UIIcon;
    public Updater_Loader loader;
    public GameObject starDetailCanvas;

    public bool isActived = false;
    private void Start()
    {
        TitleText();
    }

    public void TitleText()
    {
        Text uiTextComponent = transform.GetChild(0).GetChild(0).GetComponent<Text>();
        uiTextComponent.text = mainScene;
        transform.name=mainScene;
    }
    public void OnMouseDown()
    {
        if (isActived)
        {
            UITest();
        }
    }
    public void UITest()
    {
        UITitle.GetComponent<Text>().text = mainScene;
        UIDetail.GetComponent<Text>().text = uploadDesc;
        StartCoroutine(DownloadAndSetImage(logotypeImage, UIIcon));
        loader.SetItem(ossParentPath, id, mainScene);
        starDetailCanvas.SetActive(true);
    }
    private IEnumerator DownloadAndSetImage(string imageUrl, Image targetImage)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Download error: " + request.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            targetImage.sprite = sprite;
        }
    }
}
