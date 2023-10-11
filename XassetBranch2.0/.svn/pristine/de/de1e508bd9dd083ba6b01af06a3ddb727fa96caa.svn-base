using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FriendSystem
{
    public class FriendRoomLimitPage : MonoBehaviour
    {
        [SerializeField] FriendSettingPage settingPage;
        [SerializeField] Button okBtn;
        [SerializeField] Button back;
        [SerializeField] Toggle roomLimit;
        [SerializeField] Transform tip;

        private void Start()
        {
            okBtn.onClick.AddListener(OkCLick);
            back.onClick.AddListener(Back);
            roomLimit.onValueChanged.AddListener(OnRoomLimitChange);
        }
        private void OnEnable()
        {
            tip.gameObject.SetActive(false);
            roomLimit.isOn = settingPage.chatPage.target.roomLimit;
        }
        private IEnumerator TipAnimation()
        {
            tip.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(2);
            tip.gameObject.SetActive(false);
        }
        private void OkCLick()
        {
            StartCoroutine(TipAnimation());
        }
        private void Back()
        {
            PageManager.getInstance.Back(nameof(FriendSettingPage));
        }
        private void OnRoomLimitChange(bool arg)    // 房间允许访问权限修改
        {
            settingPage.chatPage.target.roomLimit = arg;
        }
    }
}
