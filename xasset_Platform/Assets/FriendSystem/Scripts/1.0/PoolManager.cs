using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;
using System;

namespace FriendSystem
{
    /// <summary>
    /// 对象池管理
    /// </summary>
    public class PoolManager : MonoBehaviour
    {   
        public IObjectPool<GameObject> pool;    // 池
        [HideInInspector]public List<GameObject> busyObjects;    // 离开池的对象
        public Transform content;
        public GameObject prefab;
        private bool collectionChecks;

        protected virtual void Awake()
        {
            prefab.SetActive(false);
            pool= new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, 10, 100);
            busyObjects = new List<GameObject>();
        }

        private void OnDestroyPoolObject(GameObject obj)    // 达到最大数量将销毁不放入池中
        {
            Destroy(obj);
        }

        private void OnReturnedToPool(GameObject obj)   // 返回对象到池中
        {
            obj.SetActive(false);
            busyObjects.Remove(obj);
        }

        private void OnTakeFromPool(GameObject obj) // 从池中获取对象
        {
            obj.SetActive(true);
            obj.transform.SetAsLastSibling();
            busyObjects.Add(obj);
        }

        private GameObject CreatePooledItem()   // 创建对象
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
