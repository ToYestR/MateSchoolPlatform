using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FriendSystem
{
    /// <summary>
    /// 好友聊天窗口
    /// </summary>
    public class ChatPage : MonoBehaviour
    {
        [SerializeField] Button backBtn;
        [SerializeField] Button settingBtn;
        [SerializeField] TMP_InputField input;
        [SerializeField] Toggle emojiToggle;
        [SerializeField] Text nickName;
        [SerializeField] Text onlineState;
        [SerializeField] GameObject emojiWindow;
        [SerializeField] ScrollRect chatRect;
        [SerializeField] Transform content;

        GameObject meChatBybblePrefab;        // 我的 聊天 气泡
        GameObject heChatBubblePrefab;        // 她的 聊天 气泡
        List<ChatBubble> meChatBubbles = new List<ChatBubble>();        // 我的 聊天 气泡 池 
        List<ChatBubble> heChatBubbles = new List<ChatBubble>();        // 她的 聊天 气泡 池

        int indexMe;    // 我的 当前 气泡 池 序号
        int indexHe;    // 她的 当前 气泡 池 序号

        string oldPage;
        public Friend target;

        private void Awake()
        {
            heChatBubblePrefab = content.GetChild(0).gameObject;
            heChatBubblePrefab.gameObject.SetActive(false);
            meChatBybblePrefab = content.GetChild(1).gameObject;
            meChatBybblePrefab.gameObject.SetActive(false);
            input.onSubmit.AddListener(OnSubmitMsg);
            emojiToggle.onValueChanged.AddListener(OnToggleEmojiWIndow);
            backBtn.onClick.AddListener(Back);
            settingBtn.onClick.AddListener(OpenSetting);
        }

        private void Show((string, Friend) info)    // 显示当前页面
        {
            this.oldPage = info.Item1;
            this.target = info.Item2;
            nickName.text = target.friendNickName;
            onlineState.text = target.isOnLine ? "在线" : "离线";
            indexMe = 0;
            indexHe = 0;
            // 重置 池
            foreach (var item in meChatBubbles)
            {
                item.gameObject.SetActive(false);
            }
            foreach (var item in heChatBubbles)
            {
                item.gameObject.SetActive(false);
            }
            // 初始化 聊天
            for (int i = 0; i < target.chatMsgList.Count; i++)
            {
                ChatContent chatItem = target.chatMsgList[i];
                GenerateBubble(chatItem);
            }
            emojiToggle.isOn = false;
        }
        private void Back()                 // 回到之前页面
        {
            PageManager.getInstance.Back(oldPage);
        }
        private void OpenSetting()
        {
            PageManager.getInstance.Show(nameof(ChatPage), nameof(FriendSettingPage));
        }
        /// <summary>
        /// 生成 气泡
        /// </summary>
        /// <param name="chatItem"></param>
        private void GenerateBubble(ChatContent chatItem)
        {
            ChatBubble bubble = null;
            if (chatItem.isMe)
            {
                indexMe++;
                if (meChatBubbles.Count > indexMe)
                {
                    bubble = meChatBubbles[indexMe];
                }
                else
                {
                    bubble = Instantiate(meChatBybblePrefab, content).GetComponent<ChatBubble>();
                    meChatBubbles.Add(bubble);
                }

                bubble.transform.SetAsLastSibling();
                bubble.gameObject.SetActive(true);
                bubble.SetContent(chatItem.msg, Global.portrait, true, Global.nickname);
            }
            else
            {
                indexHe++;
                if (heChatBubbles.Count > indexHe)
                {
                    bubble = heChatBubbles[indexHe];
                }
                else
                {
                    bubble = Instantiate(heChatBubblePrefab, content).GetComponent<ChatBubble>();
                    heChatBubbles.Add(bubble);
                }

                bubble.transform.SetAsLastSibling();
                bubble.gameObject.SetActive(true);
                bubble.SetContent(chatItem.msg, target.friendPortrait, false, target.friendNickName);
            }
        }


        private void OnToggleEmojiWIndow(bool arg)
        {
            emojiWindow.SetActive(arg);
        }
        // 添加标签符号
        public void AddEmoji(int index)
        {
            input.text += $"<sprite={index}>";
            input.ActivateInputField();
        }
        // 收到聊天消息
        public void OnReceiveMsg(string msg)
        {
            target.unReadCount = 0;
            GenerateBubble(new ChatContent(false, msg));
            StartCoroutine(MoveDown());
        }
        // 发送聊天消息
        private void OnSubmitMsg(string msg)
        {
            if (string.IsNullOrWhiteSpace(msg)) return;
            input.text = "";
            emojiToggle.isOn = false;
            ChatContent content = new ChatContent(true, msg);
            target.chatMsgList.Add(content);
            GenerateBubble(content);
            // TODO 发送消息
            Debug.Log("发送:" + msg);
            StartCoroutine(MoveDown());
            FindObjectOfType<UnityWebSocketController>().SendChatMessage(target.friendChatNo, msg);
        }
        private IEnumerator MoveDown()
        {
            yield return null;
            chatRect.verticalNormalizedPosition = 0.0f;
        }
    }
}
