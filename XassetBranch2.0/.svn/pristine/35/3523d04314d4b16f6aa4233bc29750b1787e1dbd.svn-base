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
    private CharacterRistic otherPlayer;

    private void Start()
    {
        otherPlayer = GetComponent<CharacterRistic>();
        m_outlinable = GetComponent<Outlinable>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        print("PointerDown");
        //print("mychatNo:" + Global.chatNo + "otherplayer:" + otherPlayer.m_ChatNo);
        if (otherPlayer.chatNo != Global.chatNo&& otherPlayer.chatNo != "")
        {
            print("PointerDown");
            RequestFriendPanel.GetInstance.Show(otherPlayer.chatNo, otherPlayer.username);

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
    public void OnMouseDown()
    {
        Debug.Log("MouseDown");
        print("PointerDown");
        //print("mychatNo:" + Global.chatNo + "otherplayer:" + otherPlayer.m_ChatNo);
        if (otherPlayer.chatNo != Global.chatNo && otherPlayer.chatNo != "")
        {
            print("PointerDown");
            RequestFriendPanel.GetInstance.Show(otherPlayer.chatNo, otherPlayer.username);

        }
    }
    public void OnMouseOver()
    {
        Debug.Log("Over");
        m_outlinable.enabled = true;
    }
    public void OnMouseExit()
    {
        m_outlinable.enabled = false;
    }
}
