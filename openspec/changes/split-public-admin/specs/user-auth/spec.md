## MODIFIED Requirements

### Requirement: 會員登入

系統 SHALL 提供 `POST /api/auth/login` 端點。驗證成功後回傳 JWT Access Token（15 分鐘效期）和 Refresh Token（7 天效期）。回傳的 user 資訊 MUST 包含角色列表，前端根據角色決定導向前台或後台。

#### Scenario: 登入成功

- **WHEN** 使用者提供正確的 Email 和密碼，且帳號為啟用狀態
- **THEN** 系統回傳 JWT Access Token、Refresh Token 和包含角色的 user 資訊

#### Scenario: 密碼錯誤

- **WHEN** 使用者提供錯誤的密碼
- **THEN** 系統回傳 HTTP 401 + 錯誤訊息

#### Scenario: 帳號已停用

- **WHEN** 使用者帳號的 IsActive 為 false
- **THEN** 系統回傳 HTTP 401 + 帳號已停用的錯誤訊息

#### Scenario: Email 不存在

- **WHEN** 使用者提供未註冊的 Email
- **THEN** 系統回傳 HTTP 401 + 通用錯誤訊息（不揭露帳號是否存在）
