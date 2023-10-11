using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using System;
using Photon.Voice.Unity;
using Photon.Voice.PUN;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Invector.vCharacterController;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace XAsset
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        private string NetworkClientState;//��Ϣ��ʾ
        private string scenename;
        public Transform m_agent;
        private void Awake()
        {
            if(Global.currentchildsceneid!="")
            {
                scenename = Global.currentchildsceneid;
            }
            else
            {
                scenename = Global.currentseneid;
            }
            Connect();
        }
        public void Connect()
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinOrCreateRoom(scenename, new RoomOptions() {MaxPlayers = 0 }, null);
            }
            else
            {
                PhotonNetwork.NetworkingClient.SerializationProtocol = SerializationProtocol.GpBinaryV16;
                PhotonNetwork.ConnectUsingSettings();
            }
        }
        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            PhotonNetwork.JoinOrCreateRoom(scenename, new Photon.Realtime.RoomOptions { MaxPlayers = 0 }, default);
        }
        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();
            Debug.Log("�������䣺" + PhotonNetwork.CurrentRoom);

        }
        public override void OnJoinedRoom()
        {
            GameObject player;
            //�жϸü����ĸ�ģ��
            if (Global.roleinfo != "")
            {
                JObject jobject = JsonConvert.DeserializeObject<JObject>(Global.roleinfo);
                int sexCount = Int32.Parse(jobject["sexJson"].ToString());
                if (sexCount == 0)
                {
                     player = PhotonNetwork.Instantiate("Prefab/Ů����ɫ", Vector3.zero, Quaternion.identity, 0);
                }
                else
                {
                     player = PhotonNetwork.Instantiate("Prefab/������ɫ", Vector3.zero, Quaternion.identity, 0);

                }
            }
            else
            {
                 player = PhotonNetwork.Instantiate("Prefab/Ů����ɫ", Vector3.zero, Quaternion.identity, 0);
            }
            player.transform.parent = m_agent;
            player.transform.localPosition = Vector3.zero;
            //YZL��ʱע��
            player.transform.GetComponentInChildren<vThirdPersonInput>().enabled = true;
            player.transform.GetChild(1).gameObject.SetActive(true);
            Debug.Log("���뵽���䣺" + PhotonNetwork.CurrentRoom);
			//player.GetComponent<Player>().MoveTo(Global.current_pos);
            Debug.Log("***********:" + Global.current_pos);
        }
        public void OnDestroy()
        {
            PhotonNetwork.LeaveRoom();
        }
        /// <summary>
        /// ����uid
        /// </summary>
        /// <param name="uid"></param>
        public void EnterPeronalSpace(string uid)
        {
            Debug.Log(uid);
        }
    }
}