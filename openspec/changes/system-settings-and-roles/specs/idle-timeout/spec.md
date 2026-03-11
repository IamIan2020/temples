## ADDED Requirements

### Requirement: 閒置偵測 Composable

前端 SHALL 提供 `useIdleTimeout` composable，監聽使用者活動事件（mousemove、keydown、click、scroll、touchstart）。閒置達設定時間前 1 分鐘 MUST 顯示警告，達設定時間 MUST 自動登出。

#### Scenario: 使用者持續操作

- **WHEN** 使用者在閒置時間內有任何操作（點擊、打字、滾動等）
- **THEN** 閒置計時器重置，不會觸發警告或登出

#### Scenario: 閒置達警告時間

- **WHEN** 使用者閒置達 (sessionTimeoutMinutes - 1) 分鐘
- **THEN** 系統顯示 IdleWarningDialog 警告對話框

#### Scenario: 閒置達登出時間

- **WHEN** 使用者閒置達 sessionTimeoutMinutes 分鐘且未回應警告
- **THEN** 系統自動執行登出，導向對應的登入頁面

### Requirement: 閒置警告對話框

前端 SHALL 提供 `IdleWarningDialog` 元件，使用 el-dialog 顯示。MUST 包含倒數計時顯示（剩餘秒數）、「繼續使用」按鈕（重置計時器）、「立即登出」按鈕。

#### Scenario: 點擊繼續使用

- **WHEN** 使用者在警告對話框點擊「繼續使用」
- **THEN** 閒置計時器重置，對話框關閉

#### Scenario: 點擊立即登出

- **WHEN** 使用者在警告對話框點擊「立即登出」
- **THEN** 系統立即登出並導向登入頁面

#### Scenario: 倒數計時歸零

- **WHEN** 警告對話框的倒數計時歸零（60 秒）
- **THEN** 系統自動登出

### Requirement: 跨 Tab 登出同步

前端 SHALL 監聽 localStorage 的 storage 事件。當一個 Tab 登出時，其他 Tab MUST 也同步登出。

#### Scenario: 一個 Tab 登出

- **WHEN** 使用者在 Tab A 登出（accessToken 從 localStorage 移除）
- **THEN** Tab B 偵測到 storage 事件，自動執行登出並導向登入頁面

### Requirement: Layout 整合閒置偵測

AdminLayout 和 PublicLayout MUST 在使用者已登入時啟動閒置偵測，並包含 IdleWarningDialog 元件。登出時根據所在 Layout 導向對應的登入頁面（前台 → `/login`，後台 → `/backstage/login`）。

#### Scenario: 後台閒置登出

- **WHEN** 管理員在後台閒置超時
- **THEN** 系統自動登出，導向 `/backstage/login`

#### Scenario: 前台閒置登出

- **WHEN** 會員在前台閒置超時
- **THEN** 系統自動登出，導向 `/login`
