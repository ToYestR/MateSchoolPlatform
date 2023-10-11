using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FriendSystem
{
    /// <summary>
    /// 黑名单 列表 窗口
    /// </summary>
    public class BlackListWindow : MonoBehaviour
    {
        GameObject prefab;                      // BlackItemPrefab 预制件
        [SerializeField] Transform content;    // 预制件父节点
        [SerializeField] Button close;          // 关闭 按钮
        [SerializeField] Text blackCountTxT;    // 黑名单 数量

        #region 
        private Dictionary<string, BlackItemPrefab> friendItems = new Dictionary<string, BlackItemPrefab>();
        private Queue<BlackItemPrefab> releaseItems = new Queue<BlackItemPrefab>();
        #endregion
        private void Awake()
        {
            prefab = content.GetChild(0).gameObject;
            prefab.SetActive(false);
            close.onClick.AddListener(()=>ToggleEnable(false));
        }
        private void OnEnable()
        {
            StartCoroutine(Refresh());
        }
        private void OnDisable()
        {
            StopAllCoroutines();
        }
        private IEnumerator Refresh()   // 列表刷新
        {
            while (true)
            {
                yield return null;
                List<string> releaseId = new List<string>();
                foreach (var friend in friendItems)
                {
                    if (!GoodFriendManager.getInstance.blackList.ContainsKey(friend.Key))
                    {
                        releaseId.Add(friend.Key);
                    }
                }
                foreach (var id in releaseId)
                {
                    ReleasePoolItem(friendItems[id]);
                    friendItems.Remove(id);
                }
                foreach (var friend in GoodFriendManager.getInstance.blackList)
                {
                    if (!friendItems.ContainsKey(friend.Key))
                    {
                        // TODO 添加
                        BlackItemPrefab item = GetPoolItem();
                        item.friendContent = friend.Value;
                        friendItems.Add(friend.Key, item);
                    }
                    else
                    {
                        // TODO 更新数据
                        friendItems[friend.Key].friendContent.CopyForValue(friend.Value);
                    }
                }
                blackCountTxT.text = friendItems.Count.ToString();
                yield return new WaitForSecondsRealtime(GoodFriendManager.getInstance.requestTime);
            }
        }

        /// <summary>
        /// 获取池中的对象
        /// </summary>
        /// <returns></returns>
        private BlackItemPrefab GetPoolItem()
        {
            if (releaseItems.Count > 0)
            {
                BlackItemPrefab item = releaseItems.Dequeue();
                item.gameObject.SetActive(true);
                return item;
            }
            else
            {
                var obj = Instantiate(prefab, content);
                obj.SetActive(true);
                return obj.GetComponent<BlackItemPrefab>();
            }
        }
        /// <summary>
        /// 释放到池中
        /// </summary>
        /// <param name="item"></param>
        private void ReleasePoolItem(BlackItemPrefab item)
        {
            item.gameObject.SetActive(false);
            item.transform.SetAsLastSibling();
            releaseItems.Enqueue(item);
        }

        private void ToggleEnable(bool state)
        {
            transform.GetChild(0).gameObject.SetActive(state);
        }

        public void Show()
        {
            ToggleEnable(true);
        }
    }
}
