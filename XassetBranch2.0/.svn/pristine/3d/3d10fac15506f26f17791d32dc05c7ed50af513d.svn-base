using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FriendSystem
{
    /// <summary>
    /// 好友弹出窗口
    /// </summary>
    public class PopUpFriendWindow : MonoBehaviour
    {
        [SerializeField] OperationWindow operation;     // 好友 操作弹窗
        [SerializeField] DeleteFriendWindow delete; // 删除 确认弹窗
        [SerializeField] BlackListWindow black; // 黑名单 窗口
        [SerializeField] ChatsWindow chat;      // 聊天窗口
        // 好友 操作 窗口
        public void ShowOperationWindow(Friend friend)
        {
            operation.Show(friend);
        }
        // 删除 窗口
        public void ShowDeleWindow(Friend friend)
        {
            delete.Show(friend);
        }
        // 黑名单 窗口
        public void ShowBlackWindow()
        {
            black.Show();
        }
        // 打开 聊天 面板
        public void ShowOpenChat(Friend friend)
        {
            chat.Show(friend);
        }
    }
}
