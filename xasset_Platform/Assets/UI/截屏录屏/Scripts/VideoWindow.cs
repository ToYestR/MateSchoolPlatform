using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoWindow : MonoBehaviour
{
    public Button closeBtn;//�رհ�ť
    public Slider timeSlider;   // ʱ�������
    public Button playBtn;  // ���Ű�ť
    public Sprite[] playTextures;// ������ͣ�л�
    public Text timeTxt;    // ʱ���ı�

    private float waitTime = 1.0f;   // ��ʾ�����ؿؼ�ʱ��
    private VideoPlayer player;// ��Ƶ������
    public UnityEvent OnPlayEvent;
    public UnityEvent OnPauseEvent;
    
    public UnityEvent<string> OnJumpEvent;
    private RawImage rawImage;  // ��ʾ��
    private bool isControl;     // �Ƿ����ڿ�����Ƶ�ؼ�
    private void Awake()
    {
        player = GetComponent<VideoPlayer>();
        rawImage = GetComponent<RawImage>();
        playBtn.GetComponent<Image>().sprite = playTextures[0];
        timeSlider.onValueChanged.AddListener(OnSliderValueChange);

        closeBtn.onClick.AddListener(Close);
        playBtn.onClick.AddListener(Play);
    }
    
    private void Play()
    {
        if (player.isPlaying)
        {
            player.Pause();
            OnPauseEvent.Invoke();
            playBtn.GetComponent<Image>().sprite = playTextures[1];
        }
        else
        {
            player.Play();
            OnPlayEvent.Invoke();
            playBtn.GetComponent<Image>().sprite = playTextures[0];
        }
    }

    private void Close()
    {
        Destroy(gameObject);
    }
    public void OnSliderHandleMouseDown()
    {
        isControl = true;
    }
    public void OnSliderHandleMouseUp()
    {
        isControl = false;
        player.time = timeSlider.value;
        player.Play();
        OnPlayEvent.Invoke();
        playBtn.GetComponent<Image>().sprite = playTextures[0];
        OnJumpEvent.Invoke(player.time.ToString());
    }
    public void OnSliderValueChange(float value)
    {
        if (Math.Abs(timeSlider.maxValue - timeSlider.value) < 0.05f && !isControl)
        {
            player.Pause();
            OnPauseEvent.Invoke();
            
            playBtn.GetComponent<Image>().sprite = playTextures[1];
        }
    }
    public void Show(string url)
    {
        player.url = url;
        RenderTexture texture = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
        texture.Create();
        player.targetTexture = texture;
        player.Play();
        OnPlayEvent.Invoke();
        playBtn.GetComponent<Image>().sprite = playTextures[0];
        rawImage.texture = texture;
    }
    private void FixedUpdate()
    {
        string curTime = GameUtil.Num2Time((int)player.time);
        string maxTime = GameUtil.Num2Time((int)player.length);
        timeTxt.text = curTime + "/" + maxTime;
        timeSlider.maxValue = (float)player.length;
        if (!isControl)
        {
            timeSlider.value = (float)player.time;
        }
        waitTime -= Time.deltaTime;
        if (waitTime<0)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        if(Input.GetAxisRaw("Mouse Y") != 0 || Input.GetAxisRaw("Mouse X") != 0)
        {
            waitTime = 1.0f;
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
