using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FriendSystem
{
    /// <summary>
    /// 教育历程条目
    /// </summary>
    public class EducationItem : MonoBehaviour
    {
        [SerializeField] Text tip;
        [SerializeField] Button edit;
        [SerializeField] Text school;
        [SerializeField] Text department;   // 院系
        [SerializeField] Text inSchoolTime; // 入学时间
        [SerializeField] Text education;    // 学历
        [Header("编辑窗口")]
        [SerializeField] AddSchool editWindow;  // 编辑窗口
        public EducationInfo info;
        // Start is called before the first frame update
        void Start()
        {
            UpdateEducation(info);
            edit.onClick.AddListener(() =>
            {
                editWindow.OpenEdit(this);
            });
        }
        private void Init()
        {
            school.transform.GetChild(0).GetComponent<Text>().text = info.school;
            department.transform.GetChild(0).GetComponent<Text>().text = info.serie;
            inSchoolTime.transform.GetChild(0).GetComponent<Text>().text = info.time;
            education.transform.GetChild(0).GetComponent<Text>().text = info.level;
        }
        private void ToggleTip(bool isShowTip)
        {
            tip.gameObject.SetActive(isShowTip);
            school.gameObject.SetActive(!isShowTip);
            department.gameObject.SetActive(!isShowTip);
            inSchoolTime.gameObject.SetActive(!isShowTip);
            education.gameObject.SetActive(!isShowTip);
        }
        /// <summary>
        /// 更新教育信息
        /// </summary>
        /// <param name="info"></param>
        public void UpdateEducation(EducationInfo info)
        {
            this.info = info;
            switch (transform.GetSiblingIndex())
            {
                case 0:
                    tip.text = "添加大学";
                    break;
                case 1:
                    tip.text = "添加中学";
                    break;
                case 2:
                    tip.text = "添加小学";
                    break;
                default:
                    tip.text = "添加学校";
                    break;
            }
            if (info == null)
            {
                ToggleTip(true);
            }
            else
            {
                ToggleTip(false);
                Init();
            }
        }
        /// <summary>
        /// 删除 教育节点
        /// </summary>
        public void Delete()
        {
            info = null;
            if (transform.parent.childCount > 3)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                UpdateEducation(info);
            }
        }
    }
}