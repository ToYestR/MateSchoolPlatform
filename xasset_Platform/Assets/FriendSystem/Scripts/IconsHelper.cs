using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace FriendSystem
{
    /// <summary>
    /// 图片下载管理器
    /// </summary>
    public class IconsHelper
    {
        private Dictionary<string, ImgProcessor> icons = new Dictionary<string, ImgProcessor>();    // 图片精灵集合
        private static IconsHelper instance;
        private static Mono GetMono;
        public static IconsHelper getInstance
        {
            get
            {
                if(instance==null)
                {
                    instance = new IconsHelper();
                    GameObject go = new GameObject("IcosHelper");
                    MonoBehaviour.DontDestroyOnLoad(go);
                    go.hideFlags = HideFlags.HideInHierarchy;
                    GetMono =go.AddComponent<Mono>();

                }
                return instance;
            }
        }
        
        /// <summary>
        /// 设置图片精灵
        /// </summary>
        /// <param name="img"></param>
        public void SetImageSprite(Image img,string url)
        {
            if(icons.ContainsKey(url))
            {
                icons[url].Process(img);
            }
            else
            {
                icons.Add(url, new ImgProcessor(img));
                GetMono.StartCoroutine(DownLoadTexture(url));
            }
        }
        private IEnumerator DownLoadTexture(string url)
        {
            using(var uwr = UnityWebRequestTexture.GetTexture(url))
            {
                yield return uwr.SendWebRequest();
                if (uwr.result == UnityWebRequest.Result.Success)
                {
                    var tex = DownloadHandlerTexture.GetContent(uwr);
                    Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f);
                    icons[url].Processor(sprite);
                }
                else
                {
                    Debug.LogError("加载纹理失败：" + url);
                }
            }
        }
        /// <summary>
        /// 图片处理器
        /// </summary>
        public class ImgProcessor
        {
            private List<Image> imgs = new List<Image>();
            public Sprite sprite;
            public ImgProcessor(Image img)
            {
                imgs.Add(img);
            }
            public void Process(Image img)
            {
                if (sprite)
                {
                    img.sprite = sprite;
                }
                else
                {
                    imgs.Add(img);
                }
            }
            public void Processor(Sprite sprite)
            {
                this.sprite = sprite;
                foreach(var item in imgs)
                {
                    if(item)
                    {
                        item.sprite = sprite;
                    }
                }
                imgs.Clear();
            }
        }
    }
}