using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FriendSystem
{
    [DefaultExecutionOrder(-10)]
    public class ChatHistoryPage : PoolManager
    {
        [SerializeField] Button addFriendBtn;
        [SerializeField] TMP_InputField search;

        public static ChatHistoryPage getInstance;
        protected override void Awake()
        {
            getInstance = this;
            base.Awake();
            search.onValueChanged.AddListener(OnSearchChange);
            addFriendBtn.onClick.AddListener(OnAddFriend);
        }


        private void OnEnable()
        {
            foreach (var item in GoodFriendManager.getInstance.friendList)
            {
                var obj = pool.Get();
                obj.GetComponent<ChatHistoryPrefabItem>().Show(item.Value,nameof(ChatHistoryPage));
            }
        }
        private void OnDisable()
        {
            Clear();
        }
        private void OnSearchChange(string arg0)    // 搜索
        {
            foreach (var obj in busyObjects)
            {
                obj.GetComponent<ChatHistoryPrefabItem>().SearchMe(arg0);
            }
            search.text = "";
        }
        private void OnAddFriend()      // 打开添加好友面板
        {
            PageManager.getInstance.Show(nameof(ChatHistoryPage), nameof(AddFriendPage));
        }
    }
}
