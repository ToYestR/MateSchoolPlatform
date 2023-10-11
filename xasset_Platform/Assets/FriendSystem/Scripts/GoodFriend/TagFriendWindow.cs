using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FriendSystem
{
    /// <summary>
    /// 好友标签 窗口
    /// </summary>
    public class TagFriendWindow : MonoBehaviour
    {
        [Header("控件")]
        [SerializeField] Button addTagBtn;
        [SerializeField] Button editTagBtn;
        [SerializeField] Transform content;
        [SerializeField] Button searchBtn;
        [SerializeField] InputField searchTxT;
        [Header("弹窗")]
        [SerializeField] PopUpTagWindow popUp;

        private GameObject prefab;
        private Dictionary<string, TagItemPrefab> tags = new Dictionary<string, TagItemPrefab>();
        private void Awake()
        {
            prefab = content.GetChild(0).gameObject;
            prefab.gameObject.SetActive(false);
        }

        private void Start()
        {
            addTagBtn.onClick.AddListener(OnAddTagClick);
            editTagBtn.onClick.AddListener(OnEditTagClick);
        }
        private void OnEnable()
        {
            StartCoroutine(Refresh());
        }

        private IEnumerator Refresh()
        {
            List<string> tempTags = new List<string>();
            while (true)
            {
                yield return null;
                tempTags.Clear();
                // 不在 标签内
                foreach (var item in tags.Values)
                {
                    if (!GoodFriendManager.getInstance.myTags.Contains(item.myTag))
                    {
                        tempTags.Add(item.myTag.name);
                    }
                }
                // 销毁
                foreach (var item in tempTags)
                {
                    TagItemPrefab prefab = tags[item];
                    tags.Remove(item);
                    Destroy(prefab.gameObject);
                }
                // 不包含标签 则 添加
                foreach (var item in GoodFriendManager.getInstance.myTags)
                {
                    if (!tags.ContainsKey(item.name))
                    {
                        TagItemPrefab prefab = Instantiate(this.prefab, content).GetComponent<TagItemPrefab>();
                        prefab.Init(item);
                        prefab.gameObject.SetActive(true);
                        tags.Add(item.name, prefab);
                    }
                }
                yield return new WaitForSeconds(GoodFriendManager.getInstance.requestTime);
            }
        }

        // 点击 展示 添加标签 窗口
        private void OnAddTagClick()
        {
            popUp.ShowAddTagWindow(null,null);
        }
        // 点击 展示 编辑标签 窗口
        private void OnEditTagClick()
        {
            popUp.SHowEditTagWindow();
        }

    }
}
