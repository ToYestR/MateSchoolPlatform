using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using libx;
public class SchoolItem : MonoBehaviour
{
    public TextMesh m_title;

    private Updater_Item m_updateritem;
    /// <summary>
    /// 对应主包名
    /// </summary>
    public string Package_name;
    /// <summary>
    /// 对应主场景名
    /// </summary>
    public string Scene_name;
    /// <summary>
    /// 学校名称
    /// </summary>
    public string school_name;
    /// <summary>
    /// 全景图MeshRenderer
    /// </summary>
    public MeshRenderer m_image;
    /// <summary>
    /// 资源名称
    /// </summary>
    public string TitileName;
    /// <summary>
    /// 链接地址
    /// </summary>
    public string link_url;

    public string scene_id;
    /// <summary>
    /// npc文字高度
    /// </summary>
    public float npcHeight = 100f;

    public string context;
    public void Awake()
    {
        m_image = GetComponent<MeshRenderer>();

        m_updateritem = GetComponent<Updater_Item>();
    }
    public void Start()
    {
        //if (ServerConfig.PackageIsLoaded.ContainsKey(TitileName))
        //{
        //    if (ServerConfig.PackageIsLoaded[TitileName])
        //    {
        //        context = TitileName + "[加载完成]";
        //    }
        //}
    }
    private void Update()
    {

    }
    public void OnMouseDown()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
        Global.currentschoolname = school_name;
        //Debug.Log(Scene_name);
        m_updateritem.Restart(link_url, TitileName, Scene_name);
        Global.curentschoolurl = link_url;
        Global.currentfilename = TitileName;
        Global.currentsenename = Scene_name;
        //context = TitileName + "\n[下载中]";
    }
    public void ShowProgess(string text)
    {
        m_title.text = TitileName + "\n" + text;
    }
    public void OnMouseUp()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = Color.white;

        //if(Global.UserName==null)
        //{
        //    return;
        //}
        //gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
        //if (ServerConfig.PackageIsLoaded.ContainsKey(TitileName))
        //{
        //    if (ServerConfig.PackageIsLoaded[TitileName])
        //    {
        //        Global.Goto_Scenename= Scene_name;
        //        SceneManager.LoadScene("02_Loading");
        //        //SceneManager.LoadScene(Scene_name);
        //    }
        //}
        //else
        //{
        //    StarLoad(Package_name);
        //}
    }
    //public void O
    public void SetSchool(string name ,string link_str,string id)
    {
        Scene_name = name;
        scene_id = id;
        TitileName = id+name;
        context = id+name;
        m_title.text = id + name;
        link_url = link_str;
    }
 

    void StarLoad(string scenename)
    {
        context = TitileName + "\n[下载中]";
        //FindObjectOfType<AssetBundleDownloader>().LoadSchoolResFunc(link_url, TitileName, OnLoadCompelte);
        //AssestBundleTest.instance.DownLoadAssestBundle(link, scenename, OnLoadCompelte, OnDownLoad);
    }
    void OnLoadCompelte()
    {
        context = TitileName + "[下载完成]";
        StartCoroutine(LoadSceneAB(LoadAssestBundle));
    }
    //void OnDownLoad(float progrees)
    //{
    //    m_title.text = TitileName + "[下载中]" + progrees.ToString() + "%";
    //}
    void LoadAssestBundle()
    {
        context = TitileName + "[加载完成]";
        //if(ServerConfig.PackageIsLoaded.ContainsKey(TitileName))
        //{
        //    ServerConfig.PackageIsLoaded[TitileName] = true;
        //}
        //else
        //{
        //    ServerConfig.PackageIsLoaded.Add(TitileName, true);
        //}
    }
    /// <summary>
    /// 加载changj
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    IEnumerator LoadSceneAB(Action action = null)
    {
        //string assetBundleName = "scenes/" + sceneName + ".unity.ab";
        string path = Application.streamingAssetsPath + "/AssetBundles/" + Package_name;
        UnityWebRequest assetBundle = UnityWebRequestAssetBundle.GetAssetBundle(path);
        yield return assetBundle.SendWebRequest();
        AssetBundle ab = DownloadHandlerAssetBundle.GetContent(assetBundle);
        //loadPath= Application.streamingAssetsPath + sceneName;
        Debug.Log(path);
        //AssetBundleCreateRequest myLoadedAssetBundle = AssetBundle.LoadFromFileAsync(path);
        //yield return myLoadedAssetBundle;
        if (ab == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            yield break;
        }
        if (ab != null)
        {
            if (action != null)
                action.Invoke();
        }
    }
    //void OnGUI()
    //{    //得到NPC头顶在3D世界中的坐标
    //     //默认NPC坐标点在脚底下，所以这里加上npcHeight它模型的高度即可
    //    Vector3 worldPosition = new Vector3(transform.position.x, transform.position.y + npcHeight, transform.position.z);
    //    //根据NPC头顶的3D坐标换算成它在2D屏幕中的坐标
    //    Vector2 position = Camera.main.WorldToScreenPoint(worldPosition);
    //    //得到真实NPC头顶的2D坐标
    //    position = new Vector2(position.x, Screen.height - position.y);
    //    //计算NPC名称的宽高
    //    GUIStyle gui = new GUIStyle();
    //    gui.normal.textColor = Color.yellow;
    //    gui.fontSize = 20;
    //    Vector2 nameSize = GUI.skin.label.CalcSize(new GUIContent(context));
    //    //设置名称显示颜色为黄色
    //    GUI.color = Color.yellow;
    //    //绘制NPC名称
    //    GUI.Label(new Rect(position.x - (nameSize.x / 2), position.y - nameSize.y, nameSize.x, nameSize.y), context, gui);
    //}
}
