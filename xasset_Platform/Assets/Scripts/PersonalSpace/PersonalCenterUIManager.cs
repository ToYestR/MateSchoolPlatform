using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace XAsset
{
    public class PersonalCenterUIManager : MonoBehaviour
    {
        //星球选项
        [Header("场景星球的开关")]
        public Toggle m_SceneSelectbtn;
        [Header("场景星球面板")]
        public GameObject m_SceneManagerWindows;
        //星球主控相机
        public GameObject m_SceneSelectCamera;
        //菜单展开

        //菜单收起

        //设置面板
        public GameObject m_SettingWindow;
        //项目设置按钮
        public Toggle m_setting_tog;
        [Header("退出界面")]
        public Button loginout_btn;
        public Button applicationQuit_btn;

        private GameObject m_mainCamera;
        void Start()
        {

            m_SceneSelectbtn.onValueChanged.AddListener(OnSelectToggleChanged);

            loginout_btn.onClick.AddListener(() => { SceneManager.LoadScene(0); });
            applicationQuit_btn.onClick.AddListener(() => { Application.Quit(); });
            m_setting_tog.onValueChanged.AddListener(SetSettingWindw);

        }

        // Update is called once per frame
        void Update()
        {
        
        }
        /// <summary>
        /// 加入个人空间的响应时间
        /// </summary>
        /// <param name="Status"></param>
        public void OnSelectToggleChanged(bool Status)
        {
            if(!m_mainCamera) m_mainCamera = Camera.main.gameObject;
            m_mainCamera.SetActive(!Status);
            //开启主相机
            m_SceneSelectCamera.SetActive(Status);
            //开启场景球面板
            m_SceneManagerWindows.SetActive(Status);

        }
        /// <summary>
        /// 开关设置面板
        /// </summary>
        /// <param name="Status"></param>
        public void SetSettingWindw(bool Status)
        {
            m_SettingWindow.SetActive(Status);
        }
    }
}
