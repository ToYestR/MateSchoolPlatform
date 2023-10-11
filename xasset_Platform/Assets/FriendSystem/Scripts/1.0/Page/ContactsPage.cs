using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FriendSystem
{
    /// <summary>
    /// 联系人页面
    /// </summary>
    public class ContactsPage : MonoBehaviour
    {
        [Header("搜索")]
        [SerializeField] TMP_InputField search;
        [Header("新朋友")]
        [SerializeField] Text newFriendCountTxt;
        [SerializeField] Button openApplyBtn;
        [Header("开关")]
        [SerializeField] Toggle friendToggle;
        [SerializeField] Toggle groupToggle;
        [SerializeField] Toggle groupEditToggle;
        [SerializeField] GameObject[] panels;   // 切换显示好友——分组——分组编辑

        private void Awake()
        {
            friendToggle.onValueChanged.AddListener(arg =>
            {
                OnTogglePanel(0);
            });
            groupToggle.onValueChanged.AddListener(arg =>
            {
                OnTogglePanel(1);
            });
            groupEditToggle.onValueChanged.AddListener(arg =>
            {
                OnTogglePanel(2);
            });
            openApplyBtn.onClick.AddListener(OnOpenApplyPanel);
        }
        private void OnOpenApplyPanel()
        {
            PageManager.getInstance.Show(nameof(ContactsPage), nameof(ApplyFriendPage));
        }
        private IEnumerator Refresh()
        {
            while (true)
            {
                {   // 红点
                    if (GoodFriendManager.getInstance.applyList.Count > 0)
                    {
                        newFriendCountTxt.transform.parent.gameObject.SetActive(true);
                        if (GoodFriendManager.getInstance.applyList.Count > 99)
                        {
                            newFriendCountTxt.text = "99";
                        }
                        else
                        {
                            newFriendCountTxt.text = GoodFriendManager.getInstance.applyList.Count.ToString();
                        }
                    }
                    else
                    {
                        newFriendCountTxt.transform.parent.gameObject.SetActive(false);
                    }
                }
                yield return new WaitForSecondsRealtime(GoodFriendManager.getInstance.requestTime);
                yield return null;
            }
        }
        private void OnEnable()
        {
            StartCoroutine(Refresh());
            if (friendToggle.isOn)
            {
                OnTogglePanel(0);
            }
            else if (groupToggle.isOn)
            {
                OnTogglePanel(1);
            }
            else
            {
                OnTogglePanel(2);
            }
        }
        private void OnTogglePanel(int index)
        {
            for (int i = 0; i < panels.Length; i++)
            {
                if (index == i)
                {
                    panels[i].gameObject.SetActive(true);
                }
                else
                {
                    panels[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
