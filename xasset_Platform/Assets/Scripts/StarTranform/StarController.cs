using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

//挂载在父物体上的，用于解析json数据，然后将数据分发给子物体
public class StarController : MonoBehaviour
{
    public string jsonString;
    public string json_test;
    public List<int> assignedChildrenIndices = new List<int>();

    private void Start()
    {
        Debug.Log("执行Start");
        JObject jobject = new JObject();
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", Global.token);
        WebRequestController.Instance.Post(ApiCore.GetUrl(ApiCore.Url_ListUnAuditMainScene), jobject.ToString(), header, Jsondeserialization);
        //Jsondeserialization(jsonString);
    }

    private void OnEnable()
    {
        //Jsondeserialization(jsonString);
        //JObject jobject = new JObject();
        //Dictionary<string, string> header = new Dictionary<string, string>();
        //header.Add("Authorization", Global.token);
        //WebRequestController.Instance.Post(ApiCore.GetUrl(ApiCore.Url_ListUnAuditMainScene), jobject.ToString(), header, Jsondeserialization);

    }

    public void Jsondeserialization(string json)
    {
        Debug.Log(json);
        JObject jobject1 = JsonConvert.DeserializeObject<JObject>(json);
        if (jobject1 == null)
        {
            Debug.LogWarning("Failed to parse JSON.");
            return;
        }
        JArray jarray1 = (JArray)jobject1["data"];
        List<StarDetail> starDetails = jarray1.ToObject<List<StarDetail>>();
        for (int i = 0; i < Mathf.Min(transform.childCount, starDetails.Count); i++)
        {
            StarDetailComponent childComponent = transform.GetChild(i).GetComponent<StarDetailComponent>();
            childComponent.isActived = true;
            if (childComponent != null && !assignedChildrenIndices.Contains(i))
            {
                childComponent.mainScene = starDetails[i].mainScene;
                childComponent.uploadDesc = starDetails[i].uploadDesc;
                childComponent.logotypeImage = starDetails[i].logotypeImage;
                childComponent.ossParentPath = starDetails[i].ossParentPath;
                childComponent.id = starDetails[i].id;

                assignedChildrenIndices.Add(i);
                childComponent.TitleText();
            }
        }
    }

    [System.Serializable]
    public class StarDetail
    {
        public string id;
        public string mainScene;
        public string uploadDesc;
        public string logotypeImage;
        public string ossParentPath;
    }
}
