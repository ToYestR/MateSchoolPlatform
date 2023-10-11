using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
namespace School_CJ
{
    public class LargeScreenVideoSwitch : MonoBehaviour
    {

        public VideoPlayer videoPlayer;//视频组件
        public new MeshRenderer renderer;
        public Texture playTex; // 播放
        public bool isStop = false;    // 是否暂停视频播放
        private void Start()
        {
            videoPlayer.Stop();
            isStop = false;
            renderer.material.SetTexture("_SecondTex", playTex);
            renderer.material.SetFloat("_Alpha", 1);
        }
        private void OnMouseDown()// 控制视频播放
        {
            if (GameUtil.MouseInUI()) return;
            isStop = !isStop;
            if (isStop)
            {
                foreach(var obj in FindObjectsOfType<LargeScreenVideoSwitch>())
                {
                    if(obj!=this)
                    {
                        obj.StopVideo();
                        obj.isStop = false;
                    }
                }
                videoPlayer.Play();
                renderer.material.SetTexture("_SecondTex", null);
                renderer.material.SetFloat("_Alpha", 0);
            }
            else
            {
                videoPlayer.Pause();
                renderer.material.SetTexture("_SecondTex", playTex);
                renderer.material.SetFloat("_Alpha", 1);
            }
        }
        public void StopVideo()
        {
            videoPlayer.Pause();
            renderer.material.SetTexture("_SecondTex", playTex);
            renderer.material.SetFloat("_Alpha", 1);
        }
    }
}
