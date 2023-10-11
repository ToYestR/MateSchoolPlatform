/************************************************
 * Author       :   XXY
 * Blog         :   https://www.cnblogs.com/jzyl
 * Gitee        :   https://gitee.com/xiaobaichong
 * Email        :   1047185209@QQ.com
 * FileName     :   MessagePopup.cs
 * CreateData   :   2023/7/17 22:55:50
 * UnityVersion :   2021.3.20f1c1
 * Description  :   弹窗气泡框架
************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;
using System.Linq;
using TMPro;


public class MessageBubbleCanvas : MonoBehaviour
{
    public class Bubble         // 泡泡
    {
        // 泡泡对象
        public Bubble(Image target, float showTime, float scaleTime, float fadeTime, bool isTMP, AnimationCurve scaleCurve, Action<Bubble> action)
        {
            this.target = target;
            this.showTime = showTime;
            this.scaleTime = scaleTime;
            this.fadeTime = fadeTime;
            this.ERecovery = action;
            this.isTMP = isTMP;
            this.scaleCurve = scaleCurve;
            this.target.StartCoroutine(ScaleAnimation());
        }
        private float showTime;
        private float scaleTime;
        private float fadeTime;
        private bool isTMP;
        private AnimationCurve scaleCurve;  // 缩放曲线

        public Image target;    // 泡泡目标对象
        private Action<Bubble> ERecovery;    // 回收时间

        private IEnumerator ScaleAnimation()        // 缩放动画
        {
            float temp = scaleTime;
            while (scaleTime > 0)
            {
                scaleTime = Mathf.Clamp(scaleTime - Time.deltaTime, 0, temp);
                target.transform.localScale = Vector3.one * scaleCurve.Evaluate(1 - scaleTime / temp);
                yield return null;
            }
            target.StartCoroutine(FadeAnimation());
        }
        private IEnumerator FadeAnimation()         // 消失渐变
        {
            while (showTime > 0)
            {
                showTime -= Time.deltaTime;
                yield return null;
            }
            float temp = fadeTime;
            Text text = target.transform.GetChild(0).GetComponent<Text>();
            TextMeshProUGUI textPro = target.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            Color imgColor = target.color;
            Color txtColor = text.color;
            Color proColor = textPro.color;
            while (fadeTime > 0)
            {
                fadeTime = Mathf.Clamp(fadeTime - Time.deltaTime, 0, temp);
                imgColor.a = fadeTime / temp;
                txtColor.a = imgColor.a;
                proColor.a = txtColor.a;
                target.color = imgColor;
                text.color = txtColor;
                textPro.color = proColor;
                yield return null;
            }
            ERecovery?.Invoke(this);
        }
    }

    [Header("边距")]
    [SerializeField] float txtCapWidth = 50;                                        // 背景与文字水平内间距
    [SerializeField] float bubleSpace = 10;                                         // 泡泡之间垂直间距

    [Header("时间")]
    [SerializeField, Range(0, 5)] float showTime = 4f;                    // 正常显示时间
    [SerializeField, Range(0, 2)] float scaleTime = 0.2f;                 // 缩放时间
    [SerializeField, Range(0, 2)] float fadeTime = 0.2f;                  // 渐变时间
    [SerializeField, Range(0, 2)] float moveTime = 0.2f;                  // 移动时间

    [Header("位置")]
    [SerializeField, Range(-0.5f, 0.5f)] float vertivalPos = 0.15f;           // 垂直显示位置

    [Header("动画曲线")]
    [SerializeField] AnimationCurve scaleCurve;                                     // 弹出缩放动画
    [SerializeField] new AudioClip audio;                                           // 弹窗提示音

    ObjectPool<GameObject> pool = null;
    GameObject prefab;
    List<Bubble> bubbles = new List<Bubble>();      // 泡泡集合
    float windowHeight;
    float cellHight;

    public static MessageBubbleCanvas getInstance;
    [RuntimeInitializeOnLoadMethod]
    private static void Initialize()
    {
        Debug.Log("fadfadfs");
        GameObject go = Resources.Load<GameObject>("MessageBubbleCanvas");
        Instantiate(go);
    }
    private void Awake()
    {
        getInstance = this;
        DontDestroyOnLoad(gameObject);
        pool = new ObjectPool<GameObject>(onCreatePopup, OnGetPopup, OnReleasePopup);
        prefab = transform.GetChild(0).gameObject;
        prefab.SetActive(false);
        windowHeight = GetComponent<RectTransform>().rect.height;
        cellHight = prefab.GetComponent<RectTransform>().rect.height + bubleSpace;
    }

    private void OnReleasePopup(GameObject obj)
    {
        obj.SetActive(false);
        Image image = obj.GetComponent<Image>();
        Text text = obj.transform.GetChild(0).GetComponent<Text>();
        TextMeshProUGUI textPro = obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        Color color = image.color;
        color.a = 1;
        image.color = color;
        color = text.color;
        color.a = 1;
        text.color = color;
        color = textPro.color;
        color.a = 1;
        textPro.color = color;
    }

    private void OnGetPopup(GameObject obj)
    {
        obj.transform.localScale = Vector3.one;
        obj.transform.SetAsLastSibling();
        obj.SetActive(true);
    }

    private GameObject onCreatePopup()
    {
        GameObject popup = Instantiate(prefab, transform);
        popup.SetActive(true);
        return popup;
    }
    private void OnBubbleRecovery(Bubble bubble) // 回收通知
    {
        bubbles.Remove(bubble);
        pool.Release(bubble.target.gameObject);
    }
    /// <summary>
    /// 显示泡泡弹幕
    /// </summary>
    /// <param name="msg">显示内容</param>
    /// <param name="isTMP">是否使用TextMeshProUGUI显示（默认使用Text）</param>
    public void Show(string msg, bool isTMP = false)
    {
        msg = msg.Replace("\n", "");
        GameObject popup = pool.Get();
        RectTransform rect = popup.GetComponent<RectTransform>();
        Text txt = popup.transform.GetChild(0).GetComponent<Text>();
        TextMeshProUGUI txtPro = popup.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        txt.gameObject.SetActive(!isTMP);
        txtPro.gameObject.SetActive(isTMP);
        rect.anchoredPosition = Vector2.up * vertivalPos * windowHeight;
        Bubble bubble = null;
        if (isTMP)
        {
            txtPro.text = msg;
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, txtPro.preferredWidth + txtCapWidth * 2);
        }
        else
        {
            txt.text = msg;
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, txt.preferredWidth + txtCapWidth * 2);
        }
        bubble = new Bubble(popup.GetComponent<Image>(), showTime, scaleTime, fadeTime, isTMP, scaleCurve, OnBubbleRecovery);
        for (int i = 0; i < bubbles.Count; i++)
        {
            var b = bubbles[i];
            b.target.StopCoroutine("MoveYAnimation");
            b.target.StartCoroutine(MoveYAnimation(b.target.GetComponent<RectTransform>(), bubbles.Count - i));
        }
        bubbles.Add(bubble);
        if (audio) AudioSource.PlayClipAtPoint(audio, Vector3.zero);
    }
    private IEnumerator MoveYAnimation(RectTransform target, int index)        // 向上移动
    {
        float targetPos = vertivalPos * windowHeight + cellHight * index;
        float temp = moveTime;
        while (temp > 0)
        {
            temp = Mathf.Clamp(temp - Time.deltaTime, 0, moveTime);
            target.anchoredPosition = Vector2.Lerp(Vector2.up * targetPos, target.anchoredPosition, temp / moveTime);
            yield return null;
        }
    }
}