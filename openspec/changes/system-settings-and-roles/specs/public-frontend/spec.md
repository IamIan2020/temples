## MODIFIED Requirements

### Requirement: 前台 Layout

前端 SHALL 提供 `PublicLayout.vue` 元件，包含簡潔的 Header（系統名稱、使用者名稱/登入按鈕、登出按鈕）。系統名稱 MUST 從 Settings Store 取得，不可硬編碼。不包含側邊導航欄。已登入時 Header 顯示使用者名稱和登出按鈕，未登入時顯示登入/註冊連結。PublicLayout MUST 整合 IdleWarningDialog 元件，已登入時啟動閒置偵測。

#### Scenario: 未登入時顯示

- **WHEN** 未登入的使用者瀏覽前台頁面
- **THEN** Header 顯示 Settings Store 的網站名稱、登入連結、註冊連結

#### Scenario: 已登入時顯示

- **WHEN** 已登入的會員瀏覽前台頁面
- **THEN** Header 顯示 Settings Store 的網站名稱、使用者顯示名稱、登出按鈕

#### Scenario: 前台閒置警告

- **WHEN** 會員在前台閒置達警告時間
- **THEN** 顯示 IdleWarningDialog 閒置警告
