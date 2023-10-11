using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinWithMouse : MonoBehaviour
{
    private bool isClick = false;
    private Vector3 nowPos;
    private Vector3 oldPos;
    public float length = 5.0f;
    private void OnMouseDown()
    {
        isClick = true;
    }

    private void OnMouseUp()
    {
        isClick = false;
    }

    private void Update()
    {
        TransPlayer();
    }

    private void TransPlayer()
    {
        nowPos = Input.mousePosition;
        if (isClick)
        {
            Vector3 offset = nowPos - oldPos;
            if (Mathf.Abs(offset.x) > Mathf.Abs(offset.y) && Mathf.Abs(offset.x) > length)
            {
                transform.Rotate(Vector3.up, -offset.x);
            }
        }
        oldPos = Input.mousePosition;
    }
}
