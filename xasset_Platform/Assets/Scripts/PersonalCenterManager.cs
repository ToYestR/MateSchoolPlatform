using libx;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XAsset
{
    public class PersonalCenterManager : MonoBehaviour
    {
        public TeleportationEdit m_teleport;
        private void Start()
        {
            Init();
        }
        private void Init()
        {
            if (Global.currentseneid != "")
            {
                m_teleport.gameObject.SetActive(true);
                //请求服务端
                //Global.chatNo = "";
                Dictionary<string, string> headers = new Dictionary<string, string>();
                headers.Add("Authorization", Global.token);
                JObject jobjects = new JObject();
                jobjects["id"] = Global.currentseneid;
                WebRequestController.Instance.Post(ApiCore.GetUrl(ApiCore.Url_GetScenePackageInfo), jobjects.ToString(), headers, GetScenePackageInfoHandle);
            }
            else
            {
                m_teleport.gameObject.SetActive(false);
            }
        }
        /// <summary>
        /// 响应单个场景详细信息
        /// </summary>
        /// <param name="result"></param>
        public void GetScenePackageInfoHandle(string result)
        {
            Debug.Log(result);
            //m_Loadingpanel.SetActive(true);
            JObject jobject = JsonConvert.DeserializeObject<JObject>(result);
            string mainscene = jobject["data"]["unityResourcePath"].ToString();
            string path = jobject["data"]["ossParentPath"].ToString();
            string id = jobject["data"]["id"].ToString();
            m_teleport.url = path;
            m_teleport.InScenceName = mainscene;

            //m_teleport.
            //FindObjectOfType<Updater_Loader>().LoadItem(path, id, mainscene);
        }
    }
}
