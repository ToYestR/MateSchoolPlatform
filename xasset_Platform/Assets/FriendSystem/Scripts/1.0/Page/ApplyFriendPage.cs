using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FriendSystem
{
    /// <summary>
    /// 好友申请面板
    /// </summary>
    public class ApplyFriendPage : PoolManager
    {
        [SerializeField] Button backBtn;
        [SerializeField] Button addBtn;
        [SerializeField] Transform tip;
        protected override void Awake()
        {
            base.Awake();
        }
        private void Start()
        {
            backBtn.onClick.AddListener(Back);
            addBtn.onClick.AddListener(OnOpenAddPanel);
        }

        string oldPage;
        private void Show(string oldPage)    // 显示当前页面
        {
            this.oldPage = oldPage;
        }
        private void Back()                 // 回到之前页面
        {
            PageManager.getInstance.Back(oldPage);
        }
        private void OnEnable()
        {
            foreach (var item in GoodFriendManager.getInstance.applyList)
            {
                var obj = pool.Get();
                obj.GetComponent<FriendApplyPrefabItem>().Show(item.Value,this);
            }
            tip.gameObject.SetActive(false);
        }
        private void OnDisable()
        {
            Clear();
        }
        private void OnOpenAddPanel()   // 打开添加好友面板
        {
            PageManager.getInstance.Show(nameof(ApplyFriendPage), nameof(AddFriendPage));
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
    }
}
