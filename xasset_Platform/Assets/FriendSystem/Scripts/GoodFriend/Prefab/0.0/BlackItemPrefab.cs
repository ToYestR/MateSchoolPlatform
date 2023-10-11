using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

namespace FriendSystem
{
    /// <summary>
    /// ���� ������ Item Ԥ�Ƽ�
    /// </summary>
    public class BlackItemPrefab : MonoBehaviour
    {
        [SerializeField] Image icon;     // ͷ��
        [SerializeField] Button removeBtn;
        [SerializeField] Text nickName;
        public Friend friendContent { get; set; }
        private void Start()
        {
            removeBtn.onClick.AddListener(OnRemoveClick);
        }

        private void OnEnable()
        {
            StartCoroutine(Refresh());
        }
        private IEnumerator Refresh()
        {
            while (true)
            {
                if (friendContent != null)
                {
                    icon.sprite = HeadSculptureWindow.getInstance.GetSprite(friendContent.friendPortrait);
                    icon.SetNativeSize();
                    nickName.text = friendContent.friendNickName;
                }
                yield return null;
            }
        }
        private void OnRemoveClick()
        {
            InfoHandle.BlackOther(friendContent.id, "N");
        }
    }
}