using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseClickCanvas : MonoBehaviour
{
    public Image image;
    public Sprite[] sprites;
    private IEnumerator Start()
    {
        for(int i=0;i<sprites.Length;i++)
        {
            yield return null;
            yield return null;
            image.sprite = sprites[i];
        }
        yield return null;
        yield return null;
        Destroy(gameObject);
    }
}
