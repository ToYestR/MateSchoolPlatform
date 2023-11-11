using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 过渡场景
/// </summary>
public class SZ_SceneLoadingControl : MonoBehaviour
{
    private Slider slider;     // 进度条
    private int curProgressVaule = 0;
    private AsyncOperation asyncOperation;
    public Text Pecentage_Text;    // 显示进度的文本
    private float timer = 3f;

    void Awake()
    {
        // 将自身对象的slider组件给Slider  属性
        slider = gameObject.GetComponent<Slider>();
    }
    private void Start()
    {
        StartCoroutine("LoadScene");   // 开启协程        
    }

    IEnumerator LoadScene()     // 通过协程加载场景
    {
        while(timer>=0)
        {
            timer-=0.1f;
            slider.value = (3 - timer) / 3f;
            Pecentage_Text.text = "加载进度：" +(int) (slider.value * 100) + "%";
            yield return new WaitForSeconds(0.1f);
        }
        //SceneManager.UnloadSceneAsync("02_Loading");
        //while(true)
        //{
        //    Scene scene =SceneManager.GetActiveScene();
        //}
        //asyncOperation = SceneManager.LoadSceneAsync(Scene_name,LoadSceneMode.Single);//异步跳转到Scene_name场景 
        //asyncOperation.allowSceneActivation = false;//当game场景加载到90%时，不让它直接跳转到game场景。         
        //yield return asyncOperation;
    }


    //void Update()
    //{
    //    if (asyncOperation == null)
    //    {
    //        return;
    //    }
    //    int progressVaule = 0;
    //    if (asyncOperation.progress < 0.9f)
    //    {
    //        progressVaule = (int)asyncOperation.progress * 100;
    //    }
    //    else
    //    {
    //        progressVaule = 1000;
    //    }
    //    if (curProgressVaule < progressVaule)
    //    {
    //        curProgressVaule += 2;
    //    }
    //    Pecentage_Text.text = "加载进度："+curProgressVaule / 10 + "%";

    //    slider.value = curProgressVaule / 1000f;
    //    if (curProgressVaule == 1000)
    //    {
    //        asyncOperation.allowSceneActivation = true;
    //    }
    //}
}
