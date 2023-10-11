using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FriendSystem
{
    public class RequestFriendPanel : MonoBehaviour
    {
        [SerializeField] Button AddBtn;
        [SerializeField] Button CancleBtn;
        [SerializeField] Text content;
        [SerializeField] InputField input;


        public static RequestFriendPanel GetInstance;

        private void Awake()
        {
            GetInstance = this;
        }
        private void Start()
        {
            AddBtn.onClick.AddListener(OnAddClick);
            CancleBtn.onClick.AddListener(OnCancelClick);
        }

        string chatNo;
        string nickName;
        private void OnAddClick()
        {
            InfoHandle.ApplyFr(chatNo, input.text, OnResponse);
            ToggleEnable(false);
        }
        private void OnCancelClick()
        {
            ToggleEnable(false);
        }
        private void ToggleEnable(bool state)
        {
            transform.GetChild(0).gameObject.SetActive(state);
        }
        public void Show(string chatNo,string nickName)
        {
            this.chatNo = chatNo;
            this.nickName = nickName;
            content.text = $"添加{this.nickName}为好友？";
            ToggleEnable(true);
        }
        private void OnResponse(string json)
        {
            Debug.Log("申请好友响应："+json);
            ToggleEnable(false);
        }
    }
}
