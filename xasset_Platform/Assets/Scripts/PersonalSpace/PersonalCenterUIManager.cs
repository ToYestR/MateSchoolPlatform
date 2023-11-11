using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Im;

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

        [Header("��װ����Ľ��밴ť")]
        public Button m_Faceliftbtn;

        [Header("�����������չ������")]
        public Button collapse_btn;
        public Button expand_btn;
        public GameObject rightpanel;


        private GameObject m_mainCamera;
        void Start()
        {

            m_SceneSelectbtn.onValueChanged.AddListener(OnSelectToggleChanged);

            loginout_btn.onClick.AddListener(() => {
                //ClientManager.Instance.Send(new Logout() { });
                PlayerPrefs.SetString("Username", "");
                PlayerPrefs.SetString("Password", "");

                SceneManager.LoadScene(0); });
            applicationQuit_btn.onClick.AddListener(() => { Application.Quit(); });
            m_setting_tog.onValueChanged.AddListener(SetSettingWindw);

            //���뻻װ����
            m_Faceliftbtn.onClick.AddListener(()=>SceneManager.LoadScene(4));

            //չ����������߼�
            //collapse_btn.onClick.AddListener(() => { rightpanel.SetActive(false)});
            //expand_btn.onClick.AddListener(() => { });
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
