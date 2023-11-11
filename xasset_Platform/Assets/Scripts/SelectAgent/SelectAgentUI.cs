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
        /// ������һ���İ�ť
        /// </summary>
        public Button m_NextStepbtn;
        /// <summary>
        /// ���ص�½����İ�ť
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
