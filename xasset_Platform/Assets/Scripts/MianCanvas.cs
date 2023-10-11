using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FriendSystem;

namespace XAsset
{
    public class MianCanvas : MonoBehaviour
    {
        [SerializeField] Button openFriend;
        [SerializeField] Button openPersonal;
        [SerializeField] Button openPhotograph;


        private void Start()
        {
            openFriend.onClick.AddListener(GoodFriendManager.getInstance.Show);
            openPersonal.onClick.AddListener(PersonalCenter.getInstance.Show);
            openPhotograph.onClick.AddListener(PhotoAlbumWindow.getInstance.Show);
        }
    }
}
