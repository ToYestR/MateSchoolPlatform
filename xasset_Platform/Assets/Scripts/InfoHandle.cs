using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InfoHandle
{
    public static void GetStudentInfo()
    {
        JObject jobject = new JObject();
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", Global.token);
        WebRequestController.Instance.Post(ApiCore.GetUrl(ApiCore.Url_GetStudentInfoByToken), jobject.ToString(), header, (str) => Debug.Log(str));
    }
    public static void GetStudentInfoByNum(string Num)
    {
        JObject jobject = new JObject();
        jobject.Add("phonenumber", Num);
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", Global.token);
        WebRequestController.Instance.Post(ApiCore.GetUrl(ApiCore.Url_GetStudentInfoByNum), jobject.ToString(), header, (str) => Debug.Log(str));
    }
    public static void GetFrList(Action<string> action)
    {
        JObject jobject = new JObject();
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", Global.token);
        WebRequestController.Instance.Post(ApiCore.GetUrl(ApiCore.Url_FriendList), jobject.ToString(), header, action);
    }
    /// <summary>
    /// ������Ӻ���
    /// </summary>
    /// <param name="chatNo"></param>
    public static void ApplyFr(string chatNo, string reason,Action<string> action)
    {
        JObject jobect = new JObject();
        jobect.Add("toChatNo", chatNo);
        jobect.Add("reason", reason);
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", Global.token);
        WebRequestController.Instance.Post(ApiCore.GetUrl(ApiCore.Url_Apply), jobect.ToString(), header, action);
    }
    /// <summary>
    /// ��������б�
    /// </summary>
    public static void GetApplyList(Action<string> action)
    {
        JObject jobect = new JObject();
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", Global.token);
        WebRequestController.Instance.Post(ApiCore.GetUrl(ApiCore.Url_ApplyList), jobect.ToString(), header, action);

    }
    /// <summary>
    /// �����������б�
    /// </summary>
    /// <param name="action"></param>
    public static void GetOnlineFriend(Action<string> action)
    {
        string url = "http://ijob-x.com/ws-api/online/getUsers?appId=100001";
        JObject jobect = new JObject();
        jobect.Add("appId", "100001");
        WebRequestController.Instance.Get(url, Global.token, action);
    }
    /// <summary>
    /// ��ѯ����ѧУ
    /// </summary>
    /// <param name="action"></param>
    public static void SearchAllSchools(Action<string> action)
    {
        string url = "http://47.101.199.191:8090/open-api/common/schoolList";
        JObject jobect = new JObject();
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", Global.token);
        WebRequestController.Instance.Post(url, jobect.ToString(), header, action);
    }
    /// <summary>
    /// ����ҵı�ǩ�б�
    /// </summary>
    /// <param name="action"></param>
    public static void GetMyTags(Action<string> action)
    {
        string url = "http://47.101.199.191:8090/open-api/common/baseStudentLabelList";
        JObject jobect = new JObject();
        jobect.Add("studentId", Global.uid);
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", Global.token);
        WebRequestController.Instance.Post(url, jobect.ToString(), header, action);
    }
    /// <summary>
    /// ��ȡ��ǩ�����к����б�
    /// </summary>
    /// <param name="action"></param>
    public static void GetStudentsByTag(int tagId, Action<string> action)
    {
        string url = "http://47.101.199.191:8090/open-api/common/selectBaseStudentByLabelIds";
        JObject jobect = new JObject();
        JArray value = new JArray(tagId);
        jobect.Add("labelIds", value);
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", Global.token);
        WebRequestController.Instance.Post(url, jobect.ToString(), header, action);
    }
    /// <summary>
    /// �޸ı�ǩ
    /// </summary>
    /// <param name="lableId"></param>
    /// <param name="studentId"></param>
    /// <param name="tag"></param>
    /// <param name="action"></param>
    public static void EditTag(int lableId, int studentId, string tag, Action<string> action)
    {
        string url = "http://47.101.199.191:8090/open-api/common/editBaseStudentLabel";
        JObject jobect = new JObject();
        jobect.Add("labelId", lableId);
        jobect.Add("studentId", studentId);
        jobect.Add("name", tag);
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", Global.token);
        WebRequestController.Instance.Post(url, jobect.ToString(), header, action);
    }
    /// <summary>
    /// ɾ��ָ���ǩͨ����ǩID
    /// </summary>
    /// <param name="tagID"></param>
    /// <param name="action"></param>
    public static void DelTag(int tagID, Action<string> action)
    {
        string url = "http://47.101.199.191:8090/open-api/common/removeBaseStudentLabel";
        JObject jobect = new JObject();
        JArray array = new JArray(tagID);
        jobect.Add("labelIds", array);
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", Global.token);
        WebRequestController.Instance.Post(url, jobect.ToString(), header, action);
    }
    /// <summary>
    /// ��ӱ�ǩ
    /// </summary>
    /// <param name="tagID"></param>
    /// <param name="action"></param>
    public static void AddTag(string tag, Action<string> action)
    {
        string url = "http://47.101.199.191:8090/open-api/common/addBaseStudentLabel";
        JObject jobect = new JObject();
        jobect.Add("studentId", Global.uid);
        jobect.Add("name", tag);
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", Global.token);
        WebRequestController.Instance.Post(url, jobect.ToString(), header, action);
    }
    /// <summary>
    /// ָ�� ��ǩ ���ѧ��
    /// </summary>
    /// <param name="tagId"></param>
    /// <param name="studentIds"></param>
    public static void TagAddStudent(int tagId, List<int> studentIds)
    {
        JObject jobect = new JObject();
        jobect.Add("labelId", tagId);
        jobect.Add("studentIds", new JArray(studentIds));
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", Global.token);
        //Debug.Log("token:" + Global.token);
        WebRequestController.Instance.Post(ApiCore.GetUrl(ApiCore.Url_TagAddStudent), jobect.ToString(), header, (str) => Debug.Log(str));
    }
    /// <summary>
    /// ָ�� ��ǩ ɾ��ѧ��
    /// </summary>
    /// <param name="tagId"></param>
    /// <param name="studentIds"></param>
    public static void TagDelStudent(int tagId, List<int> studentIds)
    {
        JObject jobect = new JObject();
        jobect.Add("labelId", tagId);
        jobect.Add("studentIds", new JArray(studentIds));
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", Global.token);
        //Debug.Log("token:" + Global.token);
        WebRequestController.Instance.Post(ApiCore.GetUrl(ApiCore.Url_TagDelStudent), jobect.ToString(), header, (str) => Debug.Log(str));
    }
    /// <summary>
    /// ͨ���ֻ��� ��ȡ ѧ����Ϣ
    /// </summary>
    /// <param name="tagId"></param>
    /// <param name="studentIds"></param>
    public static void GetStudentByPhone(string phone, Action<string> action)
    {
        JObject jobect = new JObject();
        jobect.Add("phonenumber", phone);
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", Global.token);
        WebRequestController.Instance.Post(ApiCore.GetUrl(ApiCore.Url_GetStudentInfoByNum), jobect.ToString(), header, action);
    }
    // <summary>
    /// ��˺�������
    /// </summary>
    /// <param name="id"></param>
    /// <param name="status">����״̬:1ͬ��2�ܾ�</param>
    public static void AuditFr(int id, string status,Action<string> action=null)
    {
        JObject jobect = new JObject();
        jobect.Add("id", id);
        jobect.Add("applyStatus", status);
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", Global.token);
        //Debug.Log("token:" + Global.token);
        WebRequestController.Instance.Post(ApiCore.GetUrl(ApiCore.Url_ApplyAudit), jobect.ToString(), header, action);
    }
    /// <summary>
    /// �༭�û���Ϣ
    /// </summary>
    /// <param name="json">�û���Ϣ</param>
    public static void EditStudentInfo(string json, Action<string> action)
    {
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", Global.token);
        WebRequestController.Instance.Post(ApiCore.GetUrl(ApiCore.Url_EditStudengtInfo), json, header, action);
    }
    /// <summary>
    /// ɾ������
    /// </summary>
    /// <param name="id"></param>
    public static void DeleteFr(int id,Action<string> action)
    {
        JObject jobect = new JObject();
        jobect.Add("id", id);
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", Global.token);
        WebRequestController.Instance.Post(ApiCore.GetUrl(ApiCore.Url_DeleteFr), jobect.ToString(), header, action);
    }
    /// <summary>
    /// �������� �� ȡ������
    /// //Nδ���ڣ�Y����
    /// </summary>
    /// <param name="id"></param>
    /// <param name="isblack"></param>
    public static void BlackOther(int id, string isblack)
    {
        JObject jobect = new JObject();
        jobect.Add("id", id);
        jobect.Add("black", isblack);
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", Global.token);
        WebRequestController.Instance.Post(ApiCore.GetUrl(ApiCore.Url_BlackFr), jobect.ToString(), header, (str) => Debug.Log(str));
    }
    /// <summary>
    /// ���º��� ��ע
    /// </summary>
    /// <param name="id">����ID</param>
    /// <param name="msg">��ע</param>
    public static void UpdateRemark(int id, string msg)
    {
        JObject jobect = new JObject();
        jobect.Add("id", id);
        jobect.Add("remark", msg);
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", Global.token);
        WebRequestController.Instance.Post(ApiCore.GetUrl(ApiCore.Url_UpdateFriendRemark), jobect.ToString(), header, (str) => Debug.Log(str));
    }
    public static void CreateGroup(string json)
    {
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", Global.token);
        WebRequestController.Instance.Post(ApiCore.GetUrl(ApiCore.Url_GroupCreate), json, header, (str) => Debug.Log(str));
    }

    public static void GetGroupInfo()
    {
        JObject jobect = new JObject();

        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", Global.token);
        WebRequestController.Instance.Post(ApiCore.GetUrl(ApiCore.Url_GetGroupInfo), jobect.ToString(), header, (str) => Debug.Log(str));
    }
    public static void DeleteGroup(string groupChatNo)
    {

    }
    public static void UpdateGroup(string json)
    {

    }
    public static void JoinGroup(string json)
    {

    }
    public static void QuitGroup(string json)
    {

    }
}
