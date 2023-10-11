using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;
using System;

namespace FriendSystem
{
    /// <summary>
    /// ����ع���
    /// </summary>
    public class PoolManager : MonoBehaviour
    {   
        public IObjectPool<GameObject> pool;    // ��
        [HideInInspector]public List<GameObject> busyObjects;    // �뿪�صĶ���
        public Transform content;
        public GameObject prefab;
        private bool collectionChecks;

        protected virtual void Awake()
        {
            prefab.SetActive(false);
            pool= new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, 10, 100);
            busyObjects = new List<GameObject>();
        }

        private void OnDestroyPoolObject(GameObject obj)    // �ﵽ������������ٲ��������
        {
            Destroy(obj);
        }

        private void OnReturnedToPool(GameObject obj)   // ���ض��󵽳���
        {
            obj.SetActive(false);
            busyObjects.Remove(obj);
        }

        private void OnTakeFromPool(GameObject obj) // �ӳ��л�ȡ����
        {
            obj.SetActive(true);
            obj.transform.SetAsLastSibling();
            busyObjects.Add(obj);
        }

        private GameObject CreatePooledItem()   // ��������
        {
            var obj = Instantiate(prefab, content);
            obj.SetActive(true);
            return obj;
        }
        public void Clear()
        {
            while(busyObjects.Count>0)
            {
                pool.Release(busyObjects[0]);
            }
        }
    }
}
