using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FriendSystem
{
    public class PopUpApplyWindow : MonoBehaviour
    {
        [SerializeField] FriendApplyWindow applyWindow;
        [SerializeField] FriendApplyVerifyWindow friendApplyVerify;

        public void ShowApplyWindow(bool state=true)
        {
            if (state)
            {
                applyWindow.Show();
            }
            else
            {
                applyWindow.Close();
            }
        }
        public void ShowApplyVerify(ApplyFriend apply)
        {
            friendApplyVerify.Show(apply);
        }
    }
}
