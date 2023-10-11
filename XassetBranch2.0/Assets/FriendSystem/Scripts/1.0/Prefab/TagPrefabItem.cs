using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FriendSystem
{
    public class TagPrefabItem : MonoBehaviour
    {
        public Toggle toggle;
        [SerializeField] Text tagName;

        public MyTag target;
        public void Show(MyTag tag)
        {
            target = tag;
            tagName.text = target.name;
            toggle.isOn = false;
        }
    }
}
