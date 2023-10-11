using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace FriendSystem
{
    /// <summary>
    /// 申请列表 条目
    /// </summary>
    public class ApplyFriendItemPrefab : MonoBehaviour
    {
        [SerializeField] PopUpApplyWindow popUp;

        [SerializeField] Button refuseBtn;      // 拒绝
        [SerializeField] Button agreeBtn;       // 同意

        [SerializeField] Text nickName;
        [SerializeField] Image icon;

        ApplyFriend apply;


        private void Start()
        {
            refuseBtn.onClick.AddListener(OnRefuseClick);
            agreeBtn.onClick.AddListener(OnAgreeClick);
        }
        private void OnDisable()
        {
            gameObject.SetActive(false);
        }
        public void Show(ApplyFriend apply)
        {
            this.apply = apply;
            icon.sprite = HeadSculptureWindow.getInstance.GetSprite(0);
            nickName.text = apply.fromNickName;
        }
        private void OnRefuseClick()
        {
            InfoHandle.AuditFr(apply.id, "2");
            OnDisable();
        }
        private void OnAgreeClick()
        {
            //InfoHandle.AuditFr(apply.id, "1");
            popUp.ShowApplyWindow(false);
            popUp.ShowApplyVerify(apply);
            OnDisable();
        }
    }
}
