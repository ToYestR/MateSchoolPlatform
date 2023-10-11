using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FriendSystem
{
    /// <summary>
    /// 分组编辑面板
    /// </summary>
    public class GroupEditListPanel : MonoBehaviour
    {
        [SerializeField] Button addGroupBtn;
        [SerializeField] Button okbtn;
        [SerializeField] AddGroupPopup newGroupPopup;
        [SerializeField] Transform tip;
        [SerializeField] Transform content;
        [SerializeField] GameObject prefab;

        private void Awake()
        {
            okbtn.onClick.AddListener(OnOkClick);
            addGroupBtn.onClick.AddListener(AddGroupClick);
        }
        private void OnEnable()
        {
            foreach (var item in GoodFriendManager.getInstance.myTags)
            {
                var obj = Instantiate(prefab, content);
                obj.GetComponent<GroupEditPrefabItem>().Show(item);
            }
            tip.gameObject.SetActive(false);
            newGroupPopup.gameObject.SetActive(false);
        }
        private void OnDisable()
        {
            int count = content.childCount;
            for(int i=0;i<count;i++)
            {
                DestroyImmediate(content.GetChild(0).gameObject);
            }
        }
        private void OnOkClick()
        {
            GroupEditPrefabItem[] items= content.GetComponentsInChildren<GroupEditPrefabItem>(false);
            // 标签排序
            for(int i=0;i<items.Length;i++)
            {
                GoodFriendManager.getInstance.myTags[i] = items[i].target;
            }
            StartCoroutine(TipAnimation());
        }
        private IEnumerator TipAnimation()
        {
            tip.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(2);
            tip.gameObject.SetActive(false);
        }
        private void AddGroupClick()
        {
            newGroupPopup.Show(OnAddGroupCallBack);
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
            Debug.Log("给标签添加学生：" + json);
            JObject obj = (JObject)JsonConvert.DeserializeObject(json);
            string msg = (string)obj["msg"];
            int code = (int)obj["code"];
            if (code == 200)
            {
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
                            go.GetComponent<GroupEditPrefabItem>().Show(mytag);
                        }
                    }
                });
            }
        }
    }
}
