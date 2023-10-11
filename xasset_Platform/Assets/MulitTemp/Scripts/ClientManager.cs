using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

using UnityEngine;
using Im;
using System.Net;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;

public class ClientManager
{
    Socket socket;
    Message message;
    PlayerManager playerManager;
    public string uid;
    public string nickName;
    static ClientManager _instance;
    public static ClientManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ClientManager();
            }
            return _instance;
        }
    }

    //private Thread aucThread;

    public ClientManager()
    {
        message = new Message();
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        //InitSocket();
        //StartClient();
    }


    /// <summary>
    /// 初始化socket
    /// </summary>
    public void InitSocket(string ip, string port)
    {

        try
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ip,  int.Parse(port));
            StartReceive();
        }
        catch (Exception e)
        {
            Debug.LogError("连接出错" + e.Message);
        }
    }

    /// <summary>
    /// 关闭socket
    /// </summary>
    private void CloseSocket()
    {
        if (socket.Connected && socket != null)
        {
            socket.Close();
        }
    }

    private void StartReceive()
    {
        socket.BeginReceive(message.Buffer, message.StartIndex, message.Remsize, SocketFlags.None, OnReceive, null);
    }

    private void OnReceive(IAsyncResult ar)
    {
        try
        {
            if (socket == null || !socket.Connected) return;
            int len = socket.EndReceive(ar);
            if (len == 0)
            {
                CloseSocket();
                return;
            }
            message.ReadBuffer(len, HandleResponse);
            StartReceive();
        }
        catch
        {

        }
    }

    /// <summary>
    /// 客户端处理消息 pack是接收到的包
    /// </summary>
    /// <param name="pack"></param>
    private void HandleResponse(ImMsg pack)
    {

        Debug.Log(pack.Msg.TypeUrl);

        if (pack.Msg.TypeUrl == "type.googleapis.com/Im.LoginResponse")
        {
            //msg = pack.Msg;
            Debug.Log(pack.Msg.Unpack<LoginResponse>().Result);
            //如果存在个人空间
            if(Global.gotospacename!="")
            {
                ClientManager.Instance.Send(new JoinRoomRequest() { RoomNo = Global.gotospacename });
            }
            else
            {
                ClientManager.Instance.Send(new JoinRoomRequest() { RoomNo = Global.currentseneid });
            }
            Debug.Log("发送加入房间请求，roomNO为 " + Global.currentseneid);

        }

        else if (pack.Msg.TypeUrl == "type.googleapis.com/Im.JoinRoomResponse")
        {
            Debug.Log("执行了");
            playerManager.pack = pack;
            

        }
        else if (pack.Msg.TypeUrl == "type.googleapis.com/Im.AddPlayer")
        {
            Debug.Log("执行了");
            playerManager.pack = pack;

        }
        else if (pack.Msg.TypeUrl == "type.googleapis.com/Im.UpPos")
        {
            Debug.Log("执行了");
            playerManager.pack = pack;

        }

        else if (pack.Msg.TypeUrl == "type.googleapis.com/Im.ExitRoom")
        {
            Debug.Log("执行了");
            playerManager.pack = pack;

        }


    }


    /// <summary>
    /// 客户端发送消息 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="msg"></param>
    public void Send<T>(T msg) where T : IMessage , new() 
    {
        ImMsg pack = new ImMsg();
        pack.Msg = Google.Protobuf.WellKnownTypes.Any.Pack(msg);
        byte[] sendData = Message.PackData(pack);
        socket.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, new AsyncCallback(SendCallback), null);

    }

    private void SendCallback(IAsyncResult result)
    {
        // 发送数据完成
        int bytesSent = socket.EndSend(result);
        Debug.Log("Sent " + bytesSent + " bytes to server");
    }

    ~ClientManager()
    {
        Send(new Logout() { PlayerPack = new PlayerPack() { Uid = ClientManager.Instance.uid } });
    }


}
