using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FriendSystem
{
    /// <summary>
    /// 下拉折叠功能
    /// </summary>
    public class DropDownMenu : MonoBehaviour
    {
        [SerializeField] RectTransform menuContent;
        [SerializeField] Toggle toggle;     // 显示隐藏切换开关
        [SerializeField] RectTransform content; // 展示Item 面板
        [SerializeField] float dropDown;
        [SerializeField] float itemHeight;      // 子Item 高度
        [SerializeField, Range(1, 30)] float animationSpeed = 15;
        private void Start()
        {
            toggle.onValueChanged.AddListener(arg =>
            {
                if (gameObject.activeInHierarchy == false) return;
                StopAllCoroutines();
                if (arg)
                {
                    StartCoroutine(ExpandAnimation());
                }
                else
                {
                    StartCoroutine(FoldAnimation());
                }
            });
        }
        private int activeChildCount;
        private void OnEnable()
        {
            activeChildCount= content.transform.GetComponentsInChildren<FriendListPrefabItem>(false).Length;
            if (toggle.isOn)
            {
                content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, activeChildCount * itemHeight);
                menuContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, activeChildCount * itemHeight + dropDown);
            }
            else
            {
                content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
                menuContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0 + dropDown);
            }
        }
        private void OnDisable()
        {
            content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
            menuContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0 + dropDown);
            toggle.isOn = false;
        }
        private IEnumerator FoldAnimation()
        {
            float currentContentHeight = content.rect.height;
            float endHight = 0;
            while (currentContentHeight > endHight)
            {
                yield return null;
                currentContentHeight = Mathf.MoveTowards(currentContentHeight, endHight, animationSpeed);
                content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, currentContentHeight);
                menuContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, currentContentHeight + dropDown);
            }
        }
        private IEnumerator ExpandAnimation()
        {
            float currentContentHeight = content.rect.height;
            float endHight = activeChildCount * itemHeight;
            while (currentContentHeight < endHight)
            {
                yield return null;
                currentContentHeight = Mathf.MoveTowards(currentContentHeight, endHight, animationSpeed);
                content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, currentContentHeight);
                menuContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, currentContentHeight + dropDown);
            }
        }
    }
}
