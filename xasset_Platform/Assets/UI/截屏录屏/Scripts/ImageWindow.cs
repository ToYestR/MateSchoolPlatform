using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageWindow : MonoBehaviour
{
    public Button closeBtn;//关闭按钮
    public Button preBtn;// 显示上一图片
    public Button nextBtn;// 显示下一图片
    public int index    // 当前显示序号
    {
        get; set;
    }
    public Transform target
    {
        get; set;
    }
    private RawImage mainImage;// 显示主图
    private void Awake()
    {
        mainImage = GetComponent<RawImage>();
        closeBtn.onClick.AddListener(Close);
        preBtn.onClick.AddListener(Previous);
        nextBtn.onClick.AddListener(Next);
    }
    private void Close()
    {
        Destroy(gameObject);
    }
    private void Previous()
    {
        Show(1);
    }
    private void Next()
    {
        Show(-1);
    }
    public void Show(int step)
    {
        int count = target.childCount;
        index += step;
        if (index == -1)
        {
            index = count - 1;
        }
        else if (index == count)
        {
            index = 0;
        }
        mainImage.texture = target.GetChild(index).GetComponent<RawImage>().texture;
    }
}
