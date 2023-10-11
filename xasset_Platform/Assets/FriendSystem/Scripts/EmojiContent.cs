using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace FriendSystem
{
    public class EmojiContent : MonoBehaviour
    {
        [SerializeField] List<Sprite> sprites;
        [ContextMenu("生成表情")]
        private void Generate()
        {
            GameObject go = transform.GetChild(0).gameObject;
            for (int i = 0; i < sprites.Count; i++)
            {
                if (transform.childCount <= i)
                {
                    GameObject game = Instantiate(go, transform);
                    game.GetComponent<Image>().sprite = sprites[i];
                }
                else
                {
                    transform.GetChild(i).GetComponent<Image>().sprite = sprites[i];
                }
            }
        }
    }
}