using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UpdateStatus : MonoBehaviour
{
    private UpStatusRequest upStatusRequest;
    private Animator anim;
    private Transform aimPos;

    //private Transform gunTransform;
    // Start is called before the first frame update
    void Start()
    {
        upStatusRequest = GetComponent<UpStatusRequest>();
        anim = GetComponent<Animator>();
        //gunTransform = transform.Find("HandGun");
        InvokeRepeating("UpPosFun", 1, 1f / 10f);
        aimPos = transform.Find("AimTartgetPoint");
    }

    public void UpPosFun()
    {
        //Vector2 pos = transform.position;
        //float characterRot = transform.eulerAngles.z;
        //float gunRot = gunTransform.eulerAngles.z;
        var speed = anim.GetFloat("Speed");
        var jump = anim.GetBool("Jump");
        var grounded = anim.GetBool("Grounded");
        var freefall = anim.GetBool("FreeFall");
        //var aim = anim.GetBool("Aim");
        //var fire = anim.GetBool("Fire");
        //var ver = anim.GetFloat("Ver");
        //var hor = anim.GetFloat("Hor");

        upStatusRequest.SendRequest(transform.position,transform.rotation.eulerAngles.y, speed, jump, grounded, freefall);
        //print("UDP∑¢ÀÕ«Î«Û¡À");
    }

}
