using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace XAsset
{
    public class SelectAgentUI : MonoBehaviour
    {
        /// <summary>
        /// 设置下一步的按钮
        /// </summary>
        public Button m_NextStepbtn;
        /// <summary>
        /// 返回登陆界面的按钮
        /// </summary>
        public Button m_Backbtn;
        public void Start()
        {
            m_NextStepbtn.onClick.AddListener(()=> {
                SceneManager.LoadScene("SetRole");
            });
            m_Backbtn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("login");
            });
        }
    }
}
