using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FriendSystem
{
    public class AllFriendWindow : MonoBehaviour
    {
        [SerializeField] PopUpFriendWindow popUp;
        [SerializeField] Text friendCountTxT;       // 好友数量文本
        [SerializeField] InputField searchTxT;      // 搜索好友字段
        [SerializeField] Button searchBtn;          // 搜索按钮
        [SerializeField] Button blackBtn;           // 黑名单窗口按钮
        [SerializeField] Transform content;         // 好友Item 父节点

        private GameObject prefab;                  // 好友 Item 预制件
        #region     
        private Dictionary<string, FriendItemPrefab> friendItems = new Dictionary<string, FriendItemPrefab>();
        private Queue<FriendItemPrefab> releaseItems = new Queue<FriendItemPrefab>();
        #endregion

        private void Awake()
        {
            prefab = content.GetChild(0).gameObject;
            prefab.gameObject.SetActive(false);
            blackBtn.onClick.AddListener(() => popUp.ShowBlackWindow());
        }
        private void OnEnable()
        {
            StartCoroutine(Refresh());
        }
        private void Start()
        {
            searchBtn.onClick.AddListener(OnSearchFriend);
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
                    if (!GoodFriendManager.getInstance.friendList.ContainsKey(friend.Key))
                    {
                        releaseId.Add(friend.Key);
                    }
                }
                foreach (var id in releaseId)
                {
                    ReleasePoolItem(friendItems[id]);
                    friendItems.Remove(id);
                }
                foreach (var friend in GoodFriendManager.getInstance.friendList)
                {
                    if (!friendItems.ContainsKey(friend.Key))
                    {
                        // TODO 添加
                        FriendItemPrefab item = GetPoolItem();
                        item.friendContent = friend.Value;
                        friendItems.Add(friend.Key, item);
                    }
                    else
                    {
                        // TODO 更新数据
                        friendItems[friend.Key].friendContent.CopyForValue(friend.Value);
                    }
                }
                friendCountTxT.text = friendItems.Count.ToString();
                yield return new WaitForSecondsRealtime(GoodFriendManager.getInstance.requestTime);
            }
        }
        /// <summary>
        /// 搜索好友 通过输入字段显示或隐藏 好友 Item
        /// </summary>
        private void OnSearchFriend()
        {
            if(searchTxT.text.Length<1)
            {
                foreach(var item in friendItems)
                {
                    item.Value.gameObject.SetActive(true);
                }
            }
            else
            {
                foreach (var item in friendItems)
                {
                    if(item.Value.friendContent.friendNickName.Contains(searchTxT.text))
                    {
                        item.Value.gameObject.SetActive(true);
                    }
                    else
                    {
                        item.Value.gameObject.SetActive(false);
                    }
                }
            }
        }

        /// <summary>
        /// 获取池中的对象
        /// </summary>
        /// <returns></returns>
        private FriendItemPrefab GetPoolItem()
        {
            if (releaseItems.Count > 0)
            {
                FriendItemPrefab item = releaseItems.Dequeue();
                item.gameObject.SetActive(true);
                return item;
            }
            else
            {
                var obj = Instantiate(prefab, content);
                obj.SetActive(true);
                return obj.GetComponent<FriendItemPrefab>();
            }
        }
        /// <summary>
        /// 释放到池中
        /// </summary>
        /// <param name="item"></param>
        private void ReleasePoolItem(FriendItemPrefab item)
        {
            item.gameObject.SetActive(false);
            item.transform.SetAsLastSibling();
            releaseItems.Enqueue(item);
        }
    }
}
