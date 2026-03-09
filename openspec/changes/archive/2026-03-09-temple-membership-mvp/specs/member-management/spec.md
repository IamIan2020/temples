## ADDED Requirements

### Requirement: 取得自己的 Profile

系統 SHALL 提供 `GET /api/members/me` 端點，讓已登入的會員取得自己的個人資料。

#### Scenario: 取得成功

- **WHEN** 已登入的會員請求 `/api/members/me`
- **THEN** 系統回傳該會員的完整個人資料（DisplayName、ChineseName、Birthday、Gender、Address、Email、MemberNumber、JoinDate）

#### Scenario: 未登入

- **WHEN** 未帶 Token 的請求存取 `/api/members/me`
- **THEN** 系統回傳 HTTP 401

### Requirement: 更新自己的 Profile

系統 SHALL 提供 `PUT /api/members/me` 端點，讓已登入的會員更新自己的個人資料。可更新的欄位：DisplayName、ChineseName、Birthday、Gender、Address。

#### Scenario: 更新成功

- **WHEN** 已登入的會員提供有效的更新資料
- **THEN** 系統更新個人資料並回傳更新後的完整資料

#### Scenario: 驗證失敗

- **WHEN** 提供的資料不符合驗證規則（如 DisplayName 為空）
- **THEN** 系統回傳 HTTP 400 + 驗證錯誤訊息

### Requirement: 變更自己的密碼

系統 SHALL 提供 `PUT /api/members/me/password` 端點，讓已登入的會員變更自己的密碼。MUST 提供舊密碼和新密碼。

#### Scenario: 變更成功

- **WHEN** 已登入的會員提供正確的舊密碼和符合規則的新密碼
- **THEN** 系統更新密碼並回傳成功訊息

#### Scenario: 舊密碼錯誤

- **WHEN** 提供的舊密碼不正確
- **THEN** 系統回傳 HTTP 400 + 錯誤訊息

### Requirement: 會員列表（後台）

系統 SHALL 提供 `GET /api/members` 端點，僅 WebAdmin 和 SystemAdmin 角色可存取。支援分頁和關鍵字搜尋（搜尋 Email、DisplayName、ChineseName）。

#### Scenario: 管理員查詢會員列表

- **WHEN** WebAdmin 或 SystemAdmin 請求會員列表
- **THEN** 系統回傳分頁的會員列表，包含總數、頁碼、每頁筆數

#### Scenario: 關鍵字搜尋

- **WHEN** 管理員提供搜尋關鍵字
- **THEN** 系統回傳符合條件的會員列表（模糊比對 Email、DisplayName、ChineseName）

#### Scenario: 一般會員存取

- **WHEN** Member 角色的使用者請求 `/api/members`
- **THEN** 系統回傳 HTTP 403 Forbidden

### Requirement: 查看指定會員（後台）

系統 SHALL 提供 `GET /api/members/{id}` 端點，僅 WebAdmin 和 SystemAdmin 角色可存取。

#### Scenario: 查看成功

- **WHEN** 管理員提供有效的會員 ID
- **THEN** 系統回傳該會員的完整資料

#### Scenario: 會員不存在

- **WHEN** 管理員提供不存在的會員 ID
- **THEN** 系統回傳 HTTP 404

### Requirement: 啟用/停用會員（後台）

系統 SHALL 提供 `PUT /api/members/{id}/status` 端點，僅 WebAdmin 和 SystemAdmin 角色可存取。可設定會員的 IsActive 狀態。

#### Scenario: 停用會員

- **WHEN** 管理員將會員的 IsActive 設為 false
- **THEN** 系統更新會員狀態，該會員將無法登入

#### Scenario: 啟用會員

- **WHEN** 管理員將會員的 IsActive 設為 true
- **THEN** 系統更新會員狀態，該會員恢復登入能力

#### Scenario: 停用自己的帳號

- **WHEN** 管理員嘗試停用自己的帳號
- **THEN** 系統回傳 HTTP 400，禁止停用自己

### Requirement: 變更會員角色（後台）

系統 SHALL 提供 `PUT /api/members/{id}/role` 端點，僅 SystemAdmin 角色可存取。可變更會員的角色。

#### Scenario: SystemAdmin 變更角色

- **WHEN** SystemAdmin 變更會員的角色
- **THEN** 系統更新角色並回傳成功

#### Scenario: WebAdmin 嘗試變更角色

- **WHEN** WebAdmin 嘗試存取角色變更端點
- **THEN** 系統回傳 HTTP 403 Forbidden

#### Scenario: 變更自己的角色

- **WHEN** SystemAdmin 嘗試變更自己的角色
- **THEN** 系統回傳 HTTP 400，禁止變更自己的角色

### Requirement: 停用會員（DELETE）

系統 SHALL 提供 `DELETE /api/members/{id}` 端點，僅 WebAdmin 和 SystemAdmin 角色可存取。此端點為軟刪除，將 IsActive 設為 false。

#### Scenario: 軟刪除成功

- **WHEN** 管理員對有效的會員 ID 執行 DELETE
- **THEN** 系統將該會員的 IsActive 設為 false，回傳成功

#### Scenario: 刪除自己

- **WHEN** 管理員嘗試 DELETE 自己的帳號
- **THEN** 系統回傳 HTTP 400，禁止刪除自己
