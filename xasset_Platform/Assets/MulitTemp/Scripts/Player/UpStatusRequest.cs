using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Im;
using Google.Protobuf.WellKnownTypes;

public class UpStatusRequest : MonoBehaviour
{


    public void SendRequest(Vector3 pos, float rotY, float speed, bool jump, bool grounded, bool freefall)
    {

        PosPack posPack = new PosPack();
        AnimPack animPack = new AnimPack();
        PlayerPack playerPack = new PlayerPack();

        posPack.X = pos.x;
        posPack.Y = pos.y;
        posPack.Z = pos.z;
        animPack.Speed = speed;
        animPack.Jump = jump;
        animPack.Grounded = grounded;
        animPack.FreeFall = freefall; 

        posPack.RotY = rotY;

        playerPack.Uid = ClientManager.Instance.uid;
        playerPack.PosPack = posPack;
        playerPack.AnimPack = animPack;





        ClientManager.Instance.Send(new UpPos() { PlayerPack = playerPack });
    }

    public void OnResponse(ImMsg pack)
    {
        print("位置同步收到请求了");
        //GameFace.Instance.UpPos(pack);

    }
}
