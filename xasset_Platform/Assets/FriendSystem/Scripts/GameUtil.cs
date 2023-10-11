using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.Networking;
using TMPro;
namespace FriendSystem
{
    public class GameUtil
    {
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
        /// <summary>
        /// 当前鼠标是否在UI上面
        /// </summary>
        public static bool MouseInUI()
        {
            return EventSystem.current.IsPointerOverGameObject();

        }
        /// <summary>
        /// 加载远程图片
        /// </summary>
        public static IEnumerator DownLoadImage(Image image, string url, Action action)
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
    }
}