using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FriendSystem
{
    /// <summary>
    /// 添加 好友标签列表
    /// </summary>
    public class AddTagFriendItemPrefab : MonoBehaviour
    {
        [SerializeField] Image icon;
        [SerializeField] Text nickName;

        public Toggle toggle;
        public Friend friend;

        private void OnEnable()
        {
            toggle.isOn = false;
        }
        private void OnDisable()
        {
            gameObject.SetActive(false);
        }

        public void Show(Friend friend)
        {
            this.friend = friend;
            icon.sprite = HeadSculptureWindow.getInstance.GetSprite(friend.friendPortrait);
            nickName.text = friend.friendNickName;
            gameObject.SetActive(true);
        }
        public void Select()
        {
            toggle.isOn = true;
        }
    }
}
