using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class PhotoAlbumWindow : MonoBehaviour
{
    public static PhotoAlbumWindow getInstance;
    [SerializeField] List<GameObject> toggleHideObjs;

    private void Awake()
    {
        if (FindObjectsOfType<PhotoAlbumWindow>(true).Length > 1)
        {
            Destroy(gameObject);
        }
        getInstance = this;
        //DontDestroyOnLoad(this);
        CloseWindow();
    }
    public void Show()
    {
        GetComponent<Image>().enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
        toggleHideObjs.ForEach(obj => obj.SetActive(false));
    }
    public void CloseWindow()
    {
        GetComponent<Image>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        toggleHideObjs.ForEach(obj => obj.SetActive(true));
    }
}
