using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

namespace FriendSystem
{
    /// <summary>
    /// 个人中心 类
    /// </summary>
    public class PersonalCenter : MonoBehaviour
    {
        public static PersonalCenter getInstance;
        #region 属性
        [Header("输入")]
        [SerializeField] InputField nickName;       // 昵称
        [SerializeField] InputField accountNumber;  // 账号
        [SerializeField] InputField age;            // 年龄
        [SerializeField] Dropdown sex;              // 性别
        [SerializeField] InputField mailbox;        // 邮箱
        [SerializeField] Dropdown job;              // 职业
        [SerializeField] InputField workUnit;       // 工作单位

        [Header("按钮")]
        [SerializeField] Button updateIcon;         // 上传头像
        [SerializeField] Button addItem;            // 教育历程添加 按钮
        [SerializeField] Button saveBtn;            // 保存 按钮
        [SerializeField] Button cancelBtn;           // 取消 按钮
        [SerializeField] Button closeBtn;           // 关闭按钮

        [Header("教育历程")]
        [SerializeField] Transform content;         // 教育历程条目父节点
        #endregion

        [Header("图片")]
        [SerializeField] Image iconMan;     // 男生头像
        [SerializeField] Image iconWoman;   // 女生头像
        [SerializeField] ScrollRect EducationView;

        private void Awake()
        {
            getInstance = this;
        }
        private void Start()
        {
            Init();
            EventInit();
        }
        public void Init()
        {
            // 字段
            if(PersonalInfo.icon<0)
            {
                iconMan.transform.parent.parent.gameObject.SetActive(true);
            }
            else
            {
                iconWoman.sprite = HeadSculptureWindow.getInstance.GetSprite(PersonalInfo.icon);
                iconMan.transform.parent.parent.gameObject.SetActive(false);
            }
            nickName.text = PersonalInfo.nickName;
            accountNumber.text = PersonalInfo.accountNumber;
            age.text = PersonalInfo.age.ToString();
            sex.value = PersonalInfo.sex;
            mailbox.text = PersonalInfo.mailbox;
            job.captionText.text = PersonalInfo.job;
            workUnit.text = PersonalInfo.workUnit;

            // 教育
            EducationItem[] items = content.GetComponentsInChildren<EducationItem>();
            for (int i = 0; i < items.Length; i++)
            {
                items[i].Delete();
            }
            GameObject prefabItem = content.GetChild(0).gameObject;
            if (PersonalInfo.educations.Count > 0)
            {
                for (int i = content.childCount; i < PersonalInfo.educations.Count; i++)
                {
                    Instantiate(prefabItem, content);
                }
                items = content.GetComponentsInChildren<EducationItem>();
                for (int i = 0; i < PersonalInfo.educations.Count; i++)
                {
                    items[i].UpdateEducation(PersonalInfo.educations[i]);
                }
            }
            // 按学历排序
            List<string> levels = new List<string> { "博士后", "博士", "研究生", "本科", "大专", "中专", "高中", "初中", "小学", "" };
            Dictionary<int, List<GameObject>> sort = new Dictionary<int, List<GameObject>>();
            foreach (var item in items)
            {
                string level = item.info.level;
                int index = levels.IndexOf(level);
                if (!sort.ContainsKey(index))
                {
                    sort.Add(index, new List<GameObject>());
                }
                sort[index].Add(item.gameObject);
            }
            for (int i = 0; i < levels.Count; i++)
            {
                if (sort.ContainsKey(i))
                {
                    foreach (var item in sort[i])
                    {
                        item.transform.SetAsLastSibling();
                    }
                }
            }
        }

        private void EventInit()
        {
            /*********************************************************************/
            updateIcon.onClick.AddListener(() =>
            {
                HeadSculptureWindow.getInstance.OpenHeadSculptureWindow((sprite, id) =>
                {
                    iconWoman.sprite = sprite;
                    iconMan.transform.parent.parent.gameObject.SetActive(false);
                    PersonalInfo.icon = id;
                    Global.portrait = PersonalInfo.icon;
                });
            });
            /*********************************************************************/
            addItem.onClick.AddListener(async () =>
            {
                GameObject prefabItem = content.GetChild(0).gameObject;
                Instantiate(prefabItem, content);
                await Task.Delay(100);
                EducationView.verticalNormalizedPosition = -1;
            });
            cancelBtn.onClick.AddListener(() =>
            {
                Init();
            });
            closeBtn.onClick.AddListener(() =>
            {
                ToggleWindow(false);
            });
            saveBtn.onClick.AddListener(() =>
            {
                // 参数保存
                PersonalInfo.nickName = nickName.text;
                Global.nickname = PersonalInfo.nickName;
                PersonalInfo.accountNumber = accountNumber.text;
                int.TryParse(age.text, out PersonalInfo.age);
                PersonalInfo.sex = sex.value;
                PersonalInfo.mailbox = mailbox.text;
                PersonalInfo.job = job.captionText.text;
                PersonalInfo.workUnit = workUnit.text;



                // 序列化教育历程
                EducationItem[] items = content.GetComponentsInChildren<EducationItem>();
                List<EducationInfo> infos = new List<EducationInfo>();
                foreach (var item in items)
                {
                    if (item.info != null)
                    {
                        infos.Add(item.info);
                    }
                }
                string dataStr = JsonConvert.SerializeObject(infos);
                PersonalInfo.educationsStr = dataStr.Replace("\"", "'");
                //if(PersonalInfo.educationsStr.Length>255)
                //{
                //    Debug.LogError("教育历程超过255个字符，上传失败");
                //    return;
                //}
                JObject jobect = new JObject();
                jobect.Add("id", Global.uid);
                jobect.Add("user_name", PersonalInfo.accountNumber);
                jobect.Add("nick_name", PersonalInfo.nickName);
                jobect.Add("age", PersonalInfo.age);
                jobect.Add("sex", PersonalInfo.sex);
                jobect.Add("mailbox", PersonalInfo.mailbox);
                jobect.Add("job", PersonalInfo.job);
                jobect.Add("workUnit", PersonalInfo.workUnit);
                jobect.Add("avatar", PersonalInfo.icon);
                jobect.Add("education", PersonalInfo.educationsStr);
                Debug.Log(jobect.ToString());
                InfoHandle.EditStudentInfo(jobect.ToString(), OnSubmitReceive);

                ToggleWindow(false);
            });
        }
        private void ToggleWindow(bool toggle)
        {
            transform.GetChild(0).gameObject.SetActive(toggle);
        }
        private void OnSubmitReceive(string json)
        {
            Debug.Log("提交个人信息回应：" + json);
        }
        public void Show()
        {
            ToggleWindow(true);
        }
        public void Close()
        {
            ToggleWindow(false);
        }
    }
}