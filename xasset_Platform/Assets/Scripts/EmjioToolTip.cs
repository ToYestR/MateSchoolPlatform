/************************************************
 * Author       :   XXY
 * Blog         :   https://www.cnblogs.com/jzyl
 * Email        :   1047185209@QQ.com
 * FileName     :   EmjioToolTip.cs
 * CreateData   :   2023/2/9 18:15:28
 * UnityVersion :   2020.3.33f1
 * Description  :   描述
************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EmjioToolTip : MonoBehaviour
{
    public TextMeshProUGUI m_Info;
    public void Awake()
    {
        if (m_Info)
        {
            m_Info.text = Global.nickname;
        }
    }
    void Update()
    {
        if (Camera.main)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(transform.position-Camera.main.transform.position), 10 * Time.deltaTime);
        }
    }
}
