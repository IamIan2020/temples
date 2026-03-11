## MODIFIED Requirements

### Requirement: 會員登入

系統 SHALL 提供 `POST /api/auth/login` 端點。驗證成功後回傳 JWT Access Token（15 分鐘效期）和 Refresh Token（7 天效期）。回傳的 user 資訊 MUST 包含角色列表和權限列表（permissions），前端根據角色決定導向前台或後台，根據權限控制 UI 顯示。

#### Scenario: 登入成功

- **WHEN** 使用者提供正確的 Email 和密碼，且帳號為啟用狀態
- **THEN** 系統回傳 JWT Access Token、Refresh Token 和包含角色與權限的 user 資訊

#### Scenario: 密碼錯誤

- **WHEN** 使用者提供錯誤的密碼
- **THEN** 系統回傳 HTTP 401 + 錯誤訊息

#### Scenario: 帳號已停用

- **WHEN** 使用者帳號的 IsActive 為 false
- **THEN** 系統回傳 HTTP 401 + 帳號已停用的錯誤訊息

#### Scenario: Email 不存在

- **WHEN** 使用者提供未註冊的 Email
- **THEN** 系統回傳 HTTP 401 + 通用錯誤訊息（不揭露帳號是否存在）

### Requirement: JWT 認證設定

系統 SHALL 使用 JWT Bearer Token 進行 API 認證。Token MUST 包含使用者 ID、Email、角色、權限（permission claims）等 Claims。JWT 密鑰 MUST 透過 User Secrets 管理。Permission claims MUST 從使用者角色的 RoleClaims 中查詢並寫入 JWT。

#### Scenario: 有效 Token 存取受保護 API

- **WHEN** 請求帶有有效且未過期的 JWT Access Token
- **THEN** 系統允許存取受保護的 API 端點

#### Scenario: Token 包含權限 Claims

- **WHEN** WebAdmin 登入取得 JWT Token
- **THEN** Token 中包含該角色的所有 permission claims（如 members.view、members.edit、members.delete）

#### Scenario: 無 Token 存取受保護 API

- **WHEN** 請求未帶有 Authorization header
- **THEN** 系統回傳 HTTP 401 Unauthorized

#### Scenario: 過期 Token

- **WHEN** 請求帶有已過期的 JWT Access Token
- **THEN** 系統回傳 HTTP 401 Unauthorized

## ADDED Requirements

### Requirement: 前端權限控制

前端 Auth Store MUST 提供 `hasPermission(permission: string)` 方法。SystemAdmin 角色 MUST 永遠回傳 true。其他角色根據 UserInfo 中的 permissions 陣列判斷。UserInfo MUST 包含 permissions 欄位。

#### Scenario: SystemAdmin 權限檢查

- **WHEN** 前端呼叫 `hasPermission('any.permission')` 且使用者為 SystemAdmin
- **THEN** 回傳 true

#### Scenario: 有權限角色檢查

- **WHEN** 前端呼叫 `hasPermission('members.view')` 且使用者擁有該權限
- **THEN** 回傳 true

#### Scenario: 無權限角色檢查

- **WHEN** 前端呼叫 `hasPermission('settings.edit')` 且使用者沒有該權限
- **THEN** 回傳 false
