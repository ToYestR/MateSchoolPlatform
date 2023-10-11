using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FriendSystem
{
    public class GroupListPrefabItem : PoolManager
    {
        [SerializeField] Text tagName;
        [SerializeField] Text count;
        [SerializeField] Toggle toggle;
        private MyTag target;

        public void Show(MyTag tag)
        {
            GetComponent<DropDownMenu>().enabled = false;
            target = tag;
            tagName.text = tag.name;
            InfoHandle.GetStudentsByTag(target.labelId, OnResponse);
        }
        private void OnDisable()
        {
            Clear();
        }

        private void OnResponse(string json)
        {
            Debug.Log($"获取{target.name} 下的 学生:" + json);
            JObject obj = (JObject)JsonConvert.DeserializeObject(json);
            string msg = (string)obj["msg"];
            int code = (int)obj["code"];
            JArray dataJson = (JArray)obj["data"];
            if (code == 200 && dataJson.Count > 0)
            {
                string jsonStr = dataJson[0]["students"].ToString();
                var tags = JsonConvert.DeserializeObject<List<TagFriend>>(jsonStr);
                int onlineCount = 0;
                foreach (var item in tags)
                {
                    // TODO 生成学生
                    if (GoodFriendManager.getInstance.friendList.ContainsKey(item.chatNo))
                    {
                        Friend friend = GoodFriendManager.getInstance.friendList[item.chatNo];
                        friend.tag = target;
                        if (friend.isOnLine)
                        {
                            onlineCount++;
                        }
                        var go = pool.Get();
                        go.GetComponent<FriendListPrefabItem>().Show(friend, nameof(GroupListPrefabItem));
                    }
                }
                count.text = $"{ onlineCount}/{tags.Count }";
                GetComponent<DropDownMenu>().enabled = true;
            }
        }
    }
}
