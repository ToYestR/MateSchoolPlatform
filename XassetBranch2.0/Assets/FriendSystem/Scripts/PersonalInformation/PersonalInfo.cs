using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace FriendSystem
{
    /// <summary>
    /// 个人信息
    /// </summary>
    public static class PersonalInfo
    {
        public static int icon;                 // 头像
        public static string nickName;          // 昵称
        public static string accountNumber;     // 账号

        public static int age;                  // 年龄

        public static int sex;                  // 性别   （0男 1女 2未知）

        public static string mailbox;           // 邮箱

        public static string job;               // 职业

        public static string workUnit;         // 工作单位

        public static List<EducationInfo> educations = new List<EducationInfo>();   // 教育；历程

        private static string _educationsStr;
        public static string educationsStr      // 教育历程解析
        {
            set
            {
                _educationsStr = value;
                Debug.Log(value);
                if(string.IsNullOrWhiteSpace(value)||value== "Null")
                {
                    return;
                }
                else
                {
                    educations = JsonConvert.DeserializeObject<List<EducationInfo>>(_educationsStr);
                }
            }
            get
            {
                return _educationsStr;
            }
        }
    }

    public class EducationInfo
    {   
        public string school;           // 学校
        public string serie;       // 院系
        public string time;           // 入学时间
        public string level;        // 学历
    }
}