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
    /// ������
    /// </summary>
    public float validTouchDistance = 200f; //200
    /// <summary>
    /// ������ģ������������ʱ�ڷ�
    /// </summary>
    public GameObject m_objgame;
    /// <summary>
    /// �ɱ༭�����
    /// </summary>
    public GameObject m_EditPanel;
    /// <summary>
    /// Ҫ���ɵĳ���ITEMԪ��
    /// </summary>
    public GameObject m_SceneItem;
    /// <summary>
    /// �Ƿ��ڱ༭��
    /// </summary>
    public bool IsInEdit = false;

    public string layerName = "Plane";
    /// <summary>
    /// ������ʾ���ư�ť
    /// </summary>
    public Button editbutton;
    /// <summary>
    /// ���ڱ���λ�õ�
    /// </summary>
    private Vector3 m_clickposition;
    /// <summary>
    /// ��ǰѡ��ĳ���id
    /// </summary>
    public string currentsceneid;
    [Header("���水ť")]
    public Button btn_Save;
    [Header("���͵㸸����")]
    public Transform trparent;
    public void Awake()
    {

        //����
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
    /// ��ʼ�����༭
    /// </summary>
    public void StartEdit()
    {
        //����Ӧ�ı༭��ʽ
        //����Ӧ�ı༭
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
                    if (gameObj.name != "����443") return;
                    Vector3 hitPoint = hitInfo.point;
                    Debug.Log("click object name is " + gameObj.name + " , hit point " + hitPoint.ToString());
                    //������Ӧ����
                    //GameObject g1 = Instantiate(m_objgame);
                    //g1.transform.position = hitPoint + Vector3.up * 0.3f;
                    //g1.transform.localEulerAngles = Vector3.zero;

                    m_clickposition = hitPoint + Vector3.up * 0.3f;
                    //��ѯ��������Ӧ���ӳ���
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
    /// �����λ��ѯ�ķ��ؽ��
    /// </summary>
    /// <param name="json"></param>
    public void selectScenePackageTransmissionRsp(string json)
    {
        m_EditPanel.SetActive(true);


    }
    /// <summary>
    /// �����ӳ�����ѯ�ķ��ؽ��
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
    /// �����Ӧ��λ
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
        Debug.Log("��ʼ��ѯ");
        WebRequestController.Instance.Post(ApiCore.GetUrl(ApiCore.selectScenePackageTransmission), jobject.ToString(), header, RefreshTransmissionHandle);
    }
    /// <summary>
    /// ˢ�²�������Ӧ�Ĵ�����
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
            //��������ֵ
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
