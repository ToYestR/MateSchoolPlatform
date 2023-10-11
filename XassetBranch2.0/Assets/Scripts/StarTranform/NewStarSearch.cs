using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class NewStarSearch : Singleton<NewStarSearch>
{
    // UI组件引用
    public InputField searchInput;
    public GameObject buttonPrefab;
    public Transform canvasChild;
    public GameObject test;
    public GameObject starDetailCanvas;
    private bool isInitialized = false;
    // public bool isUIActive = false;
    public Button searchButton;// 搜索按钮的引用

    [HideInInspector]
    public List<Transform> namedChildren = new List<Transform>();

    private void Start()
    {
        searchButton.onClick.AddListener(OnButtonClickTest);
    }

    public List<Transform> GetNamedChildren(Transform parent)
    {
        List<Transform> result = new List<Transform>();
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (!string.IsNullOrEmpty(child.name))
            {
                result.Add(child);
            }
        }
        return result;
    }

    public void GenerateButtons(List<Transform> children)
    {
        foreach (Transform child in children)
        {
            GameObject newButton = Instantiate(buttonPrefab, canvasChild);
            newButton.name = child.name;  // 假设按钮的名字和子物体的名字一样
            newButton.GetComponentInChildren<Text>().text = child.name;
            newButton.SetActive(false);  // 默认设置为不激活

            Button buttonComponent = newButton.GetComponent<Button>();
            buttonComponent.onClick.AddListener(() => SetCanvasOn(newButton.name));
        }
        test.SetActive(false);
    }

    public void OnSearchButtonClick()
    {
        string searchText = searchInput.text.Trim();

        List<GameObject> matchingButtons = new List<GameObject>();

        // 首先，找出所有匹配的按钮
        for (int i = 0; i < canvasChild.childCount; i++)
        {
            GameObject btn = canvasChild.GetChild(i).gameObject;
            string btnName = btn.name;

            if (btnName.Contains(searchText))
            {
                matchingButtons.Add(btn);
            }
        }

        // 如果搜索文本为空或找到匹配的按钮
        if (string.IsNullOrEmpty(searchText) || matchingButtons.Count > 0)
        {
            for (int i = 0; i < canvasChild.childCount; i++)
            {
                GameObject btn = canvasChild.GetChild(i).gameObject;

                if (string.IsNullOrEmpty(searchText))
                {
                    test.SetActive(true);
                    btn.SetActive(true);
                    // isUIActive = true;
                }
                else
                {
                    test.SetActive(true);
                    btn.SetActive(matchingButtons.Contains(btn));
                    // isUIActive = true;
                }
            }
        }
        else
        {
            // 如果没有找到匹配的按钮，隐藏所有按钮
            for (int i = 0; i < canvasChild.childCount; i++)
            {
                test.SetActive(false);
                canvasChild.GetChild(i).gameObject.SetActive(false);
                // isUIActive = false;
            }
        }
    }

    public void SetCanvasOn(string buttonName)
    {
        foreach (Transform child in transform)
        {
            if (child.name == buttonName)
            {
                StarDetailComponent starDetailComponent = child.GetComponent<StarDetailComponent>();
                if (starDetailComponent != null)
                {
                    starDetailComponent.UITest();
                    NewStartTransform.Instance.AllControl = false;
                    starDetailCanvas.SetActive(true);
                    // isUIActive = true;
                    return;
                }
            }
            else
            {
                Debug.LogWarning("No matching child object found!");
            }
        }
    }

    public void OnButtonClickTest()
    {
        if (!isInitialized)
        {
            InitializeUI();
            isInitialized = true;
        }
    }

    private void InitializeUI()
    {
        namedChildren = GetNamedChildren(transform);
        GenerateButtons(namedChildren);
    }
}
