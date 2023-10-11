using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ActivateAndMoveObject : Singleton<ActivateAndMoveObject>
{
    public GameObject objectToActivate; // 您希望激活的物体的引用
    public Transform playerTransform;   // 玩家的Transform
    public GameObject UIPanel;          // 您希望打开的UI面板的引用
    public Button searchButton;         // 搜索按钮的引用
    public Button forkButton;
    public float distanceFromPlayer=0f ; // 物体与玩家的距离
    public float upwardOffset = 0f;
    private bool isActivated = true;
    private bool isSearchActivated = true;
    public bool isUIActive = false;

    public void OnButtonClick()
    {
        if (isActivated)
        {
            isActivated = !isActivated;
            // 激活物体

            playerTransform = GameObject.Find("MySelf").transform;
            objectToActivate.SetActive(true);
            //searchButton.gameObject.SetActive(true);
            NewStartTransform.Instance.AllControl = true;
            // EventHandler.OnSearchButtonEvent(NewStarSearch.Instance.namedChildren);
            // 将物体移动到玩家的正前方10单位的距离
            Vector3 targetPosition = playerTransform.position + playerTransform.forward * distanceFromPlayer + Vector3.up * upwardOffset;
            objectToActivate.transform.position = targetPosition;
        }
        else
        {
            isActivated = !isActivated;
            // 取消激活物体
            objectToActivate.SetActive(false);
            //YZL临时注释
            //searchButton.gameObject.SetActive(false);
            NewStartTransform.Instance.AllControl = false;
        }
    }

    public void OnSearchButtonClick()
    {
        if (isSearchActivated)
        {
            isSearchActivated = !isSearchActivated;
            NewStartTransform.Instance.AllControl = false;
            //打开UI面板
            UIPanel.SetActive(true);
            isUIActive = true;
        }
        else
        {
            isSearchActivated = !isSearchActivated;
            NewStartTransform.Instance.AllControl = true;
            //关闭UI面板
            UIPanel.SetActive(false);
            isUIActive = false;
        }
    }

    public void ClosePanel()
    {
        UIPanel.SetActive(false);
        isSearchActivated = !isSearchActivated;
        NewStartTransform.Instance.AllControl = true;
        isUIActive = false;
    }
}
