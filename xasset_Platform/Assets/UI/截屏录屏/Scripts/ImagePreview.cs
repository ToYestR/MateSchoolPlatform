using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

public class ImagePreview : MonoBehaviour,IPointerClickHandler
{
    public Toggle toggle;// 是否选中
    public GameObject imageWindow;// 展示大图
    private RawImage mainImg;// 图片
    private string url = ""; //图片路径
    private void Awake()
    {
        mainImg = GetComponent<RawImage>();
    }
    /// <summary>
    /// 显示
    /// </summary>
    /// <param name="path"></param>
    public void Show(string path)
    {
        url = path;
        if (mainImg.texture == null)
        {
            StartCoroutine(Loading());
        }
    }
    /// <summary>
    /// 加载图片
    /// </summary>
    /// <returns></returns>
    private IEnumerator Loading()
    {
        if (File.Exists(url))
        {
            using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
            {
                yield return uwr.SendWebRequest();

                if (uwr.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log("加载本地纹理失败：" + uwr.error);
                }
                else
                {
                    mainImg.texture = DownloadHandlerTexture.GetContent(uwr);
                }
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(FolderWindow.isEdit)// 编辑-- 选中
        {
            toggle.isOn = !toggle.isOn;
            if(toggle.isOn)
            {
                mainImg.color = Color.gray;
            }
            else
            {
                mainImg.color = Color.white;
            }
        }
        else                       // 展示
        {
            GameObject obj = Instantiate(imageWindow, transform.root);
            ImageWindow window = obj.GetComponent<ImageWindow>();
            window.index = transform.GetSiblingIndex();
            window.target = transform.parent;
            window.Show(0);
        }
    }
    private void Resume()//恢复初始状态
    {
        mainImg.color = Color.white;
    }
    private void Remove()//销毁自身
    {
        Destroy(gameObject);
    }
    private void Delete()// 选中--销毁自身并删除本地文件
    {
        if(toggle.isOn)
        {
            Remove();
            File.Delete(url);
        }
    }
}
