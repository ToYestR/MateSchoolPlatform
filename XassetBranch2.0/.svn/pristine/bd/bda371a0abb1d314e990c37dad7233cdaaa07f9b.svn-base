using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using School_CJ;
using System.Linq;

public class TestPlace : MonoBehaviour
{
    static public TestPlace instance;
    /// <summary>
    /// 最大距离
    /// </summary>
    public float validTouchDistance = 200f; //200
    /// <summary>
    /// 传送阵模板物体用于随时摆放
    /// </summary>
    public GameObject m_objgame;
    /// <summary>
    /// 可编辑的面板
    /// </summary>
    public GameObject m_EditPanel;
    /// <summary>
    /// 要生成的场景ITEM元素
    /// </summary>
    public GameObject m_SceneItem;
    /// <summary>
    /// 是否在编辑中
    /// </summary>
    public bool IsInEdit = false;

    public string layerName = "Plane";
    /// <summary>
    /// 用于显示控制按钮
    /// </summary>
    public Button editbutton;
    /// <summary>
    /// 用于保存位置点
    /// </summary>
    private Vector3 m_clickposition;
    /// <summary>
    /// 当前选择的场景id
    /// </summary>
    public string currentsceneid;
    [Header("保存按钮")]
    public Button btn_Save;
    [Header("传送点父物体")]
    public Transform trparent;
    public void Awake()
    {

        //单例
        instance = this;
        btn_Save.onClick.AddListener(SavePoint);

        editbutton.gameObject.SetActive(Global.isCanEdit);
        if (!Global.isCanEdit)
        {
            RefreshTransmission("");
        }
    }
    public void Start()
    {
        RefreshTransmission("");
    }
    /// <summary>
    /// 开始场景编辑
    /// </summary>
    public void StartEdit()
    {
        //打开相应的编辑方式
        //打开相应的编辑
        IsInEdit = true;

    }
    public void Update()
    {
        if (IsInEdit)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo, validTouchDistance))
                {
                    Debug.Log("catch");
                    GameObject gameObj = hitInfo.collider.gameObject;
                    if (gameObj.name != "对象443") return;
                    Vector3 hitPoint = hitInfo.point;
                    Debug.Log("click object name is " + gameObj.name + " , hit point " + hitPoint.ToString());
                    //生成相应物体
                    //GameObject g1 = Instantiate(m_objgame);
                    //g1.transform.position = hitPoint + Vector3.up * 0.3f;
                    //g1.transform.localEulerAngles = Vector3.zero;

                    m_clickposition = hitPoint + Vector3.up * 0.3f;
                    //查询场景所对应的子场景
                    JObject jobect = new JObject();
                    jobect.Add("mainScenePackageId",/* Global.currentseneid*/131);
                    Dictionary<string, string> header = new Dictionary<string, string>();
                    header.Add("Authorization", Global.token);
                    Debug.Log(jobect.ToString());
                    WebRequestController.Instance.Post(ApiCore.GetUrl(ApiCore.Url_ListUnAuditChildScene), jobect.ToString(), header, GetChildSceneRespond);
                    IsInEdit = false;
                }
            }
        }
    }
    /// <summary>
    /// 处理点位查询的返回结果
    /// </summary>
    /// <param name="json"></param>
    public void selectScenePackageTransmissionRsp(string json)
    {
        m_EditPanel.SetActive(true);


    }
    /// <summary>
    /// 处理子场景查询的返回结果
    /// </summary>
    /// <param name="json"></param>
    public void GetChildSceneRespond(string json)
    {
        Debug.Log(json);
        m_EditPanel.SetActive(true);
        JObject jobject = JsonConvert.DeserializeObject<JObject>(json);
        JArray jarray = (JArray)jobject["data"];
        foreach (JObject j1 in jarray)
        {
            Instantiate(m_SceneItem, m_SceneItem.transform.parent);
            foreach (Transform tm in trparent.GetComponentsInChildren<Transform>(false))
            {
                if (tm.name != trparent.name)
                {
                    Destroy(tm.gameObject);
                }
            }
            m_SceneItem.GetComponent<SceneItem>().SetInfo(j1["id"].ToString(), j1["organizationName"].ToString(), j1["mainScene"].ToString(), j1["ossParentPath"].ToString(), j1["updateTime"].ToString(), j1["unityResourceName"].ToString());
            m_SceneItem.SetActive(true);
        }
    }
    /// <summary>
    /// 保存对应点位
    /// </summary>
    public void SavePoint()
    {
        JObject jobjectv = new JObject();
        jobjectv.Add("x", m_clickposition.x.ToString());
        jobjectv.Add("y", m_clickposition.y.ToString());
        jobjectv.Add("z", m_clickposition.z.ToString());


        JObject jobject = new JObject();
        jobject.Add("transmissionName", Random.Range(0, 10000));
        jobject.Add("transmissionAddress", jobjectv.ToString());
        jobject.Add("childSceneId", currentsceneid);
        jobject.Add("mainSceneId", Global.currentseneid);
        Debug.Log(jobject.ToString());
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", Global.token);
        WebRequestController.Instance.Post(ApiCore.GetUrl(ApiCore.Url_addPackageTransmission), jobject.ToString(), header, RefreshTransmission);
    }
    public void RefreshTransmission(string str)
    {
        JObject jobject = new JObject();
        jobject.Add("mainSceneId", Global.currentseneid);
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", Global.token);
        Debug.Log("开始查询");
        WebRequestController.Instance.Post(ApiCore.GetUrl(ApiCore.selectScenePackageTransmission), jobject.ToString(), header, RefreshTransmissionHandle);
    }
    /// <summary>
    /// 刷新并生成相应的传送阵
    /// </summary>
    /// <param name="str"></param>
    public void RefreshTransmissionHandle(string str)
    {
        Debug.Log(str);
        foreach(Transform tm in trparent.GetComponentsInChildren<Transform>(true))
        {
            if (tm.name != trparent.name)
            {
                Destroy(tm.gameObject);
            }
        }
        JObject jobject = JsonConvert.DeserializeObject<JObject>(str);
        JArray jarray = (JArray)jobject["data"];
        foreach(JObject j1 in jarray)
        {
            JObject jobjectv = JsonConvert.DeserializeObject<JObject>(j1["transmissionAddress"].ToString());
            Vector3 v1 = new Vector3(float.Parse(jobjectv["x"].ToString()), float.Parse(jobjectv["y"].ToString()), float.Parse(jobjectv["z"].ToString()));
            GameObject g1 = Instantiate(m_objgame,trparent);
            g1.transform.position = v1;
            g1.transform.localEulerAngles = new Vector3(-90f, 0, 0);
            //给传送阵赋值
            g1.GetComponent<TeleportationEdit>().TitileName = j1["mainScene"].ToString();
            g1.GetComponent<TeleportationEdit>().InScenceId = j1["childSceneId"].ToString();
            g1.GetComponent<TeleportationEdit>().InScenceName = j1["unityResourcePath"].ToString();
            string url =j1["ossPath"].ToString();
            //Debug.Log(url.Split("/").AsQueryable().Last());
            //url = System.IO.Path.GetFullPath(url);
            url=url.Replace("."+url.Split(".").AsQueryable().Last(),"/");
            Debug.Log(url);
            g1.GetComponent<TeleportationEdit>().url = url;
        }
    }
    public void DeleteTransmission(string str)
    {
        Debug.Log(str);
        JObject jobject = JsonConvert.DeserializeObject<JObject>(str);
        JArray jarray = (JArray)jobject["data"];
        JArray jarray1 = new JArray();
        foreach (JObject j1 in jarray)
        {
            jarray1.Add(j1["id"].ToString());
        }
        JObject jobject1 = new JObject();

        jobject1.Add("ids", jarray1);
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", Global.token);
        WebRequestController.Instance.Post(ApiCore.GetUrl(ApiCore.Url_RemoveTransmission), jobject1.ToString(), header, Debug.Log);

    }
}
