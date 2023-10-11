using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPreview : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public RawImage mainImage;// 主图
    public Toggle toggle; //是否选中
    public GameObject videoWindow;// 视频预览窗口

    private VideoPlayer videoPlayer;//播放器
    private RenderTexture texture;// 视频纹理
    private string url;     // 文件路径
    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }
    public void Show(string path)// 显示
    {
        url = path;
        if (mainImage.texture == null)
        {
            videoPlayer.url = url;
            texture = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
            texture.Create();
            videoPlayer.targetTexture = texture;
            mainImage.texture = texture;
            StartCoroutine(RandomPlay());
        }
    }
    private IEnumerator RandomPlay()
    {
        videoPlayer.Play();
        for (int i = 0; i < Random.Range(2, 3); i++)
        {
            yield return null;
        }
        videoPlayer.Pause();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (FolderWindow.isEdit)// 编辑-- 选中
        {
            toggle.isOn = !toggle.isOn;
            if (toggle.isOn)
            {
                mainImage.color = Color.gray;
            }
            else
            {
                mainImage.color = Color.white;
            }
        }
        else                       // 展示
        {
            GameObject obj = Instantiate(videoWindow, transform.root);
            VideoWindow window = obj.GetComponent<VideoWindow>();
            window.Show(url);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(toggle.isOn)
        {
            return;
        }
        videoPlayer.time = 0.0f;
        videoPlayer.Play();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        videoPlayer.Pause();
    }


    private void Resume()//恢复初始状态
    {
        mainImage.color = Color.white;
        toggle.isOn = false;
    }
    private void Remove()//销毁自身
    {
        Destroy(gameObject);
    }
    private void Delete()// 选中--销毁自身并删除本地文件
    {
        if (toggle.isOn)
        {
            Remove();
            File.Delete(url);
        }
    }
}
