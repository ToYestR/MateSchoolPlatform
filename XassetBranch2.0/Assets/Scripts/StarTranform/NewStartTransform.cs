using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//挂载在父物体上的，用于控制星球的旋转
public class NewStartTransform : Singleton<NewStartTransform>
{
    public float rotationSpeed = 0.5f; // 鼠标拖拽时的旋转速度
    public float autoRotateSpeed = 40f; // 自动旋转的速度

    private Vector3 lastMousePos;
    private bool isDragging = false; // 标记是否正在拖拽
    public bool AllControl = true; // 标记是否正在自动旋转

    private void Update()
    {
        if (AllControl == true)
        {
            // 鼠标左键按下
            if (Input.GetMouseButtonDown(0))
            {
                isDragging = true;
                lastMousePos = Input.mousePosition;
            }
            // 鼠标左键持续按下（拖动中）
            else if (Input.GetMouseButton(0) && isDragging)
            {
                RotateWithDrag();
            }
            // 鼠标左键放开
            else if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }

            // 如果没有拖拽，则自动旋转
            if (!isDragging)
            {
                transform.Rotate(0, autoRotateSpeed * Time.deltaTime, 0, Space.World);
            }
        }
    }

    // 根据鼠标拖动来旋转球体
    private void RotateWithDrag()
    {
        Vector3 delta = Input.mousePosition - lastMousePos;
        transform.Rotate(Vector3.down * delta.x * rotationSpeed, Space.World);
        transform.Rotate(Vector3.right * delta.y * rotationSpeed, Space.World);
        lastMousePos = Input.mousePosition;
    }
}
