using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace FriendSystem
{
    /// <summary>
    /// 好友 标签预制件
    /// </summary>
    public class TagItemPrefab : MonoBehaviour
    {
        [HideInInspector] public MyTag myTag;
        [SerializeField] Text tagName;
        [SerializeField] Text nickNameList;
        /// <summary>
        /// 初始化 标签
        /// </summary>
        public void Init(MyTag tag)
        {
            myTag = tag;
            tagName.text = myTag.name;
            nickNameList.text = "";
        }
        private void OnEnable()
        {
            StartCoroutine(Refresh());
        }
        private IEnumerator Refresh()
        {
            while(true)
            {
                yield return null;
                InfoHandle.GetStudentsByTag(myTag.labelId, OnRequestStudent);
                yield return new WaitForSeconds(GoodFriendManager.getInstance.requestTime);
            }
        }
        private void OnRequestStudent(string json)
        {
            //Debug.Log($"获取{myTag.name} 标签 下的 学生:" + json);
            JObject obj = (JObject)JsonConvert.DeserializeObject(json);
            string msg = (string)obj["msg"];
            int code = (int)obj["code"];
            JArray dataJson = (JArray)obj["data"]; 
            string tagStr = "";
            if (code == 200&&dataJson.Count>0)
            {
                
                string jsonStr = dataJson[0]["students"].ToString();
                var tags = JsonConvert.DeserializeObject<List<TagFriend>>(jsonStr);
                Debug.Log(tags.Count);
                foreach(var item in tags)
                {
                    tagStr += item.nickName + "  ";
                    //Debug.Log(item.nickName);
                }
            }
            nickNameList.text = tagStr;
        }
    }
}
