using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AvatarButton : MonoBehaviour
{
    /// <summary>
    /// 在Toggle组件的OnValueChanged事件中注册调用
    /// </summary>
    /// <param name="isOn"></param>
    public void OnValueChanged(bool isOn)
    {
        if (isOn)
        {
            if (gameObject.name == "boy" || gameObject.name == "girl")
            {
                AvatarSys._instance.SexChange();
                return;
            }
            string[] names = gameObject.name.Split('-');
            AvatarSys._instance.OnChangePeople(names[0], names[1]);
            // switch (names[0])
            // {
            //     case "pants":
            //         PlayAnimation("item_pants");
            //         break;
            //     case "shoes":
            //         PlayAnimation("item_boots");
            //         break;
            //     case "top":
            //         PlayAnimation("item_shirt");
            //         break;
            //     default:
            //         break;
            // }
        }
    }
    public void SetBoy(bool isOn)
    {
        if (AvatarSys._instance.sexCount == 1) return;
        if (isOn)
        {
            if (gameObject.name == "boy" || gameObject.name == "girl")
            {
                AvatarSys._instance.sexCount = 1;
                AvatarSys._instance.SexChange();
                return;
            }
            string[] names = gameObject.name.Split('-');
            AvatarSys._instance.OnChangePeople(names[0], names[1]);
        }
    }
    public void SetGirl(bool isOn)
    {
        if (AvatarSys._instance.sexCount == 0) return;
        if (isOn)
        {
            if (gameObject.name == "boy" || gameObject.name == "girl")
            {
                AvatarSys._instance.sexCount = 0;
                AvatarSys._instance.SexChange();
                return;
            }
            string[] names = gameObject.name.Split('-');
            AvatarSys._instance.OnChangePeople(names[0], names[1]);
        }
    }

    public void PlayAnimation(string animName)
    {
        Animation anim = GameObject.FindWithTag("Player").GetComponent<Animation>();
        if (!anim.IsPlaying(animName))
        {
            anim.Play(animName);
            anim.PlayQueued("idle1");
        }
    }

    public void LoadScenes()
    {
        //保存完成加载新场景-YZL
        AvatarSys._instance.SaveJson();
        //SceneManager.LoadScene("Scenes/02");
    }
}

