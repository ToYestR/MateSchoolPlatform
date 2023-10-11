using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FriendSystem
{
    /// <summary>
    /// ���� ���� ����
    /// </summary>
    public class OperationWindow : MonoBehaviour
    {
        [SerializeField] Button chatBtn;
        [SerializeField] Button delBtn;
        [SerializeField] Button blackBtn;
        [SerializeField] Button inRoomBtn;
        [SerializeField] PopUpFriendWindow popUp;     // ��������
        Friend friend;
        private void Awake()
        {
            ToggleEnable(false);
        }
        private void Start()
        {
            chatBtn.onClick.AddListener(OnChatClick);
            delBtn.onClick.AddListener(OnDeleClick);
            blackBtn.onClick.AddListener(OnBlackClick);
            inRoomBtn.onClick.AddListener(OnInRoomClick);
        }
        private void ToggleEnable(bool state)
        {
            transform.GetChild(0).gameObject.SetActive(state);
        }
        /// <summary>
        /// ��ʾ���� �� չʾ�� ָ��λ��
        /// </summary>
        /// <param name="pos">��ʾλ��</param>
        public void Show(Friend item)
        {
            if (item != null)
            {
                friend = item;
                Vector2 pos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(popUp.GetComponent<RectTransform>(), Input.mousePosition, null, out pos);
                Debug.Log(pos);
                GetComponent<RectTransform>().anchoredPosition = pos;
                ToggleEnable(true);
            }
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !RectTransformUtility.RectangleContainsScreenPoint(transform.GetChild(0).GetComponent<RectTransform>(), Input.mousePosition))
            {
                ToggleEnable(false);
            }
        }
        // ����
        private void OnChatClick()
        {
            ToggleEnable(false);
            popUp.ShowOpenChat(friend);
        }
        // ɾ��
        private void OnDeleClick()
        {
            ToggleEnable(false);
            popUp.ShowDeleWindow(friend);
            InfoHandle.DeleteFr(friend.id, json =>
             {
                 Debug.Log("ɾ�����ѣ�" + json);
             });
        }
        // ������
        private void OnBlackClick()
        {
            ToggleEnable(false);
            InfoHandle.BlackOther(friend.id, "Y");
        }
        // ���뷿��
        private void OnInRoomClick()
        {
            ToggleEnable(false);
        }
    }
}
