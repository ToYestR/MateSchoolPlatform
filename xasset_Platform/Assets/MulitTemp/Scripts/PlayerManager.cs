using Im;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.TextCore.Text;
using StarterAssets;
using UnityEngine.InputSystem;
using Google.Protobuf.WellKnownTypes;
using System.Runtime.Serialization;

public class PlayerManager : MonoBehaviour
{
    GameObject player;
    private Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();
    static PlayerManager _instance;
    public static PlayerManager Instance
    {
        get
        {

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        player = Resources.Load<GameObject>("Player");

    }


    private void Update()
    {

        if (pack != null && pack.Msg.TypeUrl == "type.googleapis.com/Im.JoinRoomResponse")
        {
            HandleJoinRommResponse(pack.Msg.Unpack<JoinRoomResponse>());
            
        }
        else if (pack != null && pack.Msg.TypeUrl == "type.googleapis.com/Im.AddPlayer")
        {
            HandleAddPlayer(pack.Msg.Unpack<AddPlayer>());

        }
        else if (pack != null && pack.Msg.TypeUrl == "type.googleapis.com/Im.UpPos")
        {
            HandleUpPos(pack.Msg.Unpack<UpPos>());

        }
        else if (pack != null && pack.Msg.TypeUrl == "type.googleapis.com/Im.ExitRoom")
        {
            HandleExitRoom(pack.Msg.Unpack<ExitRoom>());

        }
    }

    public ImMsg pack;

    public void HandleJoinRommResponse(JoinRoomResponse pack)
    {
        this.pack = null;
        if (GameObject.Find("Canvas") != null)
        {
            GameObject.Find("Canvas").gameObject.SetActive(false);
        }

        Debug.Log("执行了");
        Debug.Log(pack.PlayerPack.Count);
        foreach (var p in pack.PlayerPack)
        {
            Debug.Log("添加角色" + p.Uid);

            //pos.x += (posindex += 2);
            GameObject g = Object.Instantiate(player,GameObject.Find("Agent").transform);

            if (p.Uid.Equals(ClientManager.Instance.uid))
            {
                //创建本地角色
                var controller= g.GetComponent<ThirdPersonController>();
                CharacterRistic characterRistic = g.GetComponent<CharacterRistic>();
                characterRistic.isLocal = true;
                characterRistic.username = p.NickName;
                g.AddComponent<UpStatusRequest>();
                g.AddComponent<UpdateStatus>();

                 
            }


            else
            {
                //创建其他客户端的角色
                CharacterRistic characterRistic = g.GetComponent<CharacterRistic>();
                characterRistic.isLocal = false;
                characterRistic.username = p.NickName;
                var controller = g.GetComponent<ThirdPersonController>();
                g.AddComponent<RemoteCharacter>();

                //Object.Destroy(g.GetComponentInChildren<Camera>().gameObject);

                Object.Destroy(controller.GetComponentInChildren<Camera>().gameObject);
                Object.Destroy(controller.GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>().gameObject);

                Object.Destroy(g.GetComponent<PlayerInput>());

            }
            players.Add(p.Uid, g);

        }

    }
    public void HandleAddPlayer(AddPlayer pack)
    {
        this.pack = null;
        GameObject g = Object.Instantiate(player, Vector3.zero, Quaternion.identity);
        //创建其他客户端的角色
        CharacterRistic characterRistic = g.GetComponent<CharacterRistic>();
        characterRistic.isLocal = false;
        characterRistic.uid = pack.PlayerPack.Uid;
        characterRistic.username = pack.PlayerPack.NickName;
        characterRistic.tittle.text = pack.PlayerPack.NickName;
        characterRistic.chatNo = pack.PlayerPack.Chatno;
        var controller = g.GetComponent<ThirdPersonController>();
        g.AddComponent<RemoteCharacter>();

        //Object.Destroy(g.GetComponentInChildren<Camera>().gameObject);

        Object.Destroy(controller.GetComponentInChildren<Camera>().gameObject);
        Object.Destroy(controller.GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>().gameObject);

        Object.Destroy(g.GetComponent<PlayerInput>());
        players.Add(pack.PlayerPack.Uid, g);
    }

    public void HandleUpPos(UpPos pack)
    {
        var playerPack = pack.PlayerPack;
        var posPack = playerPack.PosPack;
        var animPack = playerPack.AnimPack;
        players.TryGetValue(playerPack.Uid, out GameObject g);
        g.GetComponent<RemoteCharacter>().SetState(new Vector3(posPack.X, posPack.Y, posPack.Z), posPack.RotY, animPack.Speed, animPack.Jump, animPack.Grounded,animPack.FreeFall);
    }

    public void HandleExitRoom(ExitRoom pack)
    {
        var playerPack = pack.PlayerPack;
        players.TryGetValue(playerPack.Uid, out GameObject g);
        DestroyImmediate(g);
    }

    private void OnDestroy()
    {

    }
}
