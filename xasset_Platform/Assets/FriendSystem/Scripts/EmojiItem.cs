using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace FriendSystem
{
    public class EmojiItem : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] ChatPage chatWindow;
        public void OnPointerClick(PointerEventData eventData)
        {
            chatWindow.AddEmoji(transform.GetSiblingIndex());
        }
    }
}