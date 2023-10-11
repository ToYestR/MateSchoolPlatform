using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FriendSystem
{
    /// <summary>
    /// 备注窗口页面
    /// </summary>
    public class RemarkPage : MonoBehaviour
    {
        [SerializeField] FriendSettingPage settingPage;
        [SerializeField] Button okBtn;
        [SerializeField] Button back;
        [SerializeField] InputField input;
        [SerializeField] GameObject tip;

        private void Start()
        {
            back.onClick.AddListener(Back);
            okBtn.onClick.AddListener(OkClick);
        }
        private void OnEnable()
        {
            tip.SetActive(false);
            input.text = settingPage.chatPage.target.remark;
        }
        private void Back()
        {
            PageManager.getInstance.Back(nameof(FriendSettingPage));
        }
        private void OkClick()  // 点击确认
        {
            InfoHandle.UpdateRemark(settingPage.chatPage.target.id, input.text, OnResponse);
        }
        private void OnResponse(string json)        // 服务器响应
        {
            Debug.Log("更新好友备注：" + json);
            StartCoroutine(TipAnimation());
        }
        private IEnumerator TipAnimation()
        {
            tip.SetActive(true);
            yield return new WaitForSecondsRealtime(2);
            tip.SetActive(false);
        }
    }
}
