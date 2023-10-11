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
    /// ��Ӧ������
    /// </summary>
    public string Package_name;
    /// <summary>
    /// ��Ӧ��������
    /// </summary>
    public string Scene_name;
    /// <summary>
    /// ѧУ����
    /// </summary>
    public string school_name;
    /// <summary>
    /// ȫ��ͼMeshRenderer
    /// </summary>
    public MeshRenderer m_image;
    /// <summary>
    /// ��Դ����
    /// </summary>
    public string TitileName;
    /// <summary>
    /// ���ӵ�ַ
    /// </summary>
    public string link_url;

    public string scene_id;
    /// <summary>
    /// npc���ָ߶�
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
        //        context = TitileName + "[�������]";
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
        //context = TitileName + "\n[������]";
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
        context = TitileName + "\n[������]";
        //FindObjectOfType<AssetBundleDownloader>().LoadSchoolResFunc(link_url, TitileName, OnLoadCompelte);
        //AssestBundleTest.instance.DownLoadAssestBundle(link, scenename, OnLoadCompelte, OnDownLoad);
    }
    void OnLoadCompelte()
    {
        context = TitileName + "[�������]";
        StartCoroutine(LoadSceneAB(LoadAssestBundle));
    }
    //void OnDownLoad(float progrees)
    //{
    //    m_title.text = TitileName + "[������]" + progrees.ToString() + "%";
    //}
    void LoadAssestBundle()
    {
        context = TitileName + "[�������]";
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
    /// ����changj
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
    //{    //�õ�NPCͷ����3D�����е�����
    //     //Ĭ��NPC������ڽŵ��£������������npcHeight��ģ�͵ĸ߶ȼ���
    //    Vector3 worldPosition = new Vector3(transform.position.x, transform.position.y + npcHeight, transform.position.z);
    //    //����NPCͷ����3D���껻�������2D��Ļ�е�����
    //    Vector2 position = Camera.main.WorldToScreenPoint(worldPosition);
    //    //�õ���ʵNPCͷ����2D����
    //    position = new Vector2(position.x, Screen.height - position.y);
    //    //����NPC���ƵĿ��
    //    GUIStyle gui = new GUIStyle();
    //    gui.normal.textColor = Color.yellow;
    //    gui.fontSize = 20;
    //    Vector2 nameSize = GUI.skin.label.CalcSize(new GUIContent(context));
    //    //����������ʾ��ɫΪ��ɫ
    //    GUI.color = Color.yellow;
    //    //����NPC����
    //    GUI.Label(new Rect(position.x - (nameSize.x / 2), position.y - nameSize.y, nameSize.x, nameSize.y), context, gui);
    //}
}
