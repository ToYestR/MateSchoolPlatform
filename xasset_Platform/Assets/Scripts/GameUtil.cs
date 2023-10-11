using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using RenderHeads.Media.AVProMovieCapture;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.Networking;
using TMPro;

public class GameUtil
{
    #region 截屏
    public static void ScreenCapture(string fileName)//截屏并保存本地
    {
        string path = Application.persistentDataPath + "/CaptureScreen/";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        UnityEngine.ScreenCapture.CaptureScreenshot(path + fileName + ".png");
    }
    public static Texture2D GetScreenTexture()// 截屏视口纹理(纹理大小有限制)
    {
        return UnityEngine.ScreenCapture.CaptureScreenshotAsTexture(2);
    }
    public static void SaveTextureFile(Texture2D texture, string name)// 保存纹理到本地
    {
        string path = Application.persistentDataPath + "/CaptureScreen/";
        byte[] data = texture.EncodeToPNG();
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        File.WriteAllBytes(path + name + ".png", data);
        Debug.Log("保存成功：" + path);
    }
    public static Sprite Texture2Sprite(Texture2D texture)//纹理转精灵
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
    }
    public static string[] getCaptures()// 获取所有截屏路径
    {
        return Directory.GetFiles(Application.persistentDataPath + "/CaptureScreen/", "*.png");
    }
    public static void ClearCaptures()//    清空所有截屏
    {
        string path = Application.persistentDataPath + "/CaptureScreen/";
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
            Debug.Log("成功清空截屏目录");
        }
    }
    #endregion
    #region 视频录制（安装：AVProMovieCapture）
    /// <summary>
    /// 开始录制（带UI）
    /// </summary>
    /// <param name="isRecordSound">是否录制声音（录制声音会消耗大量性能）</param>
    public static void StartRecordingUI(string fileName, bool isRecordSound = false)
    {
        GameObject cameraObj = Camera.main.gameObject;
        CaptureFromScreen capture;
        if (!cameraObj.TryGetComponent<CaptureFromScreen>(out capture))
        {
            capture = cameraObj.AddComponent<CaptureFromScreen>();
        }
        capture._downScale = CaptureBase.DownScale.Half;        // 分辨率
        capture._frameRate = CaptureBase.FrameRate.TwentyFour;  // 帧率
        capture._outputFolderPath = Application.persistentDataPath + @"\Recording\";// 保存路径
        capture._useMediaFoundationH264 = true;//MP4 格式
        capture._autoGenerateFilename = false;//自动生成文件名
        capture._noAudio = !isRecordSound;   // 是否录制声音
        capture._forceFilename = fileName;// 文件名

        capture.enabled = true;
        capture.StartCapture();
    }
    /// <summary>
    /// 结束录制（带UI）
    /// </summary>
    public static void StopRecordingUI()
    {
        try
        {
            CaptureFromScreen capture = Camera.main.GetComponent<CaptureFromScreen>();
            capture.StopCapture();
            capture.enabled = false;
        }
        catch (DirectoryNotFoundException e)
        {
            Debug.LogError("无效目录：" + e);
        }
    }
    /// <summary>
    /// 开始录制（不带UI）
    /// </summary>
    /// <param name="isRecordSound">是否录制声音（录制声音会消耗大量性能）</param>
    public static void StartRecording(bool isRecordSound = false)
    {
        GameObject cameraObj = Camera.main.gameObject;
        CaptureFromCamera capture;
        if (!cameraObj.TryGetComponent<CaptureFromCamera>(out capture))
        {
            capture = cameraObj.AddComponent<CaptureFromCamera>();
        }
        capture._downScale = CaptureBase.DownScale.Half;        // 分辨率
        capture._frameRate = CaptureBase.FrameRate.TwentyFour;  // 帧率
        capture._outputFolderPath = Application.persistentDataPath + @"\Recording\";// 保存路径
        capture._useMediaFoundationH264 = true;//MP4 格式
        //capture._autoGenerateFilename = false;//自动生成文件名
        capture._noAudio = !isRecordSound;   // 是否录制声音
        //capture._forceFilename = fileName;// 文件名

        capture.enabled = true;
        capture.StartCapture();
    }
    /// <summary>
    /// 结束录制（不带UI）
    /// </summary>
    public static void StopRecording()
    {
        CaptureFromCamera capture;
        if (Camera.main.TryGetComponent<CaptureFromCamera>(out capture))
        {
            capture.StopCapture();
            capture.enabled = false;
        }
    }
    /// <summary>
    /// 获取录制文件路径
    /// </summary>
    /// <returns></returns>
    public static string[] GetRecording()
    {
        return Directory.GetFiles(Application.persistentDataPath + "/Recording/", "*.mp4");
    }
    /// <summary>
    /// 清空录制记录
    /// </summary>
    public static void ClearRecording()
    {
        string path = Application.persistentDataPath + "/Recording/";
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
    }
    #endregion
    #region 射线
    public static RaycastHit? ScreenRay(Vector3 pos, Camera camera)
    {
        Ray ray = camera.ScreenPointToRay(pos);

        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        return hit;
    }
    #endregion
    #region 数值映射
    public static float Mapping01(float inMin, float inMax, float value)// inMin-inMax 映射 0-1 
    {
        float num = (value - inMin) / (inMax - inMin);
        return Mathf.Clamp01(num);
    }
    public static Vector2 Mapping01(Vector2 inMin, Vector2 inMax, Vector2 value)
    {
        return new Vector2(Mapping01(inMin.x, inMax.x, value.x), Mapping01(inMin.y, inMax.y, value.y));
    }
    public static float Mapping(float inMin, float inMax, float value)// 0-1 映射 inMin-inMax
    {
        float num = (inMax - inMin) * value + inMin;
        return Mathf.Clamp(num, inMin, inMax);
    }
    public static Vector2 Mapping(Vector2 inMin, Vector2 inMax, Vector2 value)
    {
        return new Vector2(Mapping(inMin.x, inMax.x, value.x), Mapping(inMin.y, inMax.y, value.y));
    }
    #endregion
    #region Texture To Sprite
    public static Sprite Texture2Sprite(Texture texture)
    {
        return Sprite.Create((Texture2D)texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
    }
    #endregion
    #region 时间/数字
    public static string Num2Time(int num)// 数字转时间（105-》00:01:45）
    {
        int hour = num / (60 * 60);
        int minute = (num - hour * 60 * 60) / 60;
        int second = (num - hour * 60 * 60) % 60;
        return hour.ToString("D2") + ":" + minute.ToString("D2") + ":" + second.ToString("D2");
    }
    public static double GetCurrentSecond()// 获取时间戳
    {
        return (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
    }
    #endregion
    #region 系统消息窗口
    [DllImport("user32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern int MessageBox(IntPtr handle, string mssage, string title, int type);
    #endregion
    #region 其他
    public static int getSceneIndexByName(string sceneName)// 根据场景名称获取构建中的序号
    {
        sceneName = "Assets/Scenes/" + sceneName + ".unity";
        int index = SceneUtility.GetBuildIndexByScenePath(sceneName);
        return index;
    }
    public static string getSceneNameByIndex(int index)
    {
        string str = SceneUtility.GetScenePathByBuildIndex(index);
        Match match = Regex.Match(str, "[^/]*?(?=\\.unity)");
        if(match.Success)
        {
            Debug.Log(str+"-->"+index + "-->" + match.Value);
            return match.Value;
        }
        else
        {
            Debug.LogError("场景不存在");
            return "";
        }
    }
    #endregion
    #region 打开文件夹
    public static string SelectFile(string fileExtension)
    {
        return FolderBrowserHelper.SelectFile(fileExtension);
    }
    public static string GetPathFromWindowsExplorer(string title)
    {
        return FolderBrowserHelper.GetPathFromWindowsExplorer(title);
    }
    #endregion
    #region UI事件
    public static bool MouseInUI()// 当前鼠标是否在UI上面
    {
        var m_Raycaster = GameObject.FindObjectsOfType<GraphicRaycaster>();
        if (m_Raycaster == null)
        {
            return false;
        }
        PointerEventData data = new PointerEventData(EventSystem.current);
        data.pressPosition = Input.mousePosition;
        data.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        foreach (var ray in m_Raycaster)
        {
            ray.Raycast(data, results);
            if (results.Count != 0)
            {
                return true;

            }
        }
        return false;
    }
    public static bool MouseInInput()// 当前鼠标是否在输入字段上面
    {
        GameObject obj = EventSystem.current.currentSelectedGameObject;
        if (obj != null)
        {
            if (obj.GetComponent<InputField>() != null || obj.GetComponent<TMP_InputField>() != null)
            {
                return true;
            }
        }
        return false;
    }
    #endregion
    #region 加载远程图片
    public static IEnumerator DownLoadImage(Image image, string url,Action action)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {
            yield return uwr.SendWebRequest();
            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(uwr.error);
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
                Sprite sprite = GameUtil.Texture2Sprite(texture);
                image.sprite = sprite;
                action?.Invoke();
            }
        }
    }
    #endregion
}
