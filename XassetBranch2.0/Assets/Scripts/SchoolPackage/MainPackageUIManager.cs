using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace XAsset
{
    public class MainPackageUIManager : MonoBehaviour
    {
        [Header("退出界面")]
        public Button loginout_btn;
        public Button applicationQuit_btn;

        //设置面板
        public GameObject m_SettingWindow;
        //项目设置按钮
        public Toggle m_setting_tog;
        void Start()
        {
            m_setting_tog.onValueChanged.AddListener(SetSettingWindw);

            loginout_btn.onClick.AddListener(() => {

                SceneManager.LoadScene(2);
            });
            applicationQuit_btn.onClick.AddListener(() => { Application.Quit(); });

        }
        public void SetSettingWindw(bool Status)
        {
            m_SettingWindow.SetActive(Status);
        }
    }
}
