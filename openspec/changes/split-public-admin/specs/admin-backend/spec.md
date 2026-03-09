## ADDED Requirements

### Requirement: 後台 Layout

前端 SHALL 提供 `AdminLayout.vue` 元件，包含 Header（系統名稱 + 「管理後台」標示、管理員名稱、登出按鈕）和側邊導航欄。側邊導航 MUST 包含：會員管理連結。SystemAdmin 額外顯示系統設定相關選單（預留）。

#### Scenario: WebAdmin 後台導航

- **WHEN** WebAdmin 登入後台
- **THEN** 側邊導航顯示「會員管理」

#### Scenario: SystemAdmin 後台導航

- **WHEN** SystemAdmin 登入後台
- **THEN** 側邊導航顯示「會員管理」

### Requirement: 後台登入頁面

前端 SHALL 在 `/backstage/login` 路徑提供後台登入頁面。此頁面僅供 WebAdmin 和 SystemAdmin 使用。登入成功後 MUST 驗證角色，非管理員角色 MUST 顯示錯誤訊息「您沒有後台管理權限」並拒絕登入。

#### Scenario: 管理員後台登入成功

- **WHEN** WebAdmin 或 SystemAdmin 輸入正確的 Email 和密碼
- **THEN** 系統登入成功，導向 `/backstage/members`

#### Scenario: 一般會員嘗試後台登入

- **WHEN** Member 角色的使用者在後台登入頁輸入正確帳密
- **THEN** 系統顯示錯誤訊息「您沒有後台管理權限」，不允許進入後台

#### Scenario: 已登入管理員存取後台登入頁

- **WHEN** 已登入的管理員存取 `/backstage/login`
- **THEN** 系統自動導向 `/backstage/members`

### Requirement: 後台會員列表頁面

前端 SHALL 在 `/backstage/members` 路徑提供會員列表頁面，包含分頁表格和搜尋功能。表格顯示：Email、DisplayName、角色、狀態、建立時間。

#### Scenario: 查看會員列表

- **WHEN** 管理員存取 `/backstage/members`
- **THEN** 頁面顯示分頁的會員列表

#### Scenario: 搜尋會員

- **WHEN** 管理員輸入搜尋關鍵字
- **THEN** 列表篩選符合條件的會員

### Requirement: 後台會員詳情頁面

前端 SHALL 在 `/backstage/members/:id` 路徑提供會員詳情頁面。WebAdmin 可啟用/停用會員。SystemAdmin 額外可變更會員角色。

#### Scenario: 查看會員詳情

- **WHEN** 管理員點擊列表中的會員
- **THEN** 頁面顯示該會員的完整資料

#### Scenario: 停用會員

- **WHEN** 管理員點擊停用按鈕
- **THEN** 系統停用該會員，頁面更新狀態

#### Scenario: 變更角色

- **WHEN** SystemAdmin 變更會員角色
- **THEN** 系統更新角色，頁面顯示新角色

### Requirement: 後台路由守衛

後台路由 `/backstage/*` MUST 設定導航守衛，僅允許 WebAdmin 和 SystemAdmin 存取。未登入使用者 MUST 導向 `/backstage/login`。Member 角色 MUST 導向前台 `/profile` 並顯示提示。

#### Scenario: 未登入存取後台

- **WHEN** 未登入的使用者存取 `/backstage/members`
- **THEN** 系統導向 `/backstage/login`

#### Scenario: Member 存取後台

- **WHEN** Member 角色的使用者存取 `/backstage/members`
- **THEN** 系統導向 `/profile`
