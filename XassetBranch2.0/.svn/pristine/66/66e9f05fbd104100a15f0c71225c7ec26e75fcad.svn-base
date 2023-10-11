using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using System;
#pragma warning disable CS0108 // 成员隐藏继承的成员；缺少关键字 new
// Use physics raycast hit from mouse click to set agent destination
[RequireComponent(typeof(NavMeshAgent))]
public class ClickToMove : MonoBehaviour
{
    public GameObject clickEffect;
    NavMeshAgent m_Agent;
    RaycastHit[] m_HitInfo;
    RaycastHit info;
    Rigidbody rigidbody;
    Animator animator;
    GameObject parent;
    List<string> tags;
    public static bool isNav;
    bool isClickNavMesh;
    private void Awake()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        tags = new List<string>();
        m_Agent.enabled = false;
        isNav = false;
    }

    void Start()
    {
       
    }

    void Update()
    {
        // 鼠标控制角色
        if (Input.GetMouseButtonDown(0) && Camera.main )
        {
            tags.Clear();
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            m_HitInfo = Physics.RaycastAll(ray);
            Array.ForEach(m_HitInfo, info =>
             {
                 string tag = info.collider.tag;
                 tags.Add(tag);
                 if(tag=="NavMesh")
                 {
                     this.info = info;
                     isClickNavMesh = true;
                 }
             });
            if(!tags.Contains("NPC")&&tags.Count>0&& isClickNavMesh)
            {
                var effect = Instantiate(clickEffect, this.info.point + Vector3.up * 0.005f, Quaternion.Euler(0, 0, 0), parent.transform);
                effect.transform.forward = this.info.normal;
                rigidbody.isKinematic = true;
                m_Agent.enabled = true;
                m_Agent.destination = this.info.point;
                animator.Play("Run");
                isNav = true;
            }
            isClickNavMesh = false;
        }
        // 键盘控制角色
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.Space))
        {
            m_Agent.enabled = false;
            rigidbody.isKinematic = false;
            isNav = false;
        }
        if (isNav)
        {
            if (Vector3.Distance(transform.position, m_Agent.destination) < 0.2f)
            {
                animator.Play("Idle");
            }
        }
    }
}
