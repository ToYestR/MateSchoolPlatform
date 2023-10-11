using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace FriendSystem
{
    public class PageManager : MonoBehaviour
    {
        [SerializeField] GameObject[] pages;
        public static PageManager getInstance;
        private void Awake()
        {
            getInstance = this;
        }
        public void Show(string currentPage, string showPageName)   // 切换显示页面
        {
            foreach (var obj in pages)
            {
                if (obj.name == showPageName)
                {
                    obj.SetActive(true);
                    obj.SendMessage("Show", currentPage, SendMessageOptions.DontRequireReceiver);
                }
                else
                {
                    obj.SetActive(false);
                }
            }
        }
        public void OpenChatPanel(string currentPage, Friend friend)        // 打开聊天面板
        {
            string showPageName = nameof(ChatPage);
            foreach (var obj in pages)
            {
                if (obj.name == showPageName)
                {
                    obj.SetActive(true);
                    obj.SendMessage("Show", (currentPage, friend), SendMessageOptions.DontRequireReceiver);
                }
                else
                {
                    obj.SetActive(false);
                }
            }
        }
        public void Back(string showPageName)   // 回滚
        {
            foreach (var obj in pages)
            {
                if (obj.name == showPageName)
                {
                    obj.SetActive(true);
                }
                else
                {
                    obj.SetActive(false);
                }
            }
        }
    }
}
