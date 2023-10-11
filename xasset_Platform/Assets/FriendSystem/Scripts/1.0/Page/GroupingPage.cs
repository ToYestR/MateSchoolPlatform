using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace FriendSystem
{
    public class GroupingPage : PoolManager
    {
        [SerializeField] FriendSettingPage settingPage;
        [SerializeField] Button okBtn;
        [SerializeField] Button back;
        [SerializeField] Button newTagBtn;
        [SerializeField] Transform tip;
        [SerializeField] AddGroupPopup addGroup;

        private void Start()
        {
            newTagBtn.onClick.AddListener(AddGroupClick);
            okBtn.onClick.AddListener(OnOkClick);
            back.onClick.AddListener(Back);
        }
        private void OnEnable()
        {
            addGroup.gameObject.SetActive(false);
            tip.gameObject.SetActive(false);
            // 获取学生原先的 标签
            oldTag = settingPage.chatPage.target.tag;
            // 初始化标签列表
            foreach (var item in GoodFriendManager.getInstance.myTags)
            {
                var obj = pool.Get();
                obj.GetComponent<TagPrefabItem>().Show(item);
                if (oldTag != null && oldTag.name == item.name)
                {
                    obj.GetComponent<TagPrefabItem>().toggle.isOn = true;
                }
            }
        }
        private void OnDisable()
        {
            Clear();
        }
        private void AddGroupClick()
        {
            addGroup.Show(OnAddGroupCallBack);
        }
        MyTag oldTag;       // 原先的 标签
        private void OnOkClick()
        {
            TagPrefabItem[] tags = content.GetComponentsInChildren<TagPrefabItem>(false);
            TagPrefabItem item = tags.FirstOrDefault(item => item.toggle.isOn);
            if (oldTag != null)
            {
                if (item)
                {
                    if (item.target.name != oldTag.name)
                    {
                        // 删除标签
                        InfoHandle.TagDelStudent(oldTag.labelId, new List<int> { settingPage.chatPage.target.friendUserId });
                        // 更新标签
                        InfoHandle.TagAddStudent(item.target.labelId, new List<int> { settingPage.chatPage.target.friendUserId });
                        settingPage.chatPage.target.tag = item.target;
                    }
                }
                else
                {
                    // 删除原先标签
                    InfoHandle.TagDelStudent(oldTag.labelId, new List<int> { settingPage.chatPage.target.friendUserId });
                    settingPage.chatPage.target.tag = null;
                }
            }
            else
            {
                if (item)
                {
                    // 更新标签
                    InfoHandle.TagDelStudent(oldTag.labelId, new List<int> { settingPage.chatPage.target.friendUserId });
                    InfoHandle.TagAddStudent(item.target.labelId, new List<int> { settingPage.chatPage.target.friendUserId });
                    settingPage.chatPage.target.tag = item.target;
                }
            }
            StartCoroutine(TipAnimation());
        }
        private void Back()     // 回退
        {
            PageManager.getInstance.Back(nameof(FriendSettingPage));
        }
        private IEnumerator TipAnimation()
        {
            tip.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(2);
            tip.gameObject.SetActive(false);
        }
        private string newTagName;
        private void OnAddGroupCallBack(string tagName)
        {
            tagName = tagName.Replace(" ", "");
            newTagName = tagName;
            if (!GoodFriendManager.getInstance.myTags.Any(tag => tag.name == newTagName))
            {
                InfoHandle.AddTag(newTagName, OnEditTag);
            }
        }
        // 新建标签
        private void OnEditTag(string json)
        {
            Debug.Log("新建标签：" + json);
            JObject obj = (JObject)JsonConvert.DeserializeObject(json);
            string msg = (string)obj["msg"];
            int code = (int)obj["code"];
            if (code == 200)
            {
                // 获取所有标签列表
                InfoHandle.GetMyTags(json =>
                {
                    JObject obj = (JObject)JsonConvert.DeserializeObject(json);
                    string msg = (string)obj["msg"];
                    int code = (int)obj["code"];
                    var dataJson = obj["data"].ToString();
                    if (code == 200)
                    {
                        var tags = JsonConvert.DeserializeObject<List<MyTag>>(dataJson);
                        var mytag = tags.FirstOrDefault(item => item.name == newTagName);
                        if (mytag != null)
                        {
                            var go = Instantiate(prefab, content);
                            go.GetComponent<TagPrefabItem>().Show(mytag);
                        }
                    }
                });
            }
        }
    }
}
