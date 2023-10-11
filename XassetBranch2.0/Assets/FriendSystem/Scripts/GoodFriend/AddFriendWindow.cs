using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FriendSystem
{
    public class AddFriendWindow : MonoBehaviour
    {
        [SerializeField] PopUpApplyWindow popUp;
        [SerializeField] Button searchBtn;
        [SerializeField] InputField searchTxT;
        [SerializeField] Transform content;
        [SerializeField] Button applyListBtn;   // 打开申请好友 面板 按钮

        [SerializeField]RoleItemPrefab item;

        private void Start()
        {
            searchBtn.onClick.AddListener(OnSearchClick);
            applyListBtn.onClick.AddListener(OnOpenApplyPanelClick);
        }
        private void OnEnable()
        {
            item.gameObject.SetActive(false);
            searchTxT.text = "";
        }

        // 点击 s搜索 
        private void OnSearchClick()
        {
            InfoHandle.GetStudentByPhone(searchTxT.text, OnResponse);
        }
        // 点击 打开 申请 好友 面板
        private void OnOpenApplyPanelClick()
        {
            popUp.ShowApplyWindow(true);
        }
        // 响应 获取学生信息
        private void OnResponse(string json)
        {
            Debug.Log($"通过手机号获取学生：" + json);
            JObject obj = (JObject)JsonConvert.DeserializeObject(json);
            string msg = (string)obj["msg"];
            int code = (int)obj["code"];
            var dataJson = obj["data"].ToString();
            if (code == 200)
            {
                Personnel people = JsonConvert.DeserializeObject<Personnel>(dataJson);
                item.Show(people);
            }
        }
    }
}
