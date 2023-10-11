using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FriendSystem
{
    /// <summary>
    /// ºÃÓÑ É¾³ý µ¯´°
    /// </summary>
    public class DeleteFriendWindow : MonoBehaviour
    {
        [SerializeField] Button okBtn;
        [SerializeField] Button cancelBtn;
        [SerializeField] Button successBtn;
        [SerializeField] Text tip;

        [SerializeField] PopUpFriendWindow popUp;     // µ¯³ö´°¿Ú
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
                tip.text = $"ÊÇ·ñÉ¾³ýºÃÓÑ£º{friend.friendNickName}";
                okBtn.gameObject.SetActive(true);
                cancelBtn.gameObject.SetActive(true);
                successBtn.gameObject.SetActive(false);
            }
            else
            {
                tip.text = "É¾³ý³É¹¦";
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
        /// ÇÐ»»ÏÔÊ¾´°¿Ú
        /// </summary>
        /// <param name="state"></param>
        private void ToggleEnable(bool state)
        {
            transform.GetChild(0).gameObject.SetActive(state);
        }
        // É¾³ýºÃÓÑ
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
        // É¾³ý³É¹¦
        private void OnSuccessClick()
        {
            ToggleEnable(false);
        }
    }
}
