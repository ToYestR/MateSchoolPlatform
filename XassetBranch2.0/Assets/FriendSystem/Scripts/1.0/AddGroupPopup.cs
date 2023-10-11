using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

namespace FriendSystem
{
    public class AddGroupPopup : MonoBehaviour
    {
        [SerializeField] InputField input;
        [SerializeField] Button cancelBtn;
        [SerializeField] Button addBtn;
        private void Start()
        {
            cancelBtn.onClick.AddListener(OnCancel);
            addBtn.onClick.AddListener(OnOk);
        }

        private void OnOk()
        {
            action?.Invoke(input.text);
            gameObject.SetActive(false);
        }

        private void OnCancel()
        {
            gameObject.SetActive(false);
        }

        private UnityAction<string> action;
        public void Show(UnityAction<string> action)
        {
            input.text = "";
            this.action = action;
            gameObject.SetActive(true);
        }
    }
}
