
using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteCharacter : MonoBehaviour
{
    private ThirdPersonController player;
    private Vector3 selfPos;
    private Quaternion selfAngle;
    private Animator anim;
    private Transform AimPos;
    //private AimIK aimIK;


    private void Start()
    {
        selfPos = transform.position;
        TryGetComponent<Animator>(out anim);
        //AimPos = transform.Find("AimTartgetPoint");
        //TryGetComponent<AimIK>(out aimIK);
        TryGetComponent(out player);
    }

    //角色的
    public void SetState(Vector3 selfpos, float rotY, float speed, bool jump, bool grounded, bool freefall)
    {
        selfPos = selfpos;

        selfAngle = Quaternion.Euler(0, rotY, 0);
        anim.SetFloat("Speed", speed);
        anim.SetBool("Jump", jump);
        anim.SetBool("Grounded", grounded);
        anim.SetBool("FreeFall", freefall);
    }
    //public void SetState(Vector3 selfpos, float rotY)
    //{
    //    selfPos = selfpos;
    //    selfAngle = Quaternion.Euler(0, rotY, 0);
        

        
    //}

    //private void Start()
    //{
    //    selfTransform = transform;
    //    //gunTransform = transform.Find("HandGun");
    //    //selfAngle = selfTransform.rotation;
    //    //selfPos = selfTransform.position;
    //    //gunAngle = gunTransform.rotation;
    //}

    // Update is called once per frame
    void Update()
    {

        transform.position = Vector3.Lerp(transform.position, selfPos, 0.1f);
        transform.rotation = Quaternion.Slerp(transform.rotation, selfAngle, 0.25f);
        //gunTransform.rotation = Quaternion.Slerp(gunTransform.rotation, gunAngle, 0.25f);
    }
}
