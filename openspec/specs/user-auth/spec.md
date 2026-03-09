## ADDED Requirements

### Requirement: 會員註冊

系統 SHALL 提供 `POST /api/auth/register` 端點，允許使用者以 Email 註冊帳號。註冊時 MUST 提供：Email、密碼、確認密碼、顯示名稱。註冊成功後自動指派 Member 角色。

#### Scenario: 註冊成功

- **WHEN** 使用者提供有效的 Email、密碼（符合強度要求）、確認密碼（一致）、顯示名稱
- **THEN** 系統建立帳號、指派 Member 角色，回傳成功訊息

#### Scenario: Email 已被使用

- **WHEN** 使用者提供已註冊過的 Email
- **THEN** 系統回傳 HTTP 400 + 錯誤訊息「該 Email 已被註冊」

#### Scenario: 密碼強度不足

- **WHEN** 使用者提供的密碼不符合 Identity 預設密碼規則
- **THEN** 系統回傳 HTTP 400 + 密碼強度不足的錯誤訊息

#### Scenario: 確認密碼不一致

- **WHEN** 密碼與確認密碼不一致
- **THEN** 系統回傳 HTTP 400 + 驗證錯誤

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

### Requirement: Token 刷新

系統 SHALL 提供 `POST /api/auth/refresh-token` 端點，使用有效的 Refresh Token 換取新的 Access Token。

#### Scenario: 刷新成功

- **WHEN** 提供有效且未過期的 Refresh Token
- **THEN** 系統回傳新的 Access Token 和新的 Refresh Token

#### Scenario: Refresh Token 過期

- **WHEN** 提供已過期的 Refresh Token
- **THEN** 系統回傳 HTTP 401，使用者需重新登入

### Requirement: 忘記密碼

系統 SHALL 提供 `POST /api/auth/forgot-password` 端點。輸入 Email 後，系統透過 MailKit + Gmail SMTP 寄送包含重設連結的信件。

#### Scenario: 寄送重設信成功

- **WHEN** 使用者提供已註冊的 Email
- **THEN** 系統產生 reset token，寄送包含重設連結的信件至該 Email

#### Scenario: Email 不存在但不揭露

- **WHEN** 使用者提供未註冊的 Email
- **THEN** 系統仍回傳成功訊息（不揭露帳號是否存在），但不實際寄信

### Requirement: 重設密碼

系統 SHALL 提供 `POST /api/auth/reset-password` 端點。使用者提供 Email、reset token、新密碼來重設密碼。

#### Scenario: 重設成功

- **WHEN** 使用者提供有效的 Email、未過期的 reset token、符合規則的新密碼
- **THEN** 系統更新密碼，回傳成功訊息

#### Scenario: Token 無效或過期

- **WHEN** 提供無效或已過期的 reset token
- **THEN** 系統回傳 HTTP 400 + 錯誤訊息

### Requirement: JWT 認證設定

系統 SHALL 使用 JWT Bearer Token 進行 API 認證。Token MUST 包含使用者 ID、Email、角色等 Claims。JWT 密鑰 MUST 透過 User Secrets 管理。

#### Scenario: 有效 Token 存取受保護 API

- **WHEN** 請求帶有有效且未過期的 JWT Access Token
- **THEN** 系統允許存取受保護的 API 端點

#### Scenario: 無 Token 存取受保護 API

- **WHEN** 請求未帶有 Authorization header
- **THEN** 系統回傳 HTTP 401 Unauthorized

#### Scenario: 過期 Token

- **WHEN** 請求帶有已過期的 JWT Access Token
- **THEN** 系統回傳 HTTP 401 Unauthorized

### Requirement: FluentValidation 請求驗證

系統 SHALL 使用 FluentValidation 驗證所有認證相關的請求 DTO，包含 Email 格式、密碼長度、必填欄位等。

#### Scenario: 驗證失敗

- **WHEN** 請求 DTO 的欄位不符合驗證規則
- **THEN** 系統回傳 HTTP 400 + 詳細的欄位驗證錯誤

### Requirement: Email 服務

系統 SHALL 提供 IEmailService 介面和 EmailService 實作，使用 MailKit 透過 Gmail SMTP 寄送信件。SMTP 設定 MUST 透過 User Secrets 管理。

#### Scenario: 寄送密碼重設信

- **WHEN** 呼叫 EmailService 寄送密碼重設信
- **THEN** 透過 Gmail SMTP 成功寄出信件，信件包含重設連結
