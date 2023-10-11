using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FriendSystem
{
    public class AddFriendPrefabItem : MonoBehaviour
    {
        [SerializeField] Image icon;
        [SerializeField] Text nickName;
        [SerializeField] Button addBtn;
        [SerializeField] Text age;
        [SerializeField] Text sex;

        private Personnel target;
        private AddFriendPage page;
        private void Start()
        {
            addBtn.onClick.AddListener(OnAddClick);
        }

        public void Show(Personnel friend, AddFriendPage page)
        {
            target = friend;
            this.page = page;
            nickName.text = target.nickName;
            sex.text=target.sex==0?"男":"女";
            if(GoodFriendManager.getInstance.friendList.ContainsKey(target.chatNo))
            {
                addBtn.interactable = false;
                addBtn.transform.GetChild(0).GetComponent<Text>().text = "已添加";
            }
            else
            {
                addBtn.interactable = true;
                addBtn.transform.GetChild(0).GetComponent<Text>().text = "添加";
            }
        }
        private void OnAddClick()
        {
            InfoHandle.ApplyFr(target.chatNo, "你好，加个好友聊聊天吧!",null);
            page.pool.Release(gameObject);
            page.ShowTip();
        }
    }
}
