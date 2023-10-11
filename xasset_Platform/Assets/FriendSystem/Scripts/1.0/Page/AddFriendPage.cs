using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FriendSystem
{
    public class AddFriendPage : PoolManager
    {
        [SerializeField] Button backBtn;
        [SerializeField] TMP_InputField input;
        [SerializeField] Transform tip;
        protected override void Awake()
        {
            base.Awake();
        }
        private void Start()
        {
            input.onSubmit.AddListener(OnSubmit);
            backBtn.onClick.AddListener(Back);
        }

        string oldPage;
        public void Show(string oldPage)    // 显示当前页面
        {
            this.oldPage = oldPage;
        }
        private void Back()                 // 回到之前页面
        {
            PageManager.getInstance.Back(oldPage);
        }

        private void OnEnable()
        {
            //foreach (var item in GoodFriendManager.getInstance.applyList)
            //{
            //    var obj = pool.Get();
            //    obj.GetComponent<AddFriendPrefabItem>().Show(item.Value, this);
            //}
            tip.gameObject.SetActive(false);
        }
        private void OnDisable()
        {
            Clear();
        }
        private void OnSubmit(string value)
        {
            if(!string.IsNullOrWhiteSpace(value))
            {
                InfoHandle.GetStudentByPhone(value, OnResponse);
            }
            input.text = "";
        }
        // 响应 查找学生信息
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
                var go = pool.Get();
                go.GetComponent<AddFriendPrefabItem>().Show(people, this);
            }    
        }
        public void ShowTip()
        {
            StartCoroutine(TipAnimation());
        }
        private IEnumerator TipAnimation()
        {
            tip.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(2);
            tip.gameObject.SetActive(false);
        }
    }
}
