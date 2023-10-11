using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FriendSystem
{
    public class RoleItemPrefab : MonoBehaviour
    {
        [SerializeField] Button addBtn;
        [SerializeField] Image icon;
        [SerializeField] Text nickName;
        [SerializeField] Text state;

        Personnel people;
        private void Start()
        {
            addBtn.onClick.AddListener(OnAddClick);
        }
        public void Show(Personnel people)
        {
            this.people = people;
            gameObject.SetActive(true);
            nickName.text = people.nickName;
            icon.sprite = HeadSculptureWindow.getInstance.GetSprite(people.portrait);
            if(Global.uid==people.id)
            {
                addBtn.interactable = false;
            }
            else
            {
                addBtn.interactable = true;
            }
        }
        private void OnAddClick()
        {
            // 添加 好友 申请
            InfoHandle.ApplyFr(people.chatNo,"",(json)=>
            {
                Debug.Log("申请好友：" + json);
            });
        }
    }
}
