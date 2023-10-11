using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace FriendSystem
{
    /// <summary>
    /// 好友权限设置面板
    /// </summary>
    public class FriendLimitPage : PoolManager
    {
        [SerializeField] TMP_InputField search;
        [SerializeField] Toggle allSelect;
        [SerializeField] Button okBtn;
        [SerializeField] Transform tip;

        protected override void Awake()
        {
            base.Awake();
        }
        private void Start()
        {
            okBtn.onClick.AddListener(OnOkClick);
            search.onValueChanged.AddListener(OnSearchChange);
        }


        private void OnEnable()
        {
            foreach (var item in GoodFriendManager.getInstance.friendList)
            {
                var obj = pool.Get();
                obj.GetComponent<FriendLimitPrefabItem>().Show(item.Value);
            }
            allSelect.onValueChanged.AddListener(OnSelectAll);
            tip.gameObject.SetActive(false);
        }
        private void OnDisable()
        {
            allSelect.onValueChanged.RemoveAllListeners();
            allSelect.isOn = false;
            Clear();
        }
        private void OnSelectAll(bool arg)      // 全选/全取消
        {
            foreach (var obj in busyObjects)
            {
                obj.GetComponent<FriendLimitPrefabItem>().SetToggle(arg);
            }
        }
        private void OnOkClick()    // 点击完成操作
        {
            foreach (var obj in busyObjects)
            {
                obj.GetComponent<FriendLimitPrefabItem>().Save();
            }
            StartCoroutine(TipAnimation());
        }
        private void OnSearchChange(string arg0)    // 搜索
        {
            foreach (var obj in busyObjects)
            {
                obj.GetComponent<FriendLimitPrefabItem>().SearchMe(arg0);
            }
        }
        private IEnumerator TipAnimation()
        {
            tip.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(2);
            tip.gameObject.SetActive(false);
        }
    }
}
