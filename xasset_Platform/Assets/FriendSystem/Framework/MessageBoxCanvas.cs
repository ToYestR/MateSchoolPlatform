/************************************************
 * Author       :   XXY
 * Blog         :   https://www.cnblogs.com/jzyl
 * Gitee        :   https://gitee.com/xiaobaichong
 * Email        :   1047185209@QQ.com
 * FileName     :   MessageBoxCanvas.cs
 * CreateData   :   2023/8/18 12:40:04
 * UnityVersion :   2021.3.20f1c1
 * Description  :   MessageBox 消息弹窗
************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MessageBoxCanvas : MonoBehaviour
{
    public class MessageBox
    {
        private Button okBtn;
        private Button cancelBtn;
        private Button closeBtn;
        private Button sparkleBtn;
        private Text title;
        private TextMeshProUGUI message;
        private RectTransform DragBg;
        private UnityAction<MessageBox> recoveryAction;
        private AudioClip audio;
        private float scaleTime;
        private float sparkleTime;
        public Transform target;
        private Vector2 size;
        private Vector2 screenSize;
        public MessageBox(Transform target, string msg, string title, AudioClip audioClip, UnityAction<MessageBox> recoveryAction, float scaleTime, float sparkleTime, string okTxt, UnityAction okAction, string cancelTxt, UnityAction cancelAction = null)
        {
            Init(target);
            this.recoveryAction = recoveryAction;
            this.scaleTime = scaleTime;
            this.sparkleTime = sparkleTime;

            this.title.text = title;
            this.message.text = msg;
            this.audio = audioClip;
            closeBtn.onClick.AddListener(OnClose);
            sparkleBtn.onClick.AddListener(() => sparkleBtn.StartCoroutine(SparkleAnimation()));
            if (okAction == null)
            {
                okBtn.gameObject.SetActive(false);
            }
            else
            {
                okBtn.gameObject.SetActive(true);
                okBtn.transform.GetChild(0).GetComponent<Text>().text = okTxt;
                okBtn.onClick.AddListener(okAction);
                okBtn.onClick.AddListener(OnClose);
            }
            if (cancelAction == null)
            {
                cancelBtn.gameObject.SetActive(false);
            }
            else
            {
                cancelBtn.gameObject.SetActive(true);
                cancelBtn.transform.GetChild(0).GetComponent<Text>().text = cancelTxt;
                cancelBtn.onClick.AddListener(cancelAction);
                cancelBtn.onClick.AddListener(OnClose);
            }
            DragBg.GetComponent<MonoBehaviour>().StartCoroutine(ScaleAnimation());
        }
        private void Init(Transform transform)
        {
            this.target = transform;
            okBtn = target.Find("DragBG/BottomBar/OkBtn").GetComponent<Button>();
            cancelBtn = target.Find("DragBG/BottomBar/cancelBtn").GetComponent<Button>();
            closeBtn = target.Find("DragBG/TopBar/closeBtn").GetComponent<Button>();
            sparkleBtn = target.GetComponent<Button>();
            message = target.Find("DragBG/ContentView/message").GetComponent<TextMeshProUGUI>();
            title = target.Find("DragBG/TopBar/title").GetComponent<Text>();
            DragBg = target.Find("DragBG").GetComponent<RectTransform>();

            DragBg.GetComponent<Outline>().enabled = false;
            size = target.GetComponent<RectTransform>().rect.size;
            screenSize = new Vector2(Screen.width, Screen.height);

            EventTrigger trigger;
            if (!DragBg.TryGetComponent<EventTrigger>(out trigger))
            {
                trigger = DragBg.gameObject.AddComponent<EventTrigger>();
            }
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Drag;
            entry.callback.AddListener(OnDrag);
            trigger.triggers.Add(entry);
        }

        private void OnDrag(BaseEventData arg)
        {
            PointerEventData pointer = arg as PointerEventData;
            Vector2 pos = DragBg.anchoredPosition + pointer.delta * size / screenSize;
            pos.x = Mathf.Clamp(pos.x, (-size.x + DragBg.rect.width) / 2, (size.x - DragBg.rect.width) / 2);
            pos.y = Mathf.Clamp(pos.y, (-size.y + DragBg.rect.height) / 2, (size.y - DragBg.rect.height) / 2);
            DragBg.anchoredPosition = pos;
        }

        private void OnClose()
        {
            DragBg.anchoredPosition = Vector2.zero;
            okBtn.onClick.RemoveAllListeners();
            cancelBtn.onClick.RemoveAllListeners();
            closeBtn.onClick.RemoveAllListeners();
            sparkleBtn.onClick.RemoveAllListeners();
            DragBg.GetComponent<EventTrigger>().triggers.Clear();
            recoveryAction?.Invoke(this);
        }
        private IEnumerator ScaleAnimation()
        {
            float temp = 0;
            while (temp < scaleTime)
            {
                temp = Mathf.Clamp(temp + Time.deltaTime, 0, scaleTime);
                DragBg.transform.localScale = Vector3.one * (temp / scaleTime);
                yield return null;
            }
        }
        private IEnumerator SparkleAnimation()
        {
            if (audio) AudioSource.PlayClipAtPoint(audio, Vector3.zero);
            float temp = 0;
            Outline outline = DragBg.GetComponent<Outline>();
            while (temp < sparkleTime)
            {
                temp = Mathf.Clamp(temp + Time.deltaTime, 0, sparkleTime);
                yield return null;
                yield return null;
                yield return null;
                outline.enabled = !outline.enabled;
            }
            outline.enabled = false;
        }
    }

    [SerializeField, Range(0.1f, 1)] float scaleTime = 0.2f;        // 缩放时间
    [SerializeField, Range(0.1f, 1)] float sparkleTime = 0.4f;      // 闪烁时间
    [SerializeField] AudioClip audioClip;                                   // 警报提示
    private GameObject prefab;
    private List<MessageBox> boxes = new List<MessageBox>();    // 消息盒子
    private Queue<GameObject> pool = new Queue<GameObject>();   // 对象池
    public static MessageBoxCanvas getInstance;
    [RuntimeInitializeOnLoadMethod]
    private static void Initialize()
    {
        Debug.Log("fadfadfs");
        GameObject go = Resources.Load<GameObject>("MessageBoxCanvas");
        Instantiate(go);
    }
     private void Awake()
    {
        getInstance = this;
        DontDestroyOnLoad(gameObject);
        prefab = transform.GetChild(0).gameObject;
        prefab.SetActive(false);
    }

    private void Update()
    {

    }
    private GameObject GetPoolObj()
    {
        GameObject obj = null;
        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        else
        {
            obj = Instantiate(prefab, transform);
        }
        obj.SetActive(true);
        obj.transform.SetAsLastSibling();
        return obj;
    }
    private void OnMessageBoxRecovery(MessageBox messageBox)
    {
        boxes.Remove(messageBox);
        GameObject go = messageBox.target.gameObject;
        go.SetActive(false);
        pool.Enqueue(go);
    }
    public void Show(string msg, string title)
    {
        Show(msg, title, "", null);
    }
    public void Show(string msg, string title, string okTxt, UnityAction okAction)
    {
        Show(msg, title, okTxt, okAction, "", null);
    }
    public void Show(string msg, string title, string okTxt, UnityAction okAction, string cancelTxt, UnityAction cancelAction = null)
    {
        MessageBox box = new MessageBox(GetPoolObj().transform, msg, title, audioClip, OnMessageBoxRecovery, scaleTime, sparkleTime, okTxt, okAction, cancelTxt, cancelAction);
        boxes.Add(box);
    }
}