using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace FriendSystem
{
    /// <summary>
    /// 学校搜索功能
    /// </summary>
    public class SearchSchool : MonoBehaviour
    {
        [SerializeField] InputField input;
        [SerializeField] GameObject scrollView;
        [SerializeField] Transform parent;
        [SerializeField] Button searchBtn;

        private GameObject prefab;
        private Dictionary<string, GameObject> items = new Dictionary<string, GameObject>();    // 展示内容
        private Queue<GameObject> objects = new Queue<GameObject>();                            // 对象池
        private void Start()
        {
            prefab = parent.GetChild(0).gameObject;
            prefab.gameObject.SetActive(false);
            searchBtn.onClick.AddListener(OnSubmit);

        }
        private void OnSubmit()
        {
            List<string> id = new List<string>();
            foreach (var item in items)
            {
                SetItem(item.Value);
            }
            items.Clear();
            if (string.IsNullOrWhiteSpace(input.text))
            {
                foreach (var item in FriendSystemCanvas.getInstance.universities)
                {
                    GameObject go = GetItem();
                    go.transform.Find("Item Label").GetComponent<Text>().text = item.schoolName;
                    items.Add(item.schoolName, go);
                }
            }
            else
            {
                foreach (var item in FriendSystemCanvas.getInstance.universities)
                {
                    if (item.schoolName.Contains(input.text))
                    {
                        GameObject go = GetItem();
                        go.transform.Find("Item Label").GetComponent<Text>().text = item.schoolName;
                        items.Add(item.schoolName, go);
                    }
                }
            }
            scrollView.gameObject.SetActive(true);
        }
        public void OnToggleChange(Text target)
        {
            scrollView.gameObject.SetActive(false);
            string schoolName = target.text;
            input.text = schoolName;
            Debug.Log(schoolName);
        }

        private GameObject GetItem()
        {
            GameObject go = null;
            if (objects.Count > 0)
            {
                go = objects.Dequeue();
            }
            else
            {
                go = Instantiate(prefab, parent);
            }
            go.SetActive(true);
            go.transform.SetAsLastSibling();
            return go;
        }
        private void SetItem(GameObject item)
        {
            item.gameObject.SetActive(false);
            objects.Enqueue(item);
        }
    }
}