using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
public class Player : MonoBehaviour
{
    public RawImage emoji;                          // 表情
    public TMP_Text m_ChatText;                 //我的聊天记录
    public static bool isSeatDown = false;        // 是否入座了
    private IEnumerator Start()
    {
        yield return null;
        MoveTo(Global.current_pos);
    }

    public void MoveTo(Vector3 pos)// 移动到指定位置
    {
        Transform person = transform.GetChild(0);
        person.GetComponent<Rigidbody>().isKinematic = true;
        //person.GetComponent<Rigidbody>().MovePosition(pos);
        Rigidbody rigidbody = person.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.GetComponent<Animator>().Play("Idle");
        person.localPosition = pos;
        StartCoroutine(Wait(rigidbody));
    }
    private IEnumerator Wait(Rigidbody rigidbody)
    {
        yield return null;
        yield return null;
        yield return null;
        rigidbody.isKinematic = false;
    }
}
