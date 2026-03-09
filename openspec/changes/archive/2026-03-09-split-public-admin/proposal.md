## Why

目前系統只有單一登入頁面，所有角色（Member、WebAdmin、SystemAdmin）共用相同入口和 Layout。這會導致：
1. 一般會員看到不需要的後台管理選單
2. 後台管理入口暴露在公開路徑，容易被發現
3. 前台（公開面向）和後台（管理面向）的使用體驗無法獨立設計

需要將前台會員區域與後台管理區域完全分離，提供各自的登入頁面、Layout 和路由結構。

## What Changes

- 新增前台 Layout（公開區域），包含：會員登入、註冊、忘記密碼、重設密碼、個人資料檢視/編輯
- 新增後台 Layout（管理區域），包含：管理員登入、會員列表、會員詳情管理
- 後台入口使用隱晦路徑（如 `/backstage`），不使用常見的 `/admin`
- 前台登入後只顯示會員自己的基本資料頁面，可編輯（Email 不可變更）
- 後台登入僅限 WebAdmin / SystemAdmin 角色
- 一般會員嘗試進入後台時導向前台登入頁
- 重構現有路由結構，分為 `/`（前台）和 `/backstage`（後台）兩大區塊

## Capabilities

### New Capabilities
- `public-frontend`: 前台公開區域，包含會員登入/註冊/忘記密碼/重設密碼/個人資料頁面，使用獨立 Layout
- `admin-backend`: 後台管理區域，使用隱晦路徑入口，包含管理員登入/會員管理功能，使用獨立 Layout

### Modified Capabilities
- `frontend-app`: 路由結構重構，從單一 Layout 改為前後台分離架構
- `user-auth`: 登入邏輯需區分前台會員與後台管理員，角色驗證強化

## Impact

- **前端路由**：完全重構 `router/index.ts`，分為前台和後台兩組路由
- **前端 Layout**：新增 `PublicLayout.vue`（前台）和 `AdminLayout.vue`（後台），取代現有單一 Layout
- **前端頁面**：現有頁面重新歸類到前台或後台目錄
- **導航守衛**：加強角色檢查，後台路由僅限 WebAdmin/SystemAdmin
- **後端 API**：不需要修改，現有 API 和權限控制已滿足需求
