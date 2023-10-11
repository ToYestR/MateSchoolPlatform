using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace FriendSystem
{
    /// <summary>
    /// 添加（修改） 标签 弹窗
    /// </summary>
    public class AddTagWindow : MonoBehaviour
    {
        [SerializeField] InputField tagTxT;
        [SerializeField] Button allSelectBtn;
        [SerializeField] Transform content;
        [SerializeField] Button completeBtn;
        [SerializeField] Button close;

        GameObject prefab;

        MyTag editTag;                                  // 要编辑的 标签 或 Null 则新建标签
        List<string> chatNos = new List<string>();      // 原始 标下 下 的 好友
        List<int> oldIds = new List<int>();             // 之前 没有 编辑 下的 好友
        private void Start()
        {
            prefab = content.GetChild(0).gameObject;
            prefab.SetActive(false);
            completeBtn.onClick.AddListener(OnCompelet);
            close.onClick.AddListener(OnClose);
            allSelectBtn.onClick.AddListener(OnSelectAllFriend);
        }
        private void ToggleEnable(bool state)
        {
            transform.GetChild(0).gameObject.SetActive(state);
        }

        /// <summary>
        /// 显示 窗口
        /// </summary>
        public void Show(MyTag tag, List<string> studentIds = null)          // TODO 添加 标签·字段 及其下的好友
        {
            ToggleEnable(true);
            chatNos = studentIds;
            this.editTag = tag;
            if (tag != null)
            {
                tagTxT.text = tag.name;
            }
            else
            {
                tagTxT.text = "";
            }
            oldIds.Clear();
            int index = 0;
            foreach (var item in GoodFriendManager.getInstance.friendList)
            {
                index++;
                AddTagFriendItemPrefab tagItem;
                if (content.childCount > index)
                {
                    tagItem = content.GetChild(index).GetComponent<AddTagFriendItemPrefab>();
                }
                else
                {
                    tagItem = Instantiate(prefab, content).GetComponent<AddTagFriendItemPrefab>();
                }
                tagItem.gameObject.SetActive(true);
                tagItem.Show(item.Value);
                if (chatNos != null && chatNos.Contains(item.Key))
                {
                    tagItem.Select();
                    oldIds.Add(item.Value.friendUserId);
                }
            }
        }

        // 点击 提交 按钮
        private void OnCompelet()
        {
            if (editTag!=null)       // 在原来标签下 操作
            {
                if(editTag.name!=tagTxT.text)       // 修改 标签名称
                {
                    InfoHandle.EditTag(editTag.labelId, Global.uid, tagTxT.text,json=>
                    {
                        Debug.Log("修改标签名称：" + json);

                    });
                }
                OnEditTag(editTag.labelId);
            }
            else
            {
                MyTag tag = GoodFriendManager.getInstance.myTags.FirstOrDefault(item => item.name == tagTxT.text);
                if (tag != null)            // 在已有的 标签下操作
                {
                    Debug.Log("标签已存在");
                    OnEditTag(tag.labelId);
                }   
                else                        // 新建标签
                {
                    Debug.Log("标签 不存在");
                    if (!string.IsNullOrWhiteSpace(tagTxT.text))
                    {
                        InfoHandle.AddTag(tagTxT.text, OnEditTag);
                    }
                }
            }
        }
        // 关闭 按钮
        private void OnClose()
        {
            ToggleEnable(false);
        }
        // 选择 所有 按钮
        private void OnSelectAllFriend()
        {
            foreach (var item in content.GetComponentsInChildren<AddTagFriendItemPrefab>())
            {
                item.Select();
            }
        }
        // 新建标签 添加 学生ID
        private void OnEditTag(string json)
        {
            Debug.Log("给标签添加学生：" + json);
            JObject obj = (JObject)JsonConvert.DeserializeObject(json);
            string msg = (string)obj["msg"];
            int code = (int)obj["code"];
            var list = GetSelecStudentIDs();
            if (list.Count > 0 && code == 200)
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
                        var mytag=  tags.FirstOrDefault(item => item.name == tagTxT.text);
                        if(mytag!=null)
                        {
                            InfoHandle.TagAddStudent(mytag.labelId, list);
                        }
                    }
                });
            }
        }
        // 更新标签 学生 ID
        private void OnEditTag(int tagId)
        {
            var list = GetStudentIDs();
            InfoHandle.TagAddStudent(tagId, list.Item1);
            InfoHandle.TagDelStudent(tagId, list.Item2);
        }
        /// <summary>
        /// 获取 选中的 学生 ID列表
        /// </summary>
        private List<int> GetSelecStudentIDs()
        {
            var items = content.GetComponentsInChildren<AddTagFriendItemPrefab>();
            List<int> studentIds = new List<int>();
            foreach (var item in items)
            {
                if (item.toggle.isOn)
                {
                    studentIds.Add(item.friend.friendUserId);
                }
            }
            return studentIds;
        }
        /// <summary>
        /// 获取 选择 和 取消 的 学生 ID;
        /// </summary>
        /// <returns></returns>
        private (List<int>, List<int>) GetStudentIDs()
        {
            List<int> selectIds = GetSelecStudentIDs();     // 已经选择 的 学生 ID
            List<int> addIds = new List<int>();             // 要 新增的 学生 ID
            List<int> delIds = new List<int>();             // 要 删除的 学生 ID
            if (oldIds != null)
            {
                foreach (var id in selectIds)
                {
                    if (!oldIds.Contains(id))
                    {
                        addIds.Add(id);
                    }
                }
                foreach (var id in oldIds)
                {
                    if (!selectIds.Contains(id))
                    {
                        delIds.Add(id);
                    }
                }
            }
            return (addIds, delIds);
        }
    }
}
