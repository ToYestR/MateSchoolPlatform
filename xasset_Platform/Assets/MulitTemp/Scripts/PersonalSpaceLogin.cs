using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Im;
using System.Threading;
using Google.Protobuf.WellKnownTypes;
/// <summary>
/// ���ڸ��˿ռ��½�Ľű�
/// </summary>
public class PersonalSpaceLogin : MonoBehaviour
{
    public Button loginBtn, connectBtn, press, JoinRoom;
    public InputField serverIP, port, content,nickName, roomNum;
    public int threadNum;
   

    private void Start()
    {
        //loginBtn.onClick.AddListener(OnLoginBtnClick);
        //JoinRoom.onClick.AddListener(OnJoinBtnClick);
        //connectBtn.onClick.AddListener(OnConnectBtnClick);
        //press.onClick.AddListener(Pressure);
        //Debug.Log("ִ��Start");
        if(ClientManager.Instance!=null)
        {
            ClientManager.Instance = null;
        }
        ClientManager.Instance.InitSocket("47.101.199.191", "10001");
        if (Global.gotospacename != "")
        {
            ClientManager.Instance.roomname = Global.gotospacename;
        }
        else
        {
            ClientManager.Instance.roomname = Global.uid.ToString();
        }
        LoginFuction();

        //if (ClientManager.Instance.socket==null)
        //{
        //    ClientManager.Instance.InitSocket("47.101.199.191", "10001");
        //    LoginFuction();
        //}
        //else
        //{
        //    //ֱ�Ӽ��뷿�䷽��
        //    //JoinRoomFuction();
        //    LoginFuction();
        //}
        //��½+�������
    }

    public void Pressure()
    {
        for (int i = 0; i < threadNum; i++)
        {
            Thread thread = new Thread(PresureTest);
            thread.Start();
            Debug.Log("�����߳�" + i.ToString());
        }
    }
    /// <summary>
    /// ѹ������ ���е��÷���
    /// </summary>
    public void PresureTest()
    {
        ClientManager client = new ClientManager();
        client.InitSocket("54.223.80.84", "10001");
        client.Send( new LoginRequest() { Uid = "123456" });
    }


    private void OnConnectBtnClick()
    {
        ClientManager.Instance.InitSocket(serverIP.text, port.text);
    }
    /// <summary>
    /// ��½�Ĵ����Զ����뷿��
    /// </summary>
    public void LoginFuction()
    {
        
        ClientManager.Instance.Send(new LoginRequest()
        {
            Uid = Global.uid.ToString(),
            PlayerPack = new PlayerPack()
            {
                Uid = Global.uid.ToString(),
                NickName = Global.nickname,
                Chatno = Global.chatNo,
                RoleInfo=Global.roleinfo
            }
        }) ;
        ClientManager.Instance.uid = Global.uid.ToString();
        Debug.Log("���͵�¼����uidΪ" + Global.uid);

    }

    public void JoinRoomFuction()
    {
        ClientManager.Instance.Send(new JoinRoomRequest()
        { RoomNo = ClientManager.Instance.roomname });
        Debug.Log("���ͼӷ������󣬷�����Ϊ" + ClientManager.Instance.roomname);


    }
    public void OnLoginBtnClick()
    {

        ClientManager.Instance.Send(new LoginRequest() { Uid = content.text, PlayerPack = new PlayerPack() {NickName = nickName.text } });
        ClientManager.Instance.uid = content.text;
        Debug.Log("���͵�¼����uidΪ" + content.text);
    }
    public void OnJoinBtnClick()
    {

        ClientManager.Instance.Send(new JoinRoomRequest() { RoomNo = roomNum.text });
        Debug.Log("���ͼ��뷿������roomNOΪ " + roomNum.text);
    }
    public void OnDestroy()
    {

        //Debug.Log("ִ�г����˳�,��������"+ ClientManager.Instance.roomname);
        //ClientManager.Instance.Send(new ExitRoomRequest() {RoomNo=ClientManager.Instance.roomname});
    }
}
