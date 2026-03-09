## ADDED Requirements

### Requirement: 前台 Layout

前端 SHALL 提供 `PublicLayout.vue` 元件，包含簡潔的 Header（系統名稱、使用者名稱/登入按鈕、登出按鈕）。不包含側邊導航欄。已登入時 Header 顯示使用者名稱和登出按鈕，未登入時顯示登入/註冊連結。

#### Scenario: 未登入時顯示

- **WHEN** 未登入的使用者瀏覽前台頁面
- **THEN** Header 顯示系統名稱、登入連結、註冊連結

#### Scenario: 已登入時顯示

- **WHEN** 已登入的會員瀏覽前台頁面
- **THEN** Header 顯示系統名稱、使用者顯示名稱、登出按鈕

### Requirement: 前台登入頁面

前端 SHALL 在 `/login` 路徑提供前台登入頁面，包含 Email 和密碼欄位。登入成功後 MUST 導向 `/profile`。頁面提供「忘記密碼」和「註冊」連結。此頁面僅供一般會員使用。

#### Scenario: 前台登入成功

- **WHEN** 一般會員輸入正確的 Email 和密碼
- **THEN** 系統登入成功，導向 `/profile` 頁面

#### Scenario: 管理員從前台登入

- **WHEN** WebAdmin 或 SystemAdmin 從前台 `/login` 登入
- **THEN** 系統登入成功，導向 `/profile`（管理員也可以用前台查看自己的資料）

#### Scenario: 已登入使用者存取登入頁

- **WHEN** 已登入的使用者存取 `/login`
- **THEN** 系統自動導向 `/profile`

### Requirement: 前台註冊頁面

前端 SHALL 在 `/register` 路徑提供註冊頁面，包含 Email、密碼、確認密碼、顯示名稱欄位。註冊成功後 MUST 導向 `/login` 並顯示成功訊息。

#### Scenario: 註冊成功

- **WHEN** 使用者填寫所有必填欄位且驗證通過
- **THEN** 系統註冊成功，導向 `/login` 並顯示成功訊息

### Requirement: 前台忘記密碼頁面

前端 SHALL 在 `/forgot-password` 路徑提供忘記密碼頁面。

#### Scenario: 送出忘記密碼請求

- **WHEN** 使用者輸入 Email 並送出
- **THEN** 頁面顯示「若該 Email 已註冊，將收到重設密碼信件」

### Requirement: 前台重設密碼頁面

前端 SHALL 在 `/reset-password` 路徑提供重設密碼頁面，從 URL 取得 Token 和 Email。

#### Scenario: 重設密碼成功

- **WHEN** 使用者輸入符合規則的新密碼
- **THEN** 系統重設密碼成功，導向 `/login`

### Requirement: 前台個人資料頁面

前端 SHALL 在 `/profile` 路徑提供個人資料頁面（需登入）。顯示並允許編輯：DisplayName、ChineseName、Birthday、Gender、Address。Email 欄位 MUST 為唯讀不可編輯。另提供變更密碼功能。

#### Scenario: 查看個人資料

- **WHEN** 已登入的會員存取 `/profile`
- **THEN** 頁面顯示會員的所有個人資料，Email 欄位為唯讀

#### Scenario: 編輯個人資料

- **WHEN** 會員修改資料後點擊儲存
- **THEN** 系統更新資料成功，Email 不會被變更

#### Scenario: 未登入存取 Profile

- **WHEN** 未登入的使用者存取 `/profile`
- **THEN** 系統導向 `/login`
