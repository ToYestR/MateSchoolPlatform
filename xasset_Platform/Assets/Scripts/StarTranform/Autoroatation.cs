using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

//老版本的星球旋转脚本，已经弃用
public class Autoroatation : MonoBehaviour
{
    public float duration = 3.0f; // 动画持续时间
    public float moveToCameraDuration = 2.0f; // 移动到摄像机前面的动画持续时间
    public float distanceFromCamera = 2.0f; // 子物体在摄像机前面的距离
    private GameObject selectedObject; // 被选择的物体
    private Vector3 selectedObjectOriginalLocalPosition; // 被选择的物体的原始本地位置
    private bool isAnimating = false; // 是否正在播放动画
    private bool isClick = false;
    public SphereTransform[] sphereTransform;

    // Start is called before the first frame update
    void Start()
    {
        Autoroatate();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAnimating && !isClick) // 如果鼠标左键被按下
        {
            // 检查鼠标是否点击了一个物体
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.CompareTag("Star"))
            {
                // 鼠标点击了一个物体，保存这个物体和它的原始本地位置
                selectedObject = hit.transform.gameObject;
                selectedObjectOriginalLocalPosition = selectedObject.transform.localPosition;

                // 移动被选择的物体到摄像机前面
                MoveChildToFrontOfCamera();
            }
        }

        // 如果按下空格键，子物体重新开始公转
        if (Input.GetKeyDown(KeyCode.Space) && !isAnimating)
        {
            RestartOrbit();
        }
    }

    private void Autoroatate()
    {
        transform.DORotate(new Vector3(0, 0, 360), duration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear) // 线性动画
            .SetLoops(-1, LoopType.Incremental); // 设置循环，-1表示无限循环，LoopType.Incremental表示每次循环都是在上一次的基础上继续旋转
    }

    private void MoveChildToFrontOfCamera()
    {
        isAnimating = true; // 设置开始播放动画
        isClick = true;
        selectedObject.transform.SetParent(null); // 取消子物体与父物体的关联
        Vector3 targetPosition = Camera.main.transform.position + Camera.main.transform.forward * distanceFromCamera; // 计算目标位置
        selectedObject.transform.DOMove(targetPosition, moveToCameraDuration).OnComplete(() => { isAnimating = false; foreach (var tr in sphereTransform) tr.isRotate = true; }); // 移动子物体到目标位置
    }

    public void RestartOrbit()
    {
        isAnimating = true;
        selectedObject.transform.SetParent(transform); // 重新设置子物体与父物体的关联
        selectedObject.transform.DOLocalMove(selectedObjectOriginalLocalPosition, moveToCameraDuration).OnComplete(() => { isAnimating = false; isClick = false; foreach (var tr in sphereTransform) tr.isRotate = false; }); // 移动子物体回到原始位置
    }
}
