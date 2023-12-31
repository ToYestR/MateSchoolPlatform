using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ApiCore
{
    public static string Host = "http://47.101.199.191:8090/";

    //public static string Url_GetFileList = "api/Meta/getFileList";
    //public static string Url_Upload = "api/Meta/upload";
    //public static string Url_Delete = "api/Meta/del";
    /// <summary>
    /// 学生注册
    /// </summary>
    public static string Url_Register = "open-api/common/register";
    /// <summary>
    /// 发送验证码
    /// </summary>
    public static string Url_Send = "open-api/common/SMS";
    /// <summary>
    /// 登陆接口
    /// </summary>
    public static string Url_Login = "open-api/common/login";
    #region 包管理相关
    /// <summary>
    /// 查询学校类型
    /// </summary>
    public static string Url_SchoolList = "open-api/business/listSchool";
    /// <summary>
    /// 查询场景主包-未审核
    /// </summary>
    public static string Url_ListUnAuditMainScene = "open-api/business/listUnAuditMainScene";
    /// <summary>
    /// 查询场景主包-已审核
    /// </summary>
    public static string Url_ListAuditMainScene = "open-api/business/listAuditMainScene";
    /// <summary>
    /// 查询场景子包-未审核
    /// </summary>
    public static string Url_ListUnAuditChildScene = "open-api/business/listUnAuditChildScene";
    /// <summary>
    /// 查询场景子包-已审核
    /// </summary>
    public static string Url_ListAuditChildScene = "open-api/business/listAuditChildScene";
    /// <summary>
    /// 查询任意场景包
    /// </summary>
    public static string Url_listScenePackageByIds = "open-api/common/listScenePackageByIds";
    #endregion

    #region 好友相关接口
    /// <summary>
    /// 查询好友列表
    /// </summary>
    public static string Url_FriendList = "open-api/chat/friend/list";
    /// <summary>
    /// 好友申请列表
    /// </summary>
    public static string Url_ApplyList = "open-api/chat/apply/list";
    /// <summary>
    /// 申请添加好友
    /// </summary>
    public static string Url_Apply = "open-api/chat/apply";
    /// <summary>
    /// 好友申请审核
    /// </summary>
    public static string Url_ApplyAudit = "open-api/chat/apply/audit";
    /// <summary>
    /// 好友删除
    /// </summary>
    public static string Url_DeleteFr = "open-api/chat/friend/del";
    /// <summary>
    /// 好友拉黑
    /// </summary>
    public static string Url_BlackFr = "open-api/chat/friend/black";
    #endregion
    #region 场景传送点相关
    /// <summary>
    /// 添加传送点
    /// </summary>
    public static string Url_addPackageTransmission = "open-api/common/addScenePackageTransmission";
    /// <summary>
    /// 删除传送点
    /// </summary>
    public static string Url_RemoveTransmission = "open-api/common/removeScenePackageTransmission";
    /// <summary>
    /// 修改传送点
    /// </summary>
    public static string Url_updateScenePackageTransmission = "open-api/common/updateScenePackageTransmission";
    /// <summary>
    /// 查询传送点列表
    /// </summary>
    public static string selectScenePackageTransmission = "open-api/common/selectScenePackageTransmission";
    #endregion
    #region 群组管理
    /// <summary>
    /// 创建群
    /// </summary>
    public static string Url_GroupCreate = "open-api/chat/group/add";
    /// <summary>
    /// 获取群信息
    /// </summary>
    public static string Url_GetGroupInfo = "open-api/chat/group/get";
    /// <summary>
    /// 刷新群信息
    /// </summary>
    public static string Url_UpdateGroupInfo = "open-api/chat/group/update";
    /// <summary>
    /// 删除群信息
    /// </summary>
    public static string Url_DelGroup = "open-api/chat/group/del";
    /// <summary>
    /// 加入群
    /// </summary>
    public static string Url_JoinGroup = "open-api/chat/group/join";
    /// <summary>
    /// 退出群
    /// </summary>
    public static string Url_QuitGroup = "open-api/chat/group/quit";
    /// <summary>
    /// 获得当前在线人
    /// </summary>
    public static string Url_GetOnlineUsers = "online/getUsers";

    public static string Url_GetCodeGroup = "";
    /// <summary>
    /// 获得指定的SCENID信息
    /// </summary>
    public static string Url_GetScenePackageInfo = "open-api/business/scenePackageInfo";
    #endregion
    #region 编辑学生信息
    /// <summary>
    /// 更新学生信息
    /// </summary>
    public static string Url_EditStudengtInfo = "open-api/business/editStudent";
    /// <summary>
    /// 获得学生基础信息通过token
    /// </summary>
    public static string Url_GetStudentInfoByToken = "open-api/business/getStudentByToken";
    /// <summary>
    /// 获得学生信息
    /// </summary>
    public static string Url_GetStudentInfoByNum = "open-api/business/getStudent";
    /// <summary>
    /// 通过标签添加学生
    /// </summary>
    public static string Url_TagAddStudent = "open-api/common/addLabelStudent";
    /// <summary>
    /// 通过标签删除学生
    /// </summary>
    public static string Url_TagDelStudent = "open-api/common/delLabelStudent";
    /// <summary>
    /// 更新 好友 备注
    /// </summary>

    public static string Url_UpdateFriendRemark = "open-api/chat/friend/updateFriendRemark";
    #endregion
    #region 展厅相关接口
    /// <summary>
    /// 保存展厅信息相关接口
    /// </summary>
    public static string Url_SaveScenePackageShowroom = "open-api/common/saveScenePackageShowroom";
    /// <summary>
    /// 查询展厅信息相关接口
    /// </summary>
    public static string Url_ListScenePackageShowroom = "open-api/common/listScenePackageShowroom";
    /// <summary>
    /// 上传展厅相关文件
    /// </summary>
    public static string Url_Upload = "open-api/common/upload";
    #endregion
    public static string GetUrl(string Api)
    {
        return Host + Api;
    }
}

