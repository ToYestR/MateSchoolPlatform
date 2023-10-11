using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FriendSystem
{
    public class ChatsWindow : MonoBehaviour
    {
        [SerializeField] Button closeBtn;
        [SerializeField] Transform content;     // 好友 条目 父节点

        Dictionary<string, FriendChatItemPrefab> chats = new Dictionary<string, FriendChatItemPrefab>();    // 微聊号，聊天 条目

        GameObject prefab;

        private void Awake()
        {
            prefab = content.GetChild(0).gameObject;
            prefab.gameObject.SetActive(false);
            ToggleEnable(false);
        }
        private void Start()
        {
            closeBtn.onClick.AddListener(() => ToggleEnable(false));
        }
        private void OnDisable()
        {
            ToggleEnable(false);
        }
        private void ToggleEnable(bool state)
        {
            transform.GetChild(0).gameObject.SetActive(state);
        }

        /// <summary>
        /// 显示聊天窗口 添加 聊天-》打开 聊天 面板
        /// </summary>
        public void Show(Friend friend)
        {
            ToggleEnable(true);
            List<string> oldChatNo = new List<string>();
            // 移除 不是 好友 的 聊天 条目
            foreach(var item in chats)
            {
                if(!GoodFriendManager.getInstance.friendList.ContainsKey(item.Key))
                {
                    oldChatNo.Add(item.Key);
                }
            }
            foreach(var key in oldChatNo)
            {
                FriendChatItemPrefab item = chats[key];
                item.gameObject.SetActive(false);
                item.transform.SetAsLastSibling();
                chats.Remove(key);
            }
            if (chats.ContainsKey(friend.friendChatNo))
            {
                chats[friend.friendChatNo].Show(friend);
            }
            else
            {
                // 添加 聊天
                FriendChatItemPrefab chat;
                int index = chats.Count;
                if (content.childCount > index + 1)
                {
                    chat = content.GetChild(index+1).GetComponent<FriendChatItemPrefab>();
                }
                else
                {
                    chat = Instantiate(prefab, content).GetComponent<FriendChatItemPrefab>();
                }
                chat.transform.SetSiblingIndex(index + 1);
                chats.Add(friend.friendChatNo, chat);
                chat.Show(friend);
            }
        }
    }
}
