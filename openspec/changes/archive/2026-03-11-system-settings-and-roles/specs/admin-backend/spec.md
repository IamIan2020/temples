## MODIFIED Requirements

### Requirement: 後台 Layout

前端 SHALL 提供 `AdminLayout.vue` 元件，包含 Header（系統名稱 + 「管理後台」標示、管理員名稱、登出按鈕）和側邊導航欄。側邊導航 MUST 包含：

- 會員管理（需 members.view 權限才顯示）
- 系統管理（子選單，僅 SystemAdmin 顯示）
  - 系統設定（`/backstage/settings`）
  - 權限群組（`/backstage/roles`）

系統名稱 MUST 從 Settings Store 取得，不可硬編碼。AdminLayout MUST 整合 IdleWarningDialog 元件，已登入時啟動閒置偵測。

#### Scenario: SystemAdmin 後台導航

- **WHEN** SystemAdmin 登入後台
- **THEN** 側邊導航顯示「會員管理」和「系統管理」子選單（含系統設定、權限群組）

#### Scenario: WebAdmin 後台導航

- **WHEN** WebAdmin 登入後台
- **THEN** 側邊導航顯示「會員管理」，不顯示「系統管理」子選單

#### Scenario: 自訂角色後台導航

- **WHEN** 自訂角色的使用者登入後台
- **THEN** 側邊導航根據該角色擁有的權限動態顯示對應選單

#### Scenario: 後台閒置警告

- **WHEN** 管理員在後台閒置達警告時間
- **THEN** 顯示 IdleWarningDialog 閒置警告

### Requirement: 後台路由守衛

後台路由 `/backstage/*` MUST 設定導航守衛，僅允許擁有管理權限的角色存取。未登入使用者 MUST 導向 `/backstage/login`。無任何管理權限的角色 MUST 導向前台 `/profile` 並顯示提示。

#### Scenario: 未登入存取後台

- **WHEN** 未登入的使用者存取 `/backstage/members`
- **THEN** 系統導向 `/backstage/login`

#### Scenario: 無管理權限存取後台

- **WHEN** 無任何管理權限的使用者存取 `/backstage/members`
- **THEN** 系統導向 `/profile`
