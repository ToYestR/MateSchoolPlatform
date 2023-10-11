using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FriendSystem
{
    public class ChatHistoryPrefabItem : MonoBehaviour
    {
        [SerializeField] Image icon;
        [SerializeField] Text nickName;
        [SerializeField] TextMeshProUGUI newMsg;   // 最新消息
        [SerializeField] Text count;
        [SerializeField] Text time;
        [SerializeField] Button openChatBtn;

        private Friend target;

        private void Start()
        {
            openChatBtn.onClick.AddListener(OnChatPanel);
        }
        string currentPage;
        public void Show(Friend friend,string page)
        {
            target = friend;
            currentPage = page;
            icon.sprite = HeadSculptureWindow.getInstance.GetSprite(target.friendPortrait);
            nickName.text = target.friendNickName;
        }
        private void OnEnable()
        {
            StartCoroutine(Refresh());
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
        private IEnumerator Refresh()
        {
            while(true)
            {
                yield return null;
                // 未读信息
                if (target.unReadCount > 0)
                {
                    count.transform.parent.gameObject.SetActive(true);
                    if (target.unReadCount > 99)
                    {
                        count.text = "99+";
                    }
                    else
                    {
                        count.text = target.unReadCount.ToString();
                    }
                }
                else
                {
                    count.transform.parent.gameObject.SetActive(false);
                }
                // 历史聊天
                if (target.chatMsgList.Count > 0)
                {
                    newMsg.text = target.chatMsgList[target.chatMsgList.Count - 1].msg;
                    time.text = target.chatMsgList[target.chatMsgList.Count - 1].time.ToString("T");
                }
                else
                {
                    newMsg.text = "";
                    time.text = "";
                }
                yield return new WaitForSecondsRealtime(GoodFriendManager.getInstance.requestTime);
            }
        }
        private void OnChatPanel()
        {
            PageManager.getInstance.OpenChatPanel(currentPage,target);
        }
    }
}
