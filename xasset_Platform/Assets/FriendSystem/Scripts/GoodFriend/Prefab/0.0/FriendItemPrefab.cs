using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FriendSystem
{
    /// <summary>
    /// 好友 Item 类
    /// </summary>
    public class FriendItemPrefab : MonoBehaviour
    {
        [SerializeField] Image icon;            // 头像
        [SerializeField] Text nickName;         // 昵称
        [SerializeField] Text state;            // 在线状态
        [SerializeField] PopUpFriendWindow popUp;     // 弹出窗口


        public Friend friendContent { get; set; }

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnClick);
        }
        private void OnEnable()
        {
            StartCoroutine(Refresh());
        }
        private IEnumerator Refresh()
        {
            while (true)
            {
                if (friendContent != null)
                {
                    icon.sprite = HeadSculptureWindow.getInstance.GetSprite(friendContent.friendPortrait);
                    icon.SetNativeSize();
                    nickName.text = friendContent.friendNickName;
                    state.text = friendContent.isOnLine ? "在线" : "离线";
                }
                yield return null;
            }
        }
        private void OnClick()
        {

            popUp.ShowOperationWindow(friendContent);
        }
    }
}
