using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XAsset
{
    public class CustomToggle : MonoBehaviour
    {
        Toggle toggle;
        private void Awake()
        {
            toggle = GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(OnToggleChange);
            OnToggleChange(toggle.isOn);
        }
        private void OnToggleChange(bool arg)
        {
            if(arg)
            {
                toggle.transform.GetChild(0).GetComponent<Image>().color = Color.gray;
                toggle.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Color.white;
                toggle.transform.GetChild(0).GetChild(1).GetComponent<Image>().color = Color.white;
            }
            else
            {
                toggle.transform.GetChild(0).GetComponent<Image>().color = Color.white;
                toggle.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Color.gray;
                toggle.transform.GetChild(0).GetChild(1).GetComponent<Image>().color = Color.gray;
            }
        }
    }
}
