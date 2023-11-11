using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using libx;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class AvatarSys : MonoBehaviour
{
    public static AvatarSys _instance;
    private Transform girlSourceTrans;//女孩资源模型的位置
    private GameObject girlTarget;//女孩骨骼模型，换装的目标
    private Dictionary<string, Dictionary<string, SkinnedMeshRenderer>> girlData = new Dictionary<string, Dictionary<string, SkinnedMeshRenderer>>();//女孩存储资源模型的所有的资源信息
    private Transform[] girlHips;//女孩模型的所有的骨骼信息
    private Dictionary<string, SkinnedMeshRenderer> girlSmr = new Dictionary<string, SkinnedMeshRenderer>();//女孩换装骨骼身上的骨骼网格体组件信息
    private string[,] girlStr = new string[,] { { "hair", "1" }, { "top", "1" }, { "pants", "1" }, { "shoes", "1" }, { "head", "1" } };//女孩换装的部位和对应的编号

    private Transform boySourceTrans;//男孩资源模型的位置
    private GameObject boyTarget;//男孩骨骼模型，换装的目标
    private Dictionary<string, Dictionary<string, SkinnedMeshRenderer>> boyData = new Dictionary<string, Dictionary<string, SkinnedMeshRenderer>>();//男孩存储资源模型的所有的资源信息
    private Transform[] boyHips;//男孩模型的所有的骨骼信息
    private Dictionary<string, SkinnedMeshRenderer> boySmr = new Dictionary<string, SkinnedMeshRenderer>();//男孩换装骨骼身上的骨骼网格体组件信息
    private string[,] boyStr = new string[,] { { "hair", "1" }, { "top", "1" }, { "pants", "1" }, { "shoes", "1" }, { "head", "1" } };//男孩换装的部位和对应的编号

    public int sexCount = 0;//0代表小女孩，1代表小男孩
    public GameObject girlPanel;
    public GameObject boyPanel;

    public string parttest;
    public string numtest;
    public string[,] girlstrtest = new string[,] { { "hair", "1" }, { "top", "1" }, { "pants", "1" }, { "shoes", "1" }, { "head", "1" } };
    public string[,] boystrtest = new string[,] { { "hair", "1" }, { "top", "1" }, { "pants", "1" }, { "shoes", "1" }, { "head", "1" } };

    public int sexJson;
    public string partJson;
    public string numJson;
    public string girlJson;
    public string boyJson;

    public GameObject m_Loadingpanel;
    public GameObject editChildSceneWindows;

    [Header("临时添加的两个toggle")]
    public Toggle m_boytoggle;
    public Toggle m_girltoggle;
    private void Awake()
    {
        _instance = this;
        //DontDestroyOnLoad(this.gameObject);//跳转场景时不删除游戏物体
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("执行初始化");
        //GirlAvater();
        BoyAvater();
        LoadJson(Global.roleinfo);
        if (sexCount == 0)
        {
            //m_girltoggle.isOn = true;
            //GirlAvatertest();
            boyPanel.SetActive(false);
            girlPanel.SetActive(true);
            girlTarget.SetActive(true);
            boyTarget.SetActive(false);
            InitAvatarGirltest();
        }
        else
        {
            //m_boytoggle.isOn = true;
            boyPanel.SetActive(true);
            //girlPanel.SetActive(false);
            //BoyAvatertest();
            //YZL临时注释掉女性部分
            //girlTarget.SetActive(false);
            boyTarget.SetActive(true);
            InitAvatarBoytest();
        }

        boyTarget.AddComponent<SpinWithMouse>();
        //girlTarget.AddComponent<SpinWithMouse>();
    }

    private void Update()
    {

    }

    public void GirlAvater()
    {
        InstantiateGirl();
        SaveData(girlSourceTrans, girlData, girlTarget, girlSmr);
        InitAvatarGirl();
    }

    public void GirlAvatertest()
    {
        InstantiateGirl();
        SaveData(girlSourceTrans, girlData, girlTarget, girlSmr);
        InitAvatarGirltest();
    }

    public void BoyAvater()
    {
        InstantiateBoy();
        SaveData(boySourceTrans, boyData, boyTarget, boySmr);
        InitAvatarBoy();
    }

    public void BoyAvatertest()
    {
        InstantiateBoy();
        SaveData(boySourceTrans, boyData, boyTarget, boySmr);
        InitAvatarBoytest();
    }

    private void InstantiateGirl()
    {
        //加载资源模型
        GameObject goGril = Instantiate(Resources.Load("Role/FemaleModel")) as GameObject;
        girlSourceTrans = goGril.transform;
        goGril.SetActive(false);
        girlTarget = Instantiate(Resources.Load("Role/FemaleTarget")) as GameObject;
        girlHips = girlTarget.GetComponentsInChildren<Transform>();
    }

    private void InstantiateBoy()
    {
        GameObject goBoy = Instantiate(Resources.Load("Role/MaleModel")) as GameObject;
        boySourceTrans = goBoy.transform;
        goBoy.SetActive(false);
        boyTarget = Instantiate(Resources.Load("Role/MaleTarget")) as GameObject;
        boyHips = boyTarget.GetComponentsInChildren<Transform>();
    }

    /// <summary>
    /// 存储资源模型的所有的资源信息
    /// </summary>
    /// <param name="souceTrans">原模型的位置信息</param>
    /// <param name="data">使用字典嵌套来存储名字与对应的骨骼网格体</param>
    /// <param name="target">需要换装的骨骼</param>
    /// <param name="smr">使用字典来对需要换装的骨骼进行名字和对应的骨骼网格体进行存储</param>
    private void SaveData(Transform souceTrans, Dictionary<string, Dictionary<string, SkinnedMeshRenderer>> data, GameObject target, Dictionary<string, SkinnedMeshRenderer> smr)
    {
        //在调用前就要清空字典，防止再次调用时，出现重复存储的问题
        data.Clear();
        smr.Clear();
        if (souceTrans == null)
            return;
        SkinnedMeshRenderer[] parts = souceTrans.GetComponentsInChildren<SkinnedMeshRenderer>();//遍历所有的子物体有骨骼网格体组件，然后进行存储
        foreach (var part in parts)
        {
            string[] names = part.name.Split('-');
            Debug.Log(part.name);
            if (!data.ContainsKey(names[0]))
            {
                //生成对应的部位，且只生成一个
                GameObject partGo = new GameObject();
                partGo.name = names[0];
                partGo.transform.parent = target.transform;
                smr.Add(names[0], partGo.AddComponent<SkinnedMeshRenderer>());//把骨骼target身上的骨骼网格体组件添加到字典中
                data.Add(names[0], new Dictionary<string, SkinnedMeshRenderer>());
            }
            data[names[0]].Add(names[1], part);//存储所有的骨骼网格体组件到字典中
        }
    }

    /// <summary>
    /// 传入部位，编号，从data里面拿取相对应的骨骼网格体组件，然后换装
    /// </summary>
    /// <param name="part">对应的部位</param>
    /// <param name="num">对应的部位的编号</param>
    /// <param name="data">使用字典嵌套来存储名字与对应的骨骼网格体</param>
    /// <param name="hips">模型的所有的骨骼信息</param>
    /// <param name="smr">使用字典来对需要换装的骨骼进行名字和对应的骨骼网格体进行存储</param>
    /// <param name="str">换装的部位和对应的编号的二维数组</param>
    private void ChangeMesh(string part, string num, Dictionary<string, Dictionary<string, SkinnedMeshRenderer>> data, Transform[] hips, Dictionary<string, SkinnedMeshRenderer> smr, string[,] str)
    {
        SkinnedMeshRenderer skm = data[part][num];//要更换的部位
        List<Transform> bones = new List<Transform>();
        foreach (var trans in skm.bones)
        {
            foreach (var bone in hips)
            {
                if (bone.name == trans.name)
                {
                    bones.Add(bone);
                    break;
                }
            }
        }
        //换装实现
        smr[part].bones = bones.ToArray();//绑定骨骼
        smr[part].materials = skm.materials;//替换材质
        smr[part].sharedMesh = skm.sharedMesh;//更换骨骼网格体

        SaveData(part, num, str);//在换装后，保存当前的部位和编号在新的场景中使用
    }

    /// <summary>
    /// 初始化骨骼，让它有mesh,材质，骨骼信息
    /// </summary>
    private void InitAvatarGirl()
    {
        int length = girlStr.GetLength(0);//获得行数
        for (int i = 0; i < length; i++)
        {
            ChangeMesh(girlStr[i, 0], girlStr[i, 1], girlData, girlHips, girlSmr, girlStr);//换装
        }
    }

    private void InitAvatarGirltest()
    {
        int length = girlstrtest.GetLength(0);//获得行数
        for (int i = 0; i < length; i++)
        {
            //ChangeMesh(strtest[i, 0], strtest[i, 1], datatest, girlHips, smrtest, strtest);//换装
            ChangeMesh(girlstrtest[i, 0], girlstrtest[i, 1], girlData, girlHips, girlSmr, girlstrtest);//换装
        }
    }

    private void InitAvatarBoy()
    {
        int length = boyStr.GetLength(0);//获得行数
        for (int i = 0; i < length; i++)
        {
            ChangeMesh(boyStr[i, 0], boyStr[i, 1], boyData, boyHips, boySmr, boyStr);//换装
        }
    }

    private void InitAvatarBoytest()
    {
        int length = boystrtest.GetLength(0);//获得行数
        for (int i = 0; i < length; i++)
        {
            ChangeMesh(boystrtest[i, 0], boystrtest[i, 1], boyData, boyHips, boySmr, boystrtest);//换装
        }
    }

    /// <summary>
    /// 对应不同的性别，进行换装
    /// </summary>
    /// <param name="part">对应的部位</param>
    /// <param name="num">对应的部位的编号</param>
    public void OnChangePeople(string part, string num)
    {
        if (sexCount == 0)
        {
            ChangeMesh(part, num, girlData, girlHips, girlSmr, girlStr);
        }
        else
        {
            ChangeMesh(part, num, boyData, boyHips, boySmr, boyStr);
        }
    }

    /// <summary>
    /// 改变不同的角色和隐藏显示对应的骨骼和UI
    /// </summary>
    public void SexChange()
    {
        if (sexCount == 1)
        {
            boyTarget.SetActive(true);
            girlTarget.SetActive(false);
            boyPanel.SetActive(true);
            girlPanel.SetActive(false);
            sexJson = Int32.Parse(JsonConvert.SerializeObject(sexCount));
        }
        else
        {
            boyTarget.SetActive(false);
            girlTarget.SetActive(true);
            boyPanel.SetActive(false);
            girlPanel.SetActive(true);
            sexJson = Int32.Parse(JsonConvert.SerializeObject(sexCount));
        }
    }

    /// <summary>
    /// 保存当前的部位和编号,在新场景中进行初始化
    /// </summary>
    /// <param name="part">对应的部位</param>
    /// <param name="num">对应的部位的编号</param>
    /// <param name="str">换装的部位和对应的编号的二维数组</param>
    private void SaveData(string part, string num, string[,] str)
    {

        int length = str.GetLength(0);
        for (int i = 0; i < length; i++)
        {
            if (str[i, 0] == part)
            {
                str[i, 1] = num;
            }
        }
        partJson = JsonConvert.SerializeObject(part);
        numJson = JsonConvert.SerializeObject(num);
        boyJson = JsonConvert.SerializeObject(str);
        girlJson = JsonConvert.SerializeObject(str);
    }

    public void LoadJson(string json)
    {
        if (json.Length < 2) return;
        JObject jobject = JsonConvert.DeserializeObject<JObject>(json);
        sexCount = Int32.Parse(jobject["sexJson"].ToString());
        parttest = JsonConvert.DeserializeObject<string>(jobject["partJson"].ToString());
        numtest = JsonConvert.DeserializeObject<string>(jobject["numJson"].ToString());
        girlstrtest = JsonConvert.DeserializeObject<string[,]>(jobject["girlJson"].ToString());
        boystrtest = JsonConvert.DeserializeObject<string[,]>(jobject["boyJson"].ToString());
    }

    public void SaveJson()
    {
        JObject jobTest = new JObject();
        jobTest.Add("sexJson", sexJson);
        jobTest.Add("partJson", partJson);
        jobTest.Add("numJson", numJson);
        jobTest.Add("girlJson", girlJson);
        jobTest.Add("boyJson", boyJson);

        LoadJson(jobTest.ToString());
        Debug.Log(jobTest.ToString());
        Global.roleinfo = jobTest.ToString();
        //获得角色信息
        //WebRequestController.Instance.Post(ApiCore.GetUrl(ApiCore.Url_EditStudengtInfo), userjobj.ToString(), header, Debug.Log);
        //保存角色信息
        JObject userjobj = new JObject();
        userjobj.Add("id", Global.uid);
        userjobj.Add("userName", Global.UserName);
        userjobj.Add("nickName", Global.nickname);
        userjobj.Add("roleInfo", Global.roleinfo);
        Debug.Log(userjobj.ToString());
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", Global.token);
        WebRequestController.Instance.Post(ApiCore.GetUrl(ApiCore.Url_EditStudengtInfo), userjobj.ToString(), header, Debug.Log);
        //WebRequestController.Instance.Post(ApiCore.GetUrl(ApiCore.Url_ListUnAuditMainScene), userjobj.ToString(), header, Debug.Log);
        //打开场景选择界面
        SceneManager.LoadScene(2);
        //if (Global.currentseneid == "")
        //{
        //    editChildSceneWindows.SetActive(true);
        //    Global.currentseneid = "131";
        //}
        //else
        //{
        //    EnterScene();
        //}
    }
    /// <summary>
    /// 返回主界面
    /// </summary>
    public void ReLogin()
    {
        SceneManager.LoadScene(0);
    }
    public void EnterScene()
    {
    }


}