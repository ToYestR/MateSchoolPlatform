using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPanel : MonoBehaviour
{
    public static SwitchPanel instance;
    public GameObject AddResource;
    public GameObject AddResourceSuper;
    private void Awake()
    {
        instance = this;
    }
    public void SwitchPaner()
    {
        AddResource.SetActive(true);
        AddResourceSuper.SetActive(false);
    }

    public void SwitchPanerSuper()
    {
        AddResource.SetActive(false);
        AddResourceSuper.SetActive(true);
    }
}
