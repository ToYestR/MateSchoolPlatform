using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FriendSystem
{
    /// <summary>
    /// 黑名单条目
    /// </summary>
    public class BlackFriendPrefabItem : MonoBehaviour
    {
        [SerializeField] Image icon;
        [SerializeField] Text nickName;
        [SerializeField] Toggle toggle;

        private void Start()
        {
            toggle.onValueChanged.AddListener(OnAddBlack);
        }


        private Friend target;
        private BlackFriendPage content;
        public void Show(Friend friend, BlackFriendPage panel)
        {
            target = friend;
            content = panel;

            icon.sprite = HeadSculptureWindow.getInstance.GetSprite(target.friendPortrait);
            nickName.text = target.friendNickName;
        }
        private void OnAddBlack(bool arg)
        {
            if (arg && target != null)
            {
                // 取消拉黑
                InfoHandle.BlackOther(target.id, "N", OnCancelBlack);
            }
        }
        private void OnCancelBlack(string json) // 取消拉黑回调
        {
            Debug.Log("取消拉黑回调：" + json);
            content.pool.Release(gameObject);
            content.ShowTip();
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
