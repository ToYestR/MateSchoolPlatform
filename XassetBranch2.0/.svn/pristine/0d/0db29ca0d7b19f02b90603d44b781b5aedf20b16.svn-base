using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FriendSystem
{
    /// <summary>
    /// 好友面板 个人中心 
    /// </summary>
    public class FriendSystemCanvas : MonoBehaviour
    {
        public static FriendSystemCanvas getInstance;
        public List<UniversityInfo> universities = new List<UniversityInfo>();      // 中国各大大学学校
        private void Awake()
        {
            if (FindObjectsOfType<FriendSystemCanvas>(true).Length > 1)
            {
                Destroy();
            }
            getInstance = this;
            //DontDestroyOnLoad(this);
        }
        private void Start()
        {
            Init();
        }
        private void Init()
        {
            InfoHandle.SearchAllSchools(OnUniversities);
        }
        /// <summary>
        /// 手动 销毁 该对象
        /// </summary>
        public void Destroy()
        {
            DestroyImmediate(gameObject);
        }

        #region 服务器响应
        /// <summary>
        /// 获取服务器所有学校列表 
        /// </summary>
        private void OnUniversities(string json)
        {
            Debug.Log(json);
            JObject jObj = JsonConvert.DeserializeObject<JObject>(json);
            int code = (int)jObj["code"];
            string msg = (string)jObj["msg"];
            if (code == 200)
            {
                JArray data = (JArray)jObj["data"];
                foreach (var item in data)
                {
                    UniversityInfo info = item.ToObject<UniversityInfo>();
                    universities.Add(info);
                }
                Debug.Log("成功获取学校：" + universities.Count);
            }
            else
            {
                Debug.LogError("获取学校列表失败:" + msg);
            }
        }
        #endregion

    }
}