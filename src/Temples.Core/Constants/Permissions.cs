namespace Temples.Core.Constants;

public static class Permissions
{
    public const string MembersView = "members.view";
    public const string MembersEdit = "members.edit";
    public const string MembersDelete = "members.delete";
    public const string SettingsView = "settings.view";
    public const string SettingsEdit = "settings.edit";
    public const string RolesManage = "roles.manage";
    public const string ServiceItemsView = "serviceItems.view";
    public const string ServiceItemsEdit = "serviceItems.edit";
    public const string ServiceItemsDelete = "serviceItems.delete";

    public static readonly string[] All =
    [
        MembersView, MembersEdit, MembersDelete,
        SettingsView, SettingsEdit,
        RolesManage,
        ServiceItemsView, ServiceItemsEdit, ServiceItemsDelete
    ];

    public static readonly Dictionary<string, (string Code, string DisplayName)[]> Groups = new()
    {
        ["會員管理"] =
        [
            (MembersView, "查看會員"),
            (MembersEdit, "編輯會員"),
            (MembersDelete, "刪除會員"),
        ],
        ["系統設定"] =
        [
            (SettingsView, "查看設定"),
            (SettingsEdit, "編輯設定"),
        ],
        ["權限管理"] =
        [
            (RolesManage, "管理角色"),
        ],
        ["服務項目"] =
        [
            (ServiceItemsView, "查看服務項目"),
            (ServiceItemsEdit, "編輯服務項目"),
            (ServiceItemsDelete, "刪除服務項目"),
        ],
    };

    public static readonly Dictionary<string, string> DisplayNames = new()
    {
        [MembersView] = "查看會員",
        [MembersEdit] = "編輯會員",
        [MembersDelete] = "刪除會員",
        [SettingsView] = "查看設定",
        [SettingsEdit] = "編輯設定",
        [RolesManage] = "管理角色",
        [ServiceItemsView] = "查看服務項目",
        [ServiceItemsEdit] = "編輯服務項目",
        [ServiceItemsDelete] = "刪除服務項目",
    };
}
