using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FriendSystem
{
    /// <summary>
    /// ���� ɾ�� ����
    /// </summary>
    public class DeleteFriendWindow : MonoBehaviour
    {
        [SerializeField] Button okBtn;
        [SerializeField] Button cancelBtn;
        [SerializeField] Button successBtn;
        [SerializeField] Text tip;

        [SerializeField] PopUpFriendWindow popUp;     // ��������
        Friend friend;

        private void Start()
        {
            okBtn.onClick.AddListener(OnOkClick);
            cancelBtn.onClick.AddListener(OnCancelClick);
            successBtn.onClick.AddListener(OnSuccessClick);
        }
        public void Show(Friend item)
        {
            if (item != null)
            {
                friend = item;
                tip.text = $"�Ƿ�ɾ�����ѣ�{friend.friendNickName}";
                okBtn.gameObject.SetActive(true);
                cancelBtn.gameObject.SetActive(true);
                successBtn.gameObject.SetActive(false);
            }
            else
            {
                tip.text = "ɾ���ɹ�";
                okBtn.gameObject.SetActive(false);
                cancelBtn.gameObject.SetActive(false);
                successBtn.gameObject.SetActive(true);
            }
        }
        private void Update()
        {
            if(friend==null)
            {
                if (Input.GetMouseButtonDown(0) && !RectTransformUtility.RectangleContainsScreenPoint(transform.GetChild(0).GetComponent<RectTransform>(), Input.mousePosition))
                {
                    ToggleEnable(false);
                }
            }
        }
        /// <summary>
        /// �л���ʾ����
        /// </summary>
        /// <param name="state"></param>
        private void ToggleEnable(bool state)
        {
            transform.GetChild(0).gameObject.SetActive(state);
        }
        // ɾ������
        private void OnOkClick()
        {
            InfoHandle.DeleteFr(friend.friendUserId, OnResponse);
            ToggleEnable(false);
        }
        private void OnResponse(string json)
        {
            Debug.Log(json);
            Show(null);
        }
        private void OnCancelClick()
        {
            ToggleEnable(false);
        }
        // ɾ���ɹ�
        private void OnSuccessClick()
        {
            ToggleEnable(false);
        }
    }
}
