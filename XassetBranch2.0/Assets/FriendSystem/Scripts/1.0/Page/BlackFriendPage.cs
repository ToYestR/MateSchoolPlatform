using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FriendSystem
{
    /// <summary>
    /// 黑名单面板
    /// </summary>
    public class BlackFriendPage : PoolManager
    {
        [SerializeField] TMP_InputField search;
        [SerializeField] Transform tip;
        protected override void Awake()
        {
            base.Awake();
            search.onValueChanged.AddListener(OnSearchChange);
        }

        private void OnEnable()
        {
            foreach (var item in GoodFriendManager.getInstance.blackList)
            {
                var obj = pool.Get();
                obj.GetComponent<BlackFriendPrefabItem>().Show(item.Value, this);
            }
            tip.gameObject.SetActive(false);
        }
        private void OnDisable()
        {
            Clear();
        }
        public void ShowTip()
        {
            StartCoroutine(TipAnimation());
        }
        private IEnumerator TipAnimation()
        {
            tip.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(2);
            tip.gameObject.SetActive(false);
        }
        private void OnSearchChange(string arg0)    // 搜索
        {
            foreach (var obj in busyObjects)
            {
                obj.GetComponent<BlackFriendPrefabItem>().SearchMe(arg0);
            }
        }

    }
}
