using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
namespace FriendSystem
{
    public class Utility 
    {
        /// <summary>
        /// 开启携程加载远程图片
        /// </summary>
        /// <param name="url"></param>
        /// <param name="img"></param>
        /// <returns></returns>
        public static IEnumerator URLToSprite(string url,Image img)
        {
            using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    var tex = DownloadHandlerTexture.GetContent(www);
                    Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f);
                    img.sprite = sprite;
                }
                else
                {
                    Debug.Log("加载纹理失败");
                }
            }
        }
    }
}
