using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereTransform : MonoBehaviour
{
    public bool isRotate;
    public float rotationSpeed = 200.0f;
    private float x;
    private float y;
    private Vector3 rotateValue;
    private void Update()
    {
        // if (isRotate == true)
        // {
        //     RotateOnEditor();
        // }
        RotateOnEditor();
    }

    void RotateOnEditor()
    {
        if (Input.GetMouseButton(0))
        {
            x += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            y -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
            //y = Mathf.Clamp(y, -90f, 90f);  // Limit vertical rotation
            transform.rotation = Quaternion.Euler(-y, -x, 0);
        }
    }
}
