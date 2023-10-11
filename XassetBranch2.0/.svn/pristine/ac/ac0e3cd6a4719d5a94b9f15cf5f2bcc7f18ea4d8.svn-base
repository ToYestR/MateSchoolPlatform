using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace FriendSystem
{
    public class GroupEditPrefabItem : MonoBehaviour
    {
        [SerializeField] Text nickName;
        VerticalLayoutGroup verticalLayout;
        ContentSizeFitter sizeFitter;
        RectTransform paretnRect;
        RectTransform rect;
        float height;
        private void Awake()
        {
            rect = GetComponent<RectTransform>();
            paretnRect = transform.parent.GetComponent<RectTransform>();
            verticalLayout = transform.parent.GetComponent<VerticalLayoutGroup>();
            sizeFitter = transform.parent.GetComponent<ContentSizeFitter>();
            height = rect.rect.height;
        }
        public void OnBeginDrag()
        {
            sizeFitter.enabled = false;
            verticalLayout.enabled = false;
            transform.SetAsLastSibling();
        }
        public void OnDrag()
        {
            Vector2 pos = rect.anchoredPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(paretnRect, Input.mousePosition, null, out pos);
            if (pos.y < 0 && pos.y > -paretnRect.rect.height)
            {
                rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, pos.y);
            }
            pos = rect.anchoredPosition;
            int index = (int)((Mathf.Abs(pos.y) + verticalLayout.spacing + height / 2) / (height + verticalLayout.spacing));
            // 下移
            for (int i = 0; i < index; i++)
            {
                pos.y = -(height + verticalLayout.spacing) * i + verticalLayout.spacing - height / 2;
                paretnRect.GetChild(i).GetComponent<RectTransform>().anchoredPosition = pos;
            }
            // 上移
            for (int i = index; i < paretnRect.childCount - 1; i++)
            {
                pos.y = -(height + verticalLayout.spacing) * (i + 1) + verticalLayout.spacing - height / 2;
                paretnRect.GetChild(i).GetComponent<RectTransform>().anchoredPosition = pos;
            }
        }

        public void OnEndDrag()
        {
            int index = (int)((Mathf.Abs(rect.anchoredPosition.y) + verticalLayout.spacing + height / 2) / (height + verticalLayout.spacing));
            transform.SetSiblingIndex(index);
            sizeFitter.enabled = true;
            verticalLayout.enabled = true;
            // TODO 保存排序
        }
        public MyTag target;
        public void Show(MyTag tag)
        {
            target = tag;
            nickName.text = target.name;
        }
        public void DelTag()
        {
            InfoHandle.DelTag(target.labelId, null);
            Destroy(gameObject);
        }
    }
}
