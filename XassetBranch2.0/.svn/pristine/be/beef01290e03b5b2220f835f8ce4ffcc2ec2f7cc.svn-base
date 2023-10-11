using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchUI : MonoBehaviour
{
    public static SwitchUI instance;
    public GameObject UIPanel;
    public GameObject successPicture;
    private void Awake()
    {
        instance = this;
    }

    public void OnPanel()
    {
        UIPanel.SetActive(true);
    }

    public void OffPanel()
    {
        UIPanel.SetActive(false);
    }

    public IEnumerator WaitForSeconds(float seconds)
    {
        successPicture.SetActive(true);
        yield return new WaitForSeconds(seconds);
        successPicture.SetActive(false);
    }
}
