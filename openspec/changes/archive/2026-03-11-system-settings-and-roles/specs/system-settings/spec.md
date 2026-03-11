## ADDED Requirements

### Requirement: SystemSetting 實體

系統 SHALL 提供 `SystemSetting` 實體，採用單一列表設計（Id=1），包含欄位：CompanyName（公司名稱）、WebsiteName（網站名稱）、Phone（電話）、TaxId（統編）、Copyright（版權文字）、SessionTimeoutMinutes（閒置登出分鐘數，預設 30）、UpdatedAt（更新時間）。

#### Scenario: 預設設定存在

- **WHEN** 系統首次啟動
- **THEN** 資料庫中 SHALL 存在一筆預設 SystemSetting（Id=1），CompanyName 和 WebsiteName 預設為「宮廟系統」，SessionTimeoutMinutes 預設為 30

### Requirement: 公開設定 API

系統 SHALL 提供 `GET /api/system-settings/public` 端點，不需認證即可存取。回傳 PublicSettingResponse，包含 websiteName、copyright、sessionTimeoutMinutes。

#### Scenario: 匿名取得公開設定

- **WHEN** 任何使用者（含未登入）呼叫 `GET /api/system-settings/public`
- **THEN** 系統回傳 HTTP 200 和 PublicSettingResponse

### Requirement: 管理設定 API - 查詢

系統 SHALL 提供 `GET /api/system-settings` 端點，僅 SystemAdmin 可存取。回傳完整的 SystemSettingResponse（所有欄位）。

#### Scenario: SystemAdmin 查詢設定

- **WHEN** SystemAdmin 呼叫 `GET /api/system-settings`
- **THEN** 系統回傳 HTTP 200 和完整的 SystemSettingResponse

#### Scenario: 非 SystemAdmin 查詢設定

- **WHEN** WebAdmin 或 Member 呼叫 `GET /api/system-settings`
- **THEN** 系統回傳 HTTP 403

### Requirement: 管理設定 API - 更新

系統 SHALL 提供 `PUT /api/system-settings` 端點，僅 SystemAdmin 可存取。接受 UpdateSystemSettingRequest，驗證欄位後更新設定。

#### Scenario: 更新設定成功

- **WHEN** SystemAdmin 提供有效的 UpdateSystemSettingRequest
- **THEN** 系統更新設定，回傳 HTTP 200 和更新後的 SystemSettingResponse

#### Scenario: 驗證失敗

- **WHEN** CompanyName 或 WebsiteName 為空
- **THEN** 系統回傳 HTTP 400 和驗證錯誤

#### Scenario: SessionTimeoutMinutes 範圍驗證

- **WHEN** SessionTimeoutMinutes 小於 1 或大於 480
- **THEN** 系統回傳 HTTP 400 和驗證錯誤

### Requirement: 系統設定管理頁面

前端 SHALL 在 `/backstage/settings` 提供系統設定管理頁面，僅 SystemAdmin 可存取。頁面 MUST 使用 el-form 顯示所有設定欄位，儲存後即時更新前台/後台的網站名稱和 Copyright。

#### Scenario: 查看系統設定

- **WHEN** SystemAdmin 存取 `/backstage/settings`
- **THEN** 頁面顯示所有設定欄位的目前值

#### Scenario: 儲存系統設定

- **WHEN** SystemAdmin 修改設定並點擊儲存
- **THEN** 系統儲存成功，前台和後台的網站名稱即時更新

### Requirement: Settings Pinia Store

前端 SHALL 提供 `useSettingsStore`，在應用程式初始化時從 `/api/system-settings/public` 載入公開設定。Store MUST 提供 websiteName、copyright、sessionTimeoutMinutes。載入的設定 SHALL 快取到 localStorage。

#### Scenario: 應用程式初始化載入設定

- **WHEN** 前端應用程式啟動
- **THEN** Settings Store 從 API 載入公開設定，快取到 localStorage

#### Scenario: API 不可用時使用快取

- **WHEN** 公開設定 API 無法連線
- **THEN** Settings Store 使用 localStorage 中的快取設定
