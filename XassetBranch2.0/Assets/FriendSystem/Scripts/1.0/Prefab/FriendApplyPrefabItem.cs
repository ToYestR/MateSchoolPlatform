using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FriendSystem
{
    /// <summary>
    /// 好友申请条目
    /// </summary>
    public class FriendApplyPrefabItem : MonoBehaviour
    {
        [SerializeField] Image icon;
        [SerializeField] Text nickName;
        [SerializeField] Button refuseBtn;
        [SerializeField] Button agreeBtn;
        [SerializeField] Text age;
        [SerializeField] Text sex;
        private ApplyFriend target;
        private ApplyFriendPage page;
        private void Start()
        {
            refuseBtn.onClick.AddListener(OnRefuseClick);
            agreeBtn.onClick.AddListener(OnAgreeClick);
        }

        public void Show(ApplyFriend friend, ApplyFriendPage page)
        {
            target = friend;
            this.page = page;
            nickName.text = target.fromNickName;
        }
        private void OnRefuseClick()
        {
            InfoHandle.AuditFr(target.id, "2", OnResponse);
        }
        private void OnAgreeClick()
        {
            InfoHandle.AuditFr(target.id, "1", OnResponse);
        }
        private void OnResponse(string json)
        {
            Debug.Log("申请好友操作：" + json);
            page.pool.Release(gameObject);
            page.ShowTip();
        }
    }
}
