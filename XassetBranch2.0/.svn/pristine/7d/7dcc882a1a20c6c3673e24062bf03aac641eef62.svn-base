using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Diagnostics;

public class FolderWindow : MonoBehaviour
{
    #region 属性
    public Button closeBtn; // 关闭按钮
    public Button photographBtn;//拍照切换按钮
    public Button albumBtn;//相册切换按钮
    public Button videoBtn;//视频切换按钮
    public Button editBtn;  // 编辑按钮
    public Button deleteBtn;    // 删除按钮
    public Button cancelBtn;    // 取消按钮
    public Button folderBtn;    // 目录按钮
    public ScrollRect preview;// 预览滑动页面
    public GameObject imagePreviewPrefab;//图片展示单位
    public GameObject videoPreviewPrefab;// 视频展示单位

    public static bool isEdit;    // 是否编辑相册
    #endregion
    private void Start()
    {
        // 按钮事件注册
        closeBtn.onClick.AddListener(CloseWindow);
        photographBtn.onClick.AddListener(OpenPhotograph);
        albumBtn.onClick.AddListener(OpenAlbum);
        videoBtn.onClick.AddListener(OpenVideo);
        folderBtn.onClick.AddListener(OpenFolder);

        editBtn.onClick.AddListener(Edit);
        deleteBtn.onClick.AddListener(Delete);
        cancelBtn.onClick.AddListener(Cancel);

    }
    private void OnEnable()
    {
        Init();
    }
    private void Init()//初始化
    {

        ButtonColorTint(false, photographBtn);
        ButtonColorTint(true, albumBtn);
        ButtonColorTint(false, videoBtn);
        ShowImageScroll();

        preview.verticalNormalizedPosition = 1.0f;
        preview.onValueChanged.AddListener(ValueChange);

        isEdit = false;
        editBtn.gameObject.SetActive(true);
        deleteBtn.gameObject.SetActive(false);
        cancelBtn.gameObject.SetActive(false);
    }
    /// <summary>
    /// 按钮选中切换高亮色/正常色
    /// </summary>
    /// <param name="check">是否选中</param>
    /// <param name="button">要改变的按钮</param>
    private void ButtonColorTint(bool check, Button button)
    {
        if (check)
        {
            button.GetComponentInChildren<Image>().color = new Color(0.2588235f, 0.254902f, 0.2588235f); 
            //button.transform.GetChild(0).GetComponent<Text>().color = Color.white;
        }
        else
        {
            button.GetComponentInChildren<Image>().color = new Color(0.33f, 0.33f, 0.33f);
           // button.transform.GetChild(0).GetComponent<Text>().color = Color.black;
        }
    }
    #region 按钮事件
    private void CloseWindow()// 关闭窗口
    {
        transform.parent.GetComponent<PhotoAlbumWindow>().CloseWindow();
    }
    private void OpenPhotograph()// 打开照相
    {
        gameObject.SetActive(false);
        transform.parent.GetChild(1).gameObject.SetActive(true);
    }
    private void OpenAlbum()    // 打开相册
    {
        ButtonColorTint(true, albumBtn);
        ButtonColorTint(false, videoBtn);
        ShowImageScroll();
    }
    private void OpenVideo()//打开视频
    {
        ButtonColorTint(false, albumBtn);
        ButtonColorTint(true, videoBtn);
        ShowVideoScroll();
    }
    private void Edit()//编辑相册
    {
        isEdit = !isEdit;
        if (isEdit)
        {
            editBtn.gameObject.SetActive(false);
            deleteBtn.gameObject.SetActive(true);
            cancelBtn.gameObject.SetActive(true);
        }
        else
        {
            editBtn.gameObject.SetActive(true);
            deleteBtn.gameObject.SetActive(false);
            cancelBtn.gameObject.SetActive(false);
        }
    }
    private void Cancel()// 取消编辑
    {
        isEdit = false;
        editBtn.gameObject.SetActive(true);
        deleteBtn.gameObject.SetActive(false);
        cancelBtn.gameObject.SetActive(false);
        preview.content.BroadcastMessage("Resume", SendMessageOptions.DontRequireReceiver);
    }
    private void Delete()//删除选中内容
    {
        preview.content.BroadcastMessage("Delete", SendMessageOptions.DontRequireReceiver);
    }
    private void OpenFolder()
    {
        string path = Application.persistentDataPath;
        path = Path.GetFullPath(path);
        Process.Start("explorer.exe", path);
    }
    #endregion
    private void ShowImageScroll()
    {
        preview.content.BroadcastMessage("Remove", SendMessageOptions.DontRequireReceiver);
        string path = Application.persistentDataPath + "/CaptureScreen/";
        if (!Directory.Exists(path))
        {
            return;
        }

        string[] filePaths = Directory.GetFiles(path, "*.png");
        int h = filePaths.Length / 3 + 1;
        h *= 216 + 15;
        preview.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
        Array.ForEach(filePaths, (s) =>
        {
            GameObject game = Instantiate(imagePreviewPrefab, preview.content);
            ImagePreview ip = game.GetComponent<ImagePreview>();
            ip.Show(s);
        });
    }
    private void ShowVideoScroll()
    {
        preview.content.BroadcastMessage("Remove", SendMessageOptions.DontRequireReceiver);
        string path = Application.persistentDataPath + "/Recording/";
        if (!Directory.Exists(path))
        {
            return;
        }

        string[] filePaths = Directory.GetFiles(path, "*.mp4");
        int h = filePaths.Length / 3 + 1;
        h *= 216 + 15;
        preview.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
        Array.ForEach(filePaths, (s) =>
        {
            GameObject game = Instantiate(videoPreviewPrefab, preview.content);
            VideoPreview ip = game.GetComponent<VideoPreview>();
            ip.Show(s);
        });
    }
    private void ValueChange(Vector2 v2)
    {
        float y = preview.content.localPosition.y;
    }
}
