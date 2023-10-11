using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FriendSystem
{
    /// <summary>
    /// 好友权限设置
    /// </summary>
    public class FriendLimitPrefabItem : MonoBehaviour
    {
        [SerializeField] Image icon;
        [SerializeField] Text nickName;
        [SerializeField] Toggle toggle;

        private Friend target;
        public void Show(Friend friend)
        {
            target = friend;
            icon.sprite = HeadSculptureWindow.getInstance.GetSprite(target.friendPortrait);
            nickName.text = target.friendNickName;
            toggle.isOn = friend.roomLimit;
        }
        public void SetToggle(bool arg)
        {
            toggle.isOn = arg;
        }
        public void Save()
        {
            if (target != null) target.roomLimit = toggle.isOn;
        }
        public void SearchMe(string nickName)
        {
            if (string.IsNullOrWhiteSpace(nickName))
            {
                gameObject.SetActive(true);
            }
            else if (target.friendNickName.Contains(nickName))
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
