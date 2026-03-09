## ADDED Requirements

### Requirement: Vue 3 專案架構

前端 SHALL 使用 Vue 3 + TypeScript + Vite 建立，並安裝 Element Plus、Axios、Pinia、Vue Router。

#### Scenario: 前端專案建立成功

- **WHEN** 執行 `npm run build` 於 frontend 目錄
- **THEN** 專案編譯成功且無錯誤

### Requirement: Axios API Client 與 JWT Interceptor

前端 SHALL 建立 Axios 實例，設定 base URL 指向後端 API。Request interceptor MUST 自動附加 JWT Token。Response interceptor MUST 在收到 401 時嘗試使用 Refresh Token 刷新。

#### Scenario: 自動附加 Token

- **WHEN** 前端發送 API 請求且 localStorage 中有 Access Token
- **THEN** Axios 自動在 Authorization header 附加 Bearer Token

#### Scenario: Token 過期自動刷新

- **WHEN** API 回傳 401 且有有效的 Refresh Token
- **THEN** Axios 自動呼叫 refresh-token API，取得新 Token 後重試原請求

#### Scenario: Refresh Token 也過期

- **WHEN** refresh-token API 也回傳 401
- **THEN** 前端清除所有 Token，導向登入頁面

### Requirement: Pinia Auth Store

前端 SHALL 使用 Pinia 管理認證狀態，包含 user 資訊、Token、登入/登出/註冊 actions。

#### Scenario: 登入後狀態更新

- **WHEN** 使用者登入成功
- **THEN** Store 儲存 user 資訊和 Token，isAuthenticated 為 true

#### Scenario: 登出清除狀態

- **WHEN** 使用者點擊登出
- **THEN** Store 清除所有認證資訊，導向登入頁面

### Requirement: 路由與導航守衛

前端 SHALL 設定 Vue Router，包含以下路由：
- 公開路由：登入、註冊、忘記密碼、重設密碼
- 會員路由：個人資料（需登入）
- 後台路由：會員列表、會員詳情（需 WebAdmin+ 角色）

導航守衛 MUST 檢查認證狀態和角色權限。

#### Scenario: 未登入存取會員頁面

- **WHEN** 未登入的使用者存取個人資料頁面
- **THEN** 系統導向登入頁面

#### Scenario: Member 存取後台頁面

- **WHEN** Member 角色的使用者存取後台會員列表
- **THEN** 系統導向 403 頁面或首頁

### Requirement: 登入頁面

前端 SHALL 提供登入頁面，使用 Element Plus 表單元件，包含 Email 和密碼輸入欄位、登入按鈕、「忘記密碼」和「註冊」連結。

#### Scenario: 登入成功

- **WHEN** 使用者輸入正確的 Email 和密碼並點擊登入
- **THEN** 系統登入成功，導向個人資料頁面

#### Scenario: 登入失敗

- **WHEN** 使用者輸入錯誤的帳號密碼
- **THEN** 頁面顯示錯誤訊息

### Requirement: 註冊頁面

前端 SHALL 提供註冊頁面，包含 Email、密碼、確認密碼、顯示名稱輸入欄位。MUST 使用 Element Plus 表單驗證。

#### Scenario: 註冊成功

- **WHEN** 使用者填寫所有必填欄位且驗證通過
- **THEN** 系統註冊成功，顯示成功訊息並導向登入頁面

#### Scenario: 前端驗證失敗

- **WHEN** 使用者未填寫必填欄位或密碼不一致
- **THEN** 表單顯示即時的驗證錯誤訊息

### Requirement: 忘記密碼頁面

前端 SHALL 提供忘記密碼頁面，包含 Email 輸入欄位和送出按鈕。

#### Scenario: 送出成功

- **WHEN** 使用者輸入 Email 並點擊送出
- **THEN** 頁面顯示「若該 Email 已註冊，將收到重設密碼信件」訊息

### Requirement: 重設密碼頁面

前端 SHALL 提供重設密碼頁面，從 URL 參數取得 Email 和 Token，提供新密碼和確認密碼輸入欄位。

#### Scenario: 重設成功

- **WHEN** 使用者輸入符合規則的新密碼並確認
- **THEN** 系統重設密碼成功，導向登入頁面

### Requirement: 個人資料頁面

前端 SHALL 提供個人資料頁面，顯示並允許編輯：DisplayName、ChineseName、Birthday、Gender、Address。另外提供變更密碼功能。

#### Scenario: 查看個人資料

- **WHEN** 已登入的會員存取個人資料頁面
- **THEN** 頁面顯示會員的所有個人資料

#### Scenario: 更新個人資料

- **WHEN** 會員修改資料後點擊儲存
- **THEN** 系統更新資料成功，頁面顯示更新後的資料

#### Scenario: 變更密碼

- **WHEN** 會員輸入舊密碼和新密碼後點擊變更
- **THEN** 系統變更密碼成功，顯示成功訊息

### Requirement: 後台會員列表頁面

前端 SHALL 提供後台會員列表頁面（僅 WebAdmin+ 可見），包含分頁表格和搜尋輸入欄位。表格顯示：Email、DisplayName、角色、狀態、建立時間。

#### Scenario: 查看會員列表

- **WHEN** WebAdmin 或 SystemAdmin 存取後台會員列表
- **THEN** 頁面顯示分頁的會員列表

#### Scenario: 搜尋會員

- **WHEN** 管理員輸入搜尋關鍵字
- **THEN** 列表即時篩選符合條件的會員

### Requirement: 後台會員詳情頁面

前端 SHALL 提供後台會員詳情頁面（僅 WebAdmin+ 可見），顯示會員完整資料。WebAdmin 可啟用/停用會員。SystemAdmin 額外可變更會員角色。

#### Scenario: 查看會員詳情

- **WHEN** 管理員從列表點擊查看會員
- **THEN** 頁面顯示該會員的完整資料

#### Scenario: 停用會員

- **WHEN** 管理員點擊停用按鈕
- **THEN** 系統停用該會員，頁面更新狀態顯示

#### Scenario: SystemAdmin 變更角色

- **WHEN** SystemAdmin 在詳情頁面變更會員角色
- **THEN** 系統更新角色，頁面顯示新角色

### Requirement: Layout 元件

前端 SHALL 提供 Layout 元件，包含 Header（顯示系統名稱、使用者名稱、登出按鈕）和側邊導航選單。導航選單 MUST 根據角色動態顯示：Member 只顯示個人資料，WebAdmin+ 額外顯示後台管理選單。

#### Scenario: Member 角色導航

- **WHEN** Member 角色登入
- **THEN** 導航選單只顯示「個人資料」

#### Scenario: WebAdmin 角色導航

- **WHEN** WebAdmin 角色登入
- **THEN** 導航選單顯示「個人資料」和「會員管理」

### Requirement: TypeScript 類型與後端 DTO 一致

前端的 TypeScript 類型定義 MUST 與後端 DTO 欄位完全對應。C# PascalCase 轉換為 TypeScript camelCase。

#### Scenario: 類型一致性

- **WHEN** 比對前端 TypeScript 類型與後端 C# DTO
- **THEN** 所有欄位名稱（轉換後）和類型完全對應

### Requirement: Vite Proxy 設定

前端 SHALL 設定 Vite dev server proxy，將 `/api` 路徑代理至後端 API 伺服器。

#### Scenario: API 請求代理

- **WHEN** 前端開發伺服器收到 `/api/*` 請求
- **THEN** 請求被代理至後端 API 伺服器（如 `http://localhost:5000`）
