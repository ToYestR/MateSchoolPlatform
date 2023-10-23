using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace XAsset
{
    public class PersonalCenterUIManager : MonoBehaviour
    {
        //����ѡ��
        [Header("��������Ŀ���")]
        public Toggle m_SceneSelectbtn;
        [Header("�����������")]
        public GameObject m_SceneManagerWindows;
        //�����������
        public GameObject m_SceneSelectCamera;
        //�˵�չ��

        //�˵�����

        //�������
        public GameObject m_SettingWindow;
        //��Ŀ���ð�ť
        public Toggle m_setting_tog;
        [Header("�˳�����")]
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
        /// ������˿ռ����Ӧʱ��
        /// </summary>
        /// <param name="Status"></param>
        public void OnSelectToggleChanged(bool Status)
        {
            if(!m_mainCamera) m_mainCamera = Camera.main.gameObject;
            m_mainCamera.SetActive(!Status);
            //���������
            m_SceneSelectCamera.SetActive(Status);
            //�������������
            m_SceneManagerWindows.SetActive(Status);

        }
        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="Status"></param>
        public void SetSettingWindw(bool Status)
        {
            m_SettingWindow.SetActive(Status);
        }
    }
}
