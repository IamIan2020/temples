## ADDED Requirements

### Requirement: ApplicationRole 實體

系統 SHALL 使用 `ApplicationRole`（繼承 IdentityRole）作為角色實體，包含 Description（描述）和 IsBuiltIn（是否內建）欄位。內建角色（SystemAdmin、WebAdmin、Member）的 IsBuiltIn MUST 為 true。

#### Scenario: 內建角色標記

- **WHEN** 系統初始化種子資料
- **THEN** SystemAdmin、WebAdmin、Member 角色的 IsBuiltIn 為 true

### Requirement: 權限常數定義

系統 SHALL 定義權限常數類別 `Permissions`，包含：members.view、members.edit、members.delete、settings.view、settings.edit、roles.manage。權限 MUST 按群組分類（會員管理、系統設定、權限管理）。

#### Scenario: 查詢所有可用權限

- **WHEN** 呼叫 `GET /api/roles/permissions`
- **THEN** 系統回傳按群組分類的所有權限列表

### Requirement: 內建角色預設權限

系統 SHALL 為內建角色設定預設 RoleClaims。WebAdmin 預設擁有 members.view、members.edit、members.delete。Member 無管理權限。SystemAdmin 透過 Policy assertion 永遠通過所有權限檢查。

#### Scenario: WebAdmin 預設權限

- **WHEN** 系統初始化種子資料
- **THEN** WebAdmin 角色擁有 members.view、members.edit、members.delete 權限 claims

### Requirement: 角色列表 API

系統 SHALL 提供 `GET /api/roles` 端點，僅 SystemAdmin 可存取。回傳所有角色列表，包含 Id、Name、Description、IsBuiltIn、Permissions。

#### Scenario: 查詢角色列表

- **WHEN** SystemAdmin 呼叫 `GET /api/roles`
- **THEN** 系統回傳所有角色及其權限

### Requirement: 建立自訂角色 API

系統 SHALL 提供 `POST /api/roles` 端點，僅 SystemAdmin 可存取。接受 CreateRoleRequest（Name、Description、Permissions），建立角色並設定 RoleClaims。

#### Scenario: 建立自訂角色成功

- **WHEN** SystemAdmin 提供有效的角色名稱和權限列表
- **THEN** 系統建立角色，設定 RoleClaims，回傳 HTTP 201

#### Scenario: 角色名稱重複

- **WHEN** 角色名稱已存在
- **THEN** 系統回傳 HTTP 400 和錯誤訊息

### Requirement: 更新角色 API

系統 SHALL 提供 `PUT /api/roles/{id}` 端點，僅 SystemAdmin 可存取。可修改 Name、Description、Permissions。內建角色只能修改 Description 和 Permissions，不可修改 Name。

#### Scenario: 更新自訂角色

- **WHEN** SystemAdmin 更新自訂角色的名稱、描述和權限
- **THEN** 系統更新成功

#### Scenario: 更新內建角色名稱

- **WHEN** SystemAdmin 嘗試修改內建角色的 Name
- **THEN** 系統回傳 HTTP 400，提示內建角色名稱不可修改

#### Scenario: 更新內建角色權限

- **WHEN** SystemAdmin 修改內建角色的 Permissions
- **THEN** 系統更新 RoleClaims 成功

### Requirement: 刪除角色 API

系統 SHALL 提供 `DELETE /api/roles/{id}` 端點，僅 SystemAdmin 可存取。內建角色不可刪除。有使用者使用的角色不可刪除。

#### Scenario: 刪除自訂角色成功

- **WHEN** SystemAdmin 刪除沒有使用者的自訂角色
- **THEN** 系統刪除角色成功

#### Scenario: 刪除內建角色

- **WHEN** SystemAdmin 嘗試刪除內建角色
- **THEN** 系統回傳 HTTP 400，提示內建角色不可刪除

#### Scenario: 刪除有使用者的角色

- **WHEN** SystemAdmin 嘗試刪除仍有使用者使用的角色
- **THEN** 系統回傳 HTTP 400，提示該角色仍有使用者

### Requirement: Policy-based 授權

系統 SHALL 為每個權限常數建立對應的 Authorization Policy。Policy MUST 檢查使用者是否為 SystemAdmin（永遠通過）或是否擁有對應的 permission claim。

#### Scenario: SystemAdmin 存取任何受保護端點

- **WHEN** SystemAdmin 存取任何需要權限的 API
- **THEN** 授權通過，不需檢查具體權限

#### Scenario: 有權限的角色存取

- **WHEN** 擁有 `members.view` permission claim 的使用者存取會員列表 API
- **THEN** 授權通過

#### Scenario: 無權限的角色存取

- **WHEN** 沒有 `members.view` permission claim 的使用者存取會員列表 API
- **THEN** 系統回傳 HTTP 403

### Requirement: 角色列表管理頁面

前端 SHALL 在 `/backstage/roles` 提供角色列表頁面。表格顯示角色名稱、描述、是否內建、權限數量。提供新增、編輯、刪除按鈕。內建角色 MUST 隱藏刪除按鈕。

#### Scenario: 查看角色列表

- **WHEN** SystemAdmin 存取 `/backstage/roles`
- **THEN** 頁面顯示所有角色列表

#### Scenario: 刪除角色確認

- **WHEN** SystemAdmin 點擊刪除按鈕
- **THEN** 系統顯示確認對話框，確認後執行刪除

### Requirement: 角色編輯頁面

前端 SHALL 在 `/backstage/roles/create` 和 `/backstage/roles/:id` 提供角色新增/編輯頁面。頁面包含名稱、描述欄位和按群組分類的權限 checkbox。

#### Scenario: 新增角色

- **WHEN** SystemAdmin 填寫角色名稱、描述並勾選權限後點擊儲存
- **THEN** 系統建立角色成功，導向角色列表

#### Scenario: 編輯角色

- **WHEN** SystemAdmin 修改角色資訊後點擊儲存
- **THEN** 系統更新角色成功
