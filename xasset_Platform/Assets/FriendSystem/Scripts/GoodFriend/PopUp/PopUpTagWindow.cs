using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FriendSystem
{
    public class PopUpTagWindow : MonoBehaviour
    {
        [SerializeField] AddTagWindow addTag;
        [SerializeField] EditTagWindow editTag;

        public void ShowAddTagWindow(MyTag tag,List<string> chatNos=null)
        {
            editTag.Hide();
            addTag.Show(tag,chatNos);
        }
        public void SHowEditTagWindow()
        {
            editTag.Show();
        }
    }
}