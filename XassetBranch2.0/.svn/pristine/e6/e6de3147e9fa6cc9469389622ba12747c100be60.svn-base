using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FriendSystem
{
    /// <summary>
    /// 好友申请 弹出面板
    /// </summary>
    public class FriendApplyWindow : MonoBehaviour
    {
        [SerializeField] Button closeBtn;
        [SerializeField] Transform content;
        [SerializeField]Text applyCountTxT;

        GameObject prefab;
        private void Start()
        {
            prefab = content.GetChild(0).gameObject;
            prefab.SetActive(false);

            closeBtn.onClick.AddListener(() => ToggleEnable(false));
        }
        private void ToggleEnable(bool state)
        {
            transform.GetChild(0).gameObject.SetActive(state);
        }
        private void OnEnable()
        {
           
        }
        private void Update()
        {
            applyCountTxT.text = GoodFriendManager.getInstance.applyList.Count.ToString();
        }

        public void Show()
        {
            ToggleEnable(true);
            int index = 0;
            foreach (var item in GoodFriendManager.getInstance.applyList)
            {
                index++;
                ApplyFriendItemPrefab tagItem;
                if (content.childCount > index)
                {
                    tagItem = content.GetChild(index).GetComponent<ApplyFriendItemPrefab>();
                }
                else
                {
                    tagItem = Instantiate(prefab, content).GetComponent<ApplyFriendItemPrefab>();
                }
                tagItem.gameObject.SetActive(true);
                tagItem.Show(item.Value);
            }
        }
        public void Close()
        {
            ToggleEnable(false);
        }
    }
}
