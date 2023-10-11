using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using LitJson;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;

public class AssestBundleStore : MonoBehaviour
{
    public GameObject Item_obj;

    public Transform UniveralSphere;
    [Header("是否接受过审核")]
    public bool IsPassed = true;
    //public Text m_text;
    public void Awake()
    {
        //WWWForm form = new WWWForm();
        //Dictionary<string, string> header = new Dictionary<string, string>();
        ////m_text.text = ServerConfig.GetUrl(ServerConfig.GetPackage);
        //GetComponent<WebRequestController>().Post(ServerConfig.GetUrl(ServerConfig.GetPackage), form, header, OnReceiveMessage);
    }
    public void Start()
    {
        JObject jobject = new JObject();
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", Global.token);
        if (IsPassed)
        {
            WebRequestController.Instance.Post(ApiCore.GetUrl(ApiCore.Url_ListAuditMainScene), jobject.ToString(), header, OnReceiveMessage);
        }
        else
        {
            WebRequestController.Instance.Post(ApiCore.GetUrl(ApiCore.Url_ListUnAuditMainScene), jobject.ToString(), header, OnReceiveMessage);
        }
    }
    public void OnReceiveMessage(string text)
    {
        Debug.Log(text);
        JObject jsondata = JsonConvert.DeserializeObject<JObject>(text);
        JArray item_datalist = (JArray)jsondata["data"];
        Vector3 old_pos = Item_obj.transform.position;
        foreach (JObject item in item_datalist)
        {
            var obj = GameObject.Instantiate(Item_obj);
            SchoolItem m_item = obj.GetComponent<SchoolItem>();
            m_item.SetSchool(item["schoolName"].ToString(), item["ossParentPath"].ToString(), item["id"].ToString());
            //m_item.Package_name = item["package_name"].ToString();
            m_item.Scene_name = item["mainScene"].ToString();
            m_item.school_name = item["schoolName"].ToString();
            //StartCoroutine(DownladImage(item["pic"].ToString(), m_item.m_image));

            obj.transform.parent = UniveralSphere.transform;
            int index = Random.Range(0, 400);
            obj.transform.localPosition = UniveralSphere.GetComponent<MeshFilter>().mesh.vertices[index];
            //old_pos = old_pos + new Vector3(2, 0, 0);
        }
        Item_obj.SetActive(false);
    }
    public IEnumerator DownladImage(string image_url,MeshRenderer meshrenderer)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(image_url);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("图片一下载");
            Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Sprite createSprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(0, 0));
            meshrenderer.materials[0].mainTexture = myTexture;
            //sprite = createSprite;

        }
    }
}

