using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchButton : MonoBehaviour
{
    public void OnSwitch()
    {
        SwitchPanel.instance.SwitchPaner();
    }
    public void OnSwitchSuper()
    {
        SwitchPanel.instance.SwitchPanerSuper();
    }

    public void OnPanel()
    {
        SwitchUI.instance.OnPanel();
    }

    public void OffPanel()
    {
        SwitchUI.instance.OffPanel();
    }

    public void Wait()
    {
        StartCoroutine(SwitchUI.instance.WaitForSeconds(1.5f));
    }
}
