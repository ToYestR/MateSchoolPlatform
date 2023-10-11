using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//挂在在控制器上，用于控制鼠标点击事件
public class MouseManager : Singleton<MouseManager>
{
    public event Action OnMouseClickedStar;
    RaycastHit hitInfo;
    // Update is called once per frame
    void Update()
    {
        //SetStarUI();
        //MouseControl();
    }
    void SetStarUI()
    {
        if (Camera.main)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.collider.gameObject.name.Contains("PlaneTest"))
                {
                    hitInfo.collider.gameObject.GetComponent<StarDetailComponent>().UITest();
                }
            }
        }
    }
    void MouseControl()
    {
        if (Input.GetMouseButtonDown(0) && hitInfo.collider != null)
        {
            if (hitInfo.collider.gameObject.name.Contains("PlaneTest"))
            {
                OnMouseClickedStar?.Invoke();
            }
        }
    }
}
