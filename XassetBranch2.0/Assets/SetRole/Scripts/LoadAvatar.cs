using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadAvatar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (AvatarSys._instance.sexCount == 0)
        {
            // Debug.Log("女孩");
            AvatarSys._instance.GirlAvatertest();
        }
        else
        {
            // Debug.Log("男孩");
            AvatarSys._instance.BoyAvatertest();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
