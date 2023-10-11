using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FriendSystem
{
    public class GroupListPanel : PoolManager
    {
        protected override void Awake()
        {
            base.Awake();
        }
        private void OnEnable()
        {
            foreach (var item in GoodFriendManager.getInstance.myTags)
            {
                var obj = pool.Get();
                obj.GetComponent<GroupListPrefabItem>().Show(item);
            }
        }
        private void OnDisable()
        {
            Clear();
        }
    }
}
