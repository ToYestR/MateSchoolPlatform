using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace FriendSystem 
{
    public class FriendApplyVerifyWindow : MonoBehaviour
    {
        [SerializeField] Button closeBtn;           // 关闭按钮
        [SerializeField] InputField markTxt;        // 备注
        [SerializeField] InputField tagTxt;         // 标签
        [SerializeField] Toggle visitRoom;          // 是否 允许访问房间
        [SerializeField] Button okBtn;              // 确定 按钮
                                                    // Start is called before the first frame update

        ApplyFriend apply;
        private void Start()
        {
            closeBtn.onClick.AddListener(OnCloseClick);
            okBtn.onClick.AddListener(OnOkClick);
        }
        private void ToggleEnable(bool state)
        {
            transform.GetChild(0).gameObject.SetActive(state);
        }
        public void Show(ApplyFriend apply)
        {
            this.apply = apply;
            ToggleEnable(true);
            markTxt.text = "";
            tagTxt.text = "";
        }

        private void OnCloseClick()
        {
            ToggleEnable(false);
        }
        private void OnOkClick()
        {
            ToggleEnable(false);
            InfoHandle.AuditFr(apply.id, "1", OnResponse);
        }
        private void OnResponse(string json)
        {
            ToggleEnable(false);
            Debug.Log("添加好友成功：" + json);
            // 更新备注
            InfoHandle.UpdateRemark(apply.id, markTxt.text);
            // 添加 标签
            MyTag tag = GoodFriendManager.getInstance.myTags.FirstOrDefault(tag => tag.name == tagTxt.text);
            if(tag!=null)
            {
                InfoHandle.TagAddStudent(tag.labelId, new List<int>() { apply.id});
            }
        }
    }
}
