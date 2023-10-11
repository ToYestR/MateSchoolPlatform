using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//挂载在父物体上的，用于实例化预制体，一般用于星球的生成，在游戏中不需要，只在编辑器中有效
public class InstantiateOnVertices : MonoBehaviour
{
    //public List<GameObject> prefabList = new List<GameObject>();
    public GameObject prefabToInstantiate;
    public GameObject prefabStar;
    GameObject clonedObject;
    public Camera mainCamera;
    public List<Transform> instantiatedPrefabs = new List<Transform>(); // 存储所有实例化的预制体


    private void Awake()
    {
        // mainCamera = Camera.main;
    }

    private void Start()
    {
    }

    private void Update()
    {
#if UNITY_EDITOR
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     InstantiateStarts();
        // }
#endif
        foreach (Transform prefabTransform in instantiatedPrefabs)
        {
            if (Camera.main)
            {
                Vector3 directionToCamera = Camera.main.transform.position - prefabTransform.position;
                Quaternion faceCameraRotation = Quaternion.LookRotation(directionToCamera);
                prefabTransform.rotation = faceCameraRotation;
            }
        }
    }

    public void InstantiateStarts()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        if (meshFilter != null && meshFilter.sharedMesh != null)
        {
            Vector3[] vertices = meshFilter.sharedMesh.vertices;
            //Quaternion rotation = Quaternion.Euler(-90, 0, 0);  // 定义旋转

            // 遍历顶点并实例化预制体
            for (int i = 0; i < vertices.Length; i++)
            {
                // 转换到世界坐标
                Vector3 worldPos = transform.TransformPoint(vertices[i]);
                Vector3 directionToCamera = mainCamera.transform.position - worldPos;
                Quaternion faceCameraRotation = Quaternion.LookRotation(directionToCamera);

                //GameObject selectedPrefab = prefabList[Random.Range(0, prefabList.Count)];

                // 在该位置实例化预制体
                GameObject instantiatedPrefab = Instantiate(prefabToInstantiate, worldPos, faceCameraRotation);
                instantiatedPrefab.transform.SetParent(prefabStar.transform);
                //instantiatedPrefabs.Add(instantiatedPrefab.transform); // 添加到List
            }

            clonedObject = prefabStar;
            // 将克隆的物体设置为PrefabHolder的子物体
            // clonedObject.transform.SetParent(prefabStar.transform);

            // 重置克隆物体的变换
            clonedObject.transform.localPosition = Vector3.zero;
            clonedObject.transform.localRotation = Quaternion.identity;
            clonedObject.transform.localScale = Vector3.one;

            // 将克隆物体设为预制体
            UnityEditor.PrefabUtility.SaveAsPrefabAsset(clonedObject, "Assets/Prefabs/StarTransform/Prefab.prefab");

            // 销毁克隆物体
            Destroy(clonedObject);
        }
        else
        {
            Debug.LogError("No MeshFilter or Mesh found!");
        }
    }
}
