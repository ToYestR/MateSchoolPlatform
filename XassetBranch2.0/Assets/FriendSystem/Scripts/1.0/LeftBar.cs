using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FriendSystem
{
    public class LeftBar : MonoBehaviour
    {
        [Header("¿ª¹Ø")]
        [SerializeField] Toggle firstToggle;
        [SerializeField] Toggle secondToggle;
        [SerializeField] Toggle thirdToggle;
        [SerializeField] Toggle fourthToggle;
        [Header("Ãæ°å")]
        [SerializeField] GameObject[] pages;
        private void Start()
        {
            firstToggle.onValueChanged.AddListener(arg =>
            {
                if(arg)
                {
                    ToggleIndex(0);
                }
            });
            secondToggle.onValueChanged.AddListener(arg =>
            {
                if (arg)
                {
                    ToggleIndex(1);
                }
            });
            thirdToggle.onValueChanged.AddListener(arg =>
            {
                if (arg)
                {
                    ToggleIndex(2);
                }
            });
            fourthToggle.onValueChanged.AddListener(arg =>
            {
                if (arg)
                {
                    ToggleIndex(3);
                }
            });
        }
        private void ToggleIndex(int index)
        {
            for(int i=0;i<pages.Length;i++)
            {
                if(index==i)
                {
                    pages[i].SetActive(true);
                }
                else
                {
                    pages[i].SetActive(false);
                }
            }
        }
    }
}
