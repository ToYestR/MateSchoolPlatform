using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using libx;
public class SceneInfo : MonoBehaviour
{
    public Text m_Infotext;
    public Slider m_slider;
    public void Start()
    {
        FindObjectOfType<Updater_Loader>().oncompleted += SetInfo;
    }
    public void SetInfo(string infostr, float value)
    {
        Debug.Log(value);
        m_Infotext.text = infostr;
        m_slider.value = value;
    }
}
