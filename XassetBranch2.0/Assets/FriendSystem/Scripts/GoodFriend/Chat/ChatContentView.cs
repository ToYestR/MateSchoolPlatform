using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FriendSystem
{
    /// <summary>
    /// 聊天 主 窗口
    /// </summary>
    public class ChatContentView : MonoBehaviour
    {
        [SerializeField] Text nickName;
        [SerializeField] Button sendBtn;
        [SerializeField] Toggle emojiBtn;
        [SerializeField] TMP_InputField input;
        [SerializeField] ScrollRect chatRect;
        [SerializeField] Transform content;
        [SerializeField] GameObject emojiWindow;

        GameObject meChatBybblePrefab;        // 我的 聊天 气泡
        GameObject heChatBubblePrefab;        // 她的 聊天 气泡

        public Friend friendContent;

        List<ChatBubble> meChatBubbles = new List<ChatBubble>();        // 我的 聊天 气泡 池 
        List<ChatBubble> heChatBubbles = new List<ChatBubble>();        // 她的 聊天 气泡 池

        int indexMe;    // 我的 当前 气泡 池 序号
        int indexHe;    // 她的 当前 气泡 池 序号

        private void Awake()
        {
            heChatBubblePrefab = content.GetChild(0).gameObject;
            heChatBubblePrefab.gameObject.SetActive(false);
            meChatBybblePrefab = content.GetChild(1).gameObject;
            meChatBybblePrefab.gameObject.SetActive(false);
        }
        private void Start()
        {
            emojiBtn.onValueChanged.AddListener(OnToggleEmojiWIndow);
            input.onSubmit.AddListener(OnSubmitMsg);
            sendBtn.onClick.AddListener(() => OnSubmitMsg(input.text));
            emojiBtn.isOn = false;
        }

        public void Open(Friend friend)
        {
            this.friendContent = friend;
            nickName.text = friend.friendNickName;
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
            for (int i = 0; i < friend.chatMsgList.Count; i++)
            {
                ChatContent chatItem = friend.chatMsgList[i];
                GenerateBubble(chatItem);
            }
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
                bubble.SetContent(chatItem.msg, Global.portrait, true,Global.nickname);
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
                bubble.SetContent(chatItem.msg, friendContent.friendPortrait, false,friendContent.friendNickName);
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
        }
        // 收到聊天消息
        public void OnReceiveMsg(string msg)
        {
            GenerateBubble(new ChatContent(false, msg));
            StartCoroutine(MoveDown());
        }
        // 发送聊天消息
        private void OnSubmitMsg(string msg)
        {
            if (string.IsNullOrWhiteSpace(msg)) return;
            input.text = "";
            emojiBtn.isOn = false;
            ChatContent content = new ChatContent(true, msg);
            friendContent.chatMsgList.Add(content);
            GenerateBubble(content);
            // TODO 发送消息
            Debug.Log("发送:" + msg);
            StartCoroutine(MoveDown());
            FindObjectOfType<UnityWebSocketController>().SendChatMessage(friendContent.friendChatNo, msg);
        }
        private IEnumerator MoveDown()
        {
            yield return null;
            chatRect.verticalNormalizedPosition = 0.0f;
        }
    }
}
