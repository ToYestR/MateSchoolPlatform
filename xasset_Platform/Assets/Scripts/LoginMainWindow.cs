using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginMainWindow : MonoBehaviour
{
    public Transform[] transforms;
    //public CareerChoice choice;

    private void OnEnable()
    {
        ShowStepIndex(0);
    }

    public void ShowStepIndex(int index)
    {
        for(int i=0;i<transforms.Length;i++)
        {
            if(i==index)
            {
                transforms[i].gameObject.SetActive(true);
            }
            else
            {
                transforms[i].gameObject.SetActive(false);
            }
        }
    }
    //public void ShowSelectSex()// 游客性别选择
    //{
    //    ShowStepIndex(2);
    //    choice.isUserLogin = 1;
    //    choice.Show(1);
    //}
}
