/************************************************
 * Author       :   XXY
 * Blog         :   https://www.cnblogs.com/jzyl
 * Email        :   1047185209@QQ.com
 * FileName     :   BodySelect.cs
 * CreateData   :   2023/2/20 15:38:22
 * UnityVersion :   2020.3.33f1
 * Description  :   描述
************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using EPOOutline;
using FriendSystem;
using XAsset;
public class BodySelect : MonoBehaviour,IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
{
    private Outlinable m_outlinable;
    private PlayerStatusView otherPlayer;

    private void Start()
    {
        otherPlayer = transform.GetComponentInParent<PlayerStatusView>();
        m_outlinable = GetComponent<Outlinable>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (otherPlayer.m_ChatNo != Global.chatNo&& otherPlayer.m_ChatNo!="")
        {
            RequestFriendPanel.GetInstance.Show(otherPlayer.m_ChatNo, otherPlayer.m_nicakName);
        }
        //throw new System.NotImplementedException();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_outlinable.enabled = true;
      }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_outlinable.enabled = false;
    }
}
