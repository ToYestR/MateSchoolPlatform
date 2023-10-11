using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using libx;
using System;
using FriendSystem;

namespace XAsset
{
    public class Login : MonoBehaviour
    {
        [Tooltip("���ŵ�½��")]
        public InputField m_phoneIF;
        public InputField m_CheckNumIF;
        [Tooltip("�û���½��")]
        public InputField m_UsernameIF;
        public InputField m_PasswordIF;
        [Tooltip("�û�ע���")]
        public InputField m_UsernameRIF;
        public InputField m_PasswordRIF;
        public InputField m_PhonenumberRIF;
        public InputField m_NickNameRIF;
        public InputField m_CodeRIF;
        [Tooltip("��ʾ��Ϣ")]
        public Text m_InfoText;
        [Header("ȷ�ϵ�tog")]
        public Toggle m_readtog;
        [Tooltip("������֤�밴ť")]
        public Button smsbtn;
        public Button smsrbtn;
        [Header("ֱ����ת�ĳ�������")]
        public string ScenName = "";
        public void Awake()
        {
            
        }
        /// <summary>
        /// ��½ʱ��ȡ��֤��
        /// </summary>
        public void GetSMS()
        {
            string phonenum = m_phoneIF.text;
            JObject jobject= new JObject();
            jobject.Add("phonenumber", phonenum);
            jobject.Add("isLogin", true);
            Dictionary<string, string> header = new Dictionary<string, string>();
            string msTimestamp = WebRequestController.Instance.Timestamp.ToString();
            header.Add("MSTimestamp", msTimestamp);
            string md5_str = WebRequestController.Instance.EncryMd5(msTimestamp, jobject.ToString().Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            Debug.Log(msTimestamp);
            Debug.Log(md5_str);
            header.Add("MSSign", md5_str);
            WebRequestController.Instance.Post(ApiCore.GetUrl(ApiCore.Url_Send), jobject.ToString(), header, SMSLoginHandle); 
        }
        /// <summary>
        /// ע��ʱ�����֤��
        /// </summary>
        public void GetSMSR()
        {
            string phonenum = m_PhonenumberRIF.text;
            JObject jobject = new JObject();
            jobject.Add("phonenumber", phonenum);
            jobject.Add("isLogin", false);
            Dictionary<string, string> header = new Dictionary<string, string>();
            string msTimestamp = WebRequestController.Instance.Timestamp.ToString();
            header.Add("MSTimestamp", msTimestamp);
            string md5_str = WebRequestController.Instance.EncryMd5(msTimestamp, jobject.ToString().Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            //Debug.Log(md5_str);
            header.Add("MSSign", md5_str);
            WebRequestController.Instance.Post(ApiCore.GetUrl(ApiCore.Url_Send), jobject.ToString(), header, SMSRegisterHandle);
        }
        public void Mobilelogin()
        {
            JObject jobject = new JObject();
            jobject.Add("phonenumber",m_phoneIF.text);
            jobject.Add("code",m_CheckNumIF.text);
            Debug.Log(jobject.ToString());
            WebRequestController.Instance.Post(ApiCore.GetUrl(ApiCore.Url_Login), jobject.ToString(), new Dictionary<string,string>() , LoginHandle);
        }
        public void Userlogin()
        {
            JObject jobject = new JObject();
            jobject.Add("userName", m_UsernameIF.text);
            jobject.Add("password", m_PasswordIF.text);
            WebRequestController.Instance.Post(ApiCore.GetUrl(ApiCore.Url_Login), jobject.ToString(), new Dictionary<string, string>(), LoginHandle);
            Debug.Log(jobject.ToString());
        }
        public void Register()
        {
            JObject jobject = new JObject();
            jobject.Add("userName", m_UsernameRIF.text);
            jobject.Add("password", m_PasswordRIF.text);
            jobject.Add("phonenumber", m_PhonenumberRIF.text);
            jobject.Add("nickName", m_NickNameRIF.text);
            jobject.Add("code", m_CodeRIF.text);
            if (m_readtog.isOn)
            {
                Debug.Log(jobject.ToString());
                WebRequestController.Instance.Post(ApiCore.GetUrl(ApiCore.Url_Register), jobject.ToString(), new Dictionary<string, string>(), ResgisterHandle);
            }
            else
            {
                StartCoroutine(ShowInfo("����ȷ���û�Э��"));
            }
        }
        public void LoginHandle(string json)
        {
            print(json);
            JObject jobject = JsonConvert.DeserializeObject<JObject>(json);
            if (jobject["code"].ToString() == "200")
            {
                Global.token = jobject["token"].ToString();
                JObject jobject1 = new JObject();
                Dictionary<string, string> header = new Dictionary<string, string>();
                header.Add("Authorization", Global.token);
                WebRequestController.Instance.Post(ApiCore.GetUrl(ApiCore.Url_GetStudentInfoByToken), jobject1.ToString(), header, UserInfoHandle);
                //yzl��ʱ����
            }
            else
            {
                ResgisterHandle(jobject["msg"].ToString());
            }
        }
        public void UserInfoHandle(string result)
        {
            Debug.Log(result);
            JObject jobject = JsonConvert.DeserializeObject<JObject>(result);
            Global.UserName = jobject["data"]["userName"].ToString();
            Global.mobile = jobject["data"]["phonenumber"].ToString();
            Global.nickname = jobject["data"]["nickName"].ToString();
            Global.chatNo = jobject["data"]["chatNo"].ToString();
            Global.uid = (int)jobject["data"]["id"];
            Global.roleinfo = jobject["data"]["roleInfo"].ToString();
            // Start ��ʼ����������
            int.TryParse(jobject["data"]["avatar"].ToString(), out PersonalInfo.icon);
            Global.portrait=PersonalInfo.icon;
            PersonalInfo.nickName = Global.nickname;
            PersonalInfo.accountNumber = Global.mobile;
            int.TryParse(jobject["data"]["age"].ToString(), out PersonalInfo.age);
            PersonalInfo.mailbox = (string)jobject["data"]["mailbox"];
            PersonalInfo.job = (string)jobject["data"]["job"];
            PersonalInfo.workUnit = (string)jobject["data"]["workUnit"];
            PersonalInfo.educationsStr = (string)jobject["data"]["education"];
            // End 

            //YZL������ѯָ����
            string[] arguments = Environment.GetCommandLineArgs();
            if (arguments.Length >= 2)
            {
                //string[] arguments = { "", "yhijob://131/edit" };
                //�����Ǵ���ҳ��ȡ��
                if (arguments[1].Contains("yhijob"))
                {

                    Dictionary<string, string> headers = new Dictionary<string, string>();
                    string msTimestamp = WebRequestController.Instance.Timestamp.ToString();
                    headers.Add("MSTimestamp", msTimestamp);
                    string md5_str = WebRequestController.Instance.EncryMd5(msTimestamp, jobject.ToString().Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
                    //Debug.Log(md5_str);
                    headers.Add("MSSign", md5_str);
                    headers.Add("Authorization", Global.token);
                    JObject jobjects = new JObject();
                    jobjects["id"] = arguments[1].Split("/")[2].ToString();

                    Global.currentseneid = arguments[1].Split("/")[2].ToString();
                    if (arguments[1].Split("/")[3].ToString() == "edit")
                    {
                        Global.isCanEdit = true;
                    }
                    else
                    {
                        Global.isCanEdit = false;
                    }
                    SceneManager.LoadScene(1, LoadSceneMode.Single);

                    //jobjects["id"] = "94";
                    //������������ϸ��Ϣ
                    //WebRequestController.Instance.Post(ApiCore.GetUrl(ApiCore.Url_GetScenePackageInfo), jobjects.ToString(), headers, GetScenePackageInfoHandle);
                    return;
                }
            }
                SceneManager.LoadScene(1, LoadSceneMode.Single);
                Global.isCanEdit = false;
            //if (ScenName != "")
            //{
            //    Global.currentschoolname = "�Ϻ��ǽ�ְҵѧԺԪ����У԰";
            //    FindObjectOfType<Updater_Loader>().LoadItem(ScenName);
            //    return;
            //}
            //SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
        /// <summary>
        /// ��Ӧ����������ϸ��Ϣ
        /// </summary>
        /// <param name="result"></param>
        public void GetScenePackageInfoHandle(string result)
        {
            Debug.Log(result);
            JObject jobject = JsonConvert.DeserializeObject<JObject>(result);
            string mainscene = jobject["data"]["mainScene"].ToString();
            string path = jobject["data"]["ossParentPath"].ToString();
            string id = jobject["data"]["id"].ToString();
            FindObjectOfType<Updater_Loader>().LoadItem(path, id, mainscene);
        }

        public void ResgisterHandle(string result)
        {
            JObject jobject = JsonConvert.DeserializeObject<JObject>(result);

            StartCoroutine(ShowInfo(jobject["msg"].ToString()));
            if(jobject["code"].ToString()=="200")
            {
                FindObjectOfType<LoginMainWindow>().ShowStepIndex(1);
                GameObject.Find("UserLogin_tog").GetComponent<Toggle>().isOn = true;
            }
         }

        IEnumerator ShowInfo(string text)
        {
            m_InfoText.text = text;
            yield return new WaitForSeconds(3f);
            m_InfoText.text = "";
            yield break;
        }
        public void SMSLoginHandle(string json)
        {
            Debug.Log(json);
            JObject jobject = JsonConvert.DeserializeObject<JObject>(json);
            StartCoroutine(ShowInfo(jobject["msg"].ToString()));
            if (jobject["code"].ToString()=="200")
            {
                //Debug.Log("��ʼ����ť");
                StartCoroutine(LockBtn(smsbtn));
            }
        }
        public void SMSRegisterHandle(string json)
        {
            JObject jobject = JsonConvert.DeserializeObject<JObject>(json);
            StartCoroutine(ShowInfo(jobject["msg"].ToString()));
            if (jobject["code"].ToString() == "200")
            {
                StartCoroutine(LockBtn(smsrbtn));
            }
        }
        IEnumerator LockBtn(Button btn)
        {
            Text text= btn.gameObject.GetComponentInChildren<Text>();
            btn.enabled = false;
            int timer = 60;
            while (timer>0)
            {
                timer--;
                yield return new WaitForSeconds(1f);
                text.text = timer + "s";
            }
            text.text = "��ȡ��֤��";
            btn.enabled = true;
        }
    }
}