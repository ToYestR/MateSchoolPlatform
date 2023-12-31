using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FriendSystem
{
    public class FriendListPanel : PoolManager
    {
        [SerializeField] RectTransform countTXT;

        protected override void Awake()
        {
            base.Awake();
        }
        private void OnEnable()
        {
            foreach (var item in GoodFriendManager.getInstance.friendList)
            {
                var obj = pool.Get();
                obj.GetComponent<FriendListPrefabItem>().Show(item.Value,nameof(ContactsPage));
            }
        }
        private void Update()
        {
            countTXT.GetComponent<Text>().text = GoodFriendManager.getInstance.friendList.Count + "位联系人";
            Vector2 pos = countTXT.anchoredPosition;
            RectTransform rect = content.GetComponent<RectTransform>();
            pos.y = -rect.rect.height + rect.anchoredPosition.y + GoodFriendManager.getInstance.friendList.Count * 5;
            countTXT.anchoredPosition = pos;
        }
        private void OnDisable()
        {
            Clear();
        }
    }
}
