using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SceneItem : MonoBehaviour
{
    public string sceneid;
    public Text developername_text;
    public Text scenename_text;
    public Text url_text;
    public Text Enterscenename_text;
    public Text updatetime_text;

    public void Start()
    {
        GetComponent<Toggle>().onValueChanged.AddListener((b) =>
        {
            TestPlace.instance.currentsceneid = sceneid;
        }
        );
    }
    public void SetInfo(string id,string developername,string scenename,string url,string enterscenename,string updatetime)
    {
        sceneid = id;
        developername_text.text = developername;
        scenename_text.text = scenename;
        url_text.text = url;
        Enterscenename_text.text = enterscenename;
        updatetime_text.text = updatetime;
    }
    
}
