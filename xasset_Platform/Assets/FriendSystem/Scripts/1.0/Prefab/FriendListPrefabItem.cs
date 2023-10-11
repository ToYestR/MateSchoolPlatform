using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FriendSystem
{
    public class FriendListPrefabItem : MonoBehaviour
    {
        [SerializeField] Image icon;
        [SerializeField] Text nickName;
        [SerializeField] Text onlineState;
        [SerializeField] Button openChatBtn;
        private Friend target;
        string currentPage;
        private void Start()
        {
            openChatBtn.onClick.AddListener(OnOpenChatPanel);   
        }
        public void Show(Friend friend,string page)
        {
            target = friend;
            currentPage = page;
            icon.sprite = HeadSculptureWindow.getInstance.GetSprite(target.friendPortrait);
            nickName.text = target.friendNickName;
            onlineState.text = "【"+(target.isOnLine?"在线":"离线")+"】";
        }
        private void OnOpenChatPanel()  // 打开聊天面板
        {
            PageManager.getInstance.OpenChatPanel(nameof(ContactsPage), target);
        }
    }
}
