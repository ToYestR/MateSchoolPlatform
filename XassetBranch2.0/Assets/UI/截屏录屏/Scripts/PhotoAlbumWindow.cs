using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class PhotoAlbumWindow : MonoBehaviour
{
    [SerializeField] List<GameObject> toggleHideObjs;
    private void OnEnable()
    {

        toggleHideObjs.ForEach(obj => obj.SetActive(false));

        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
    }
    public void CloseWindow()
    {
        gameObject.SetActive(false);
        toggleHideObjs.ForEach(obj => obj.SetActive(true));
    }
}
