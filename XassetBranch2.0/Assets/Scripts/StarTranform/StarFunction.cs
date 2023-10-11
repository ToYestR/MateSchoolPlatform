using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//挂载在子物体上的，用于显示星球的详细信息UI，和控制UI的显示和隐藏
public class StarFunction : MonoBehaviour
{
    public GameObject starDetailCanvas;
    public Button closeButton;


    private void Start()
    {
        //yzl临时注释

        //MouseManager.Instance.OnMouseClickedStar += OnMouseClickedStar;
        closeButton.onClick.AddListener(CloseCanvas);
    }

    //TODO:后续需要改成OnEnable和OnDisable
    // private void OnEnable()
    // {
    //     MouseManager.Instance.OnMouseClickedStar += OnMouseClickedStar;

    // }

    // private void OnDisable()
    // {
    //     MouseManager.Instance.OnMouseClickedStar -= OnMouseClickedStar;
    // }

    private void OnMouseDown()
    {
        NewStartTransform.Instance.AllControl = false;
        starDetailCanvas.SetActive(true);
    }

    public void CloseCanvas()
    {
        starDetailCanvas.SetActive(false);
        NewStartTransform.Instance.AllControl = true;

        //if (ActivateAndMoveObject.Instance.isUIActive == false)
        //    NewStartTransform.Instance.AllControl = true;
        //else
        //    NewStartTransform.Instance.AllControl = false;
    }
}
