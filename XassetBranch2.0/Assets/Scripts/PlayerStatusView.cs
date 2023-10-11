using Photon.Pun;
using Photon.Voice.PUN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatusView : MonoBehaviourPun, IPunObservable
{
    /// <summary>
    /// 玩家id
    /// </summary>
    public string uid = "";

    public string m_nicakName;
    /// <summary>
    /// 我的状态名称
    /// </summary>
    public string m_AnimatotName;
    /// <summary>
    /// 动画状态机
    /// </summary>
    private Animator m_Animator;
    /// <summary>
    /// 聊天chatNo
    /// </summary>
    public string m_ChatNo;
    /// <summary>
    /// 我的名称
    /// </summary>
    public TextMeshProUGUI m_tittle;
    /// <summary>
    /// 我的角色信息
    /// </summary>
    public string m_roleInfo = "";
    private string m_currentroleInfo = "";
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //发送当前状态
            stream.SendNext(Global.uid.ToString());
            stream.SendNext(m_nicakName);
            stream.SendNext(m_AnimatotName);
            stream.SendNext(Global.chatNo);
            stream.SendNext(Global.roleinfo);
        }
        else
        {
            uid = (string)stream.ReceiveNext();
            m_tittle.text = (string)stream.ReceiveNext();
            m_Animator.Play((string)stream.ReceiveNext());
            m_ChatNo = (string)stream.ReceiveNext();
            m_currentroleInfo = (string)stream.ReceiveNext();
        }
        if(m_currentroleInfo!= m_roleInfo)
        {
            m_roleInfo = m_currentroleInfo;
            GetComponent<AvatarSysScene>().LoadJson(m_roleInfo);
        }
    }
    void Awake()
    {
        m_nicakName = Global.nickname;
        m_tittle.text = m_nicakName;
        m_Animator = GetComponent<Animator>();
    }

}
