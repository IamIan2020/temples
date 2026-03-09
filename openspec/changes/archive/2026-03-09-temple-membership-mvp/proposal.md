## Why

宮廟目前缺乏數位化的會員管理系統，會員資料以紙本或試算表管理，難以追蹤、查詢和維護。需要建立一套線上會員系統，讓會員自行註冊、管理個人資料，管理員能有效率地管理會員。這是全新專案的第一步，從最核心的會員系統開始建立。

## What Changes

- 建立全新的 .NET 10 Web API 後端，採用三層架構（Api / Core / Infrastructure）
- 使用 ASP.NET Core Identity 管理使用者認證與角色權限
- 實作 JWT + Refresh Token 認證機制
- 實作會員註冊、登入、忘記密碼、重設密碼功能
- 實作會員個人資料 CRUD
- 實作後台管理員的會員查詢、啟用/停用、角色管理功能
- 建立 Vue 3 + TypeScript + Vite 前端，使用 Element Plus UI 框架
- 實作前端所有認證頁面與會員管理頁面
- 使用 PostgreSQL 資料庫，透過 EF Core 管理
- 使用 MailKit + Gmail SMTP 寄送密碼重設信件
- 建立三種角色：SystemAdmin、WebAdmin、Member，並實作權限控制

## Capabilities

### New Capabilities

- `user-auth`: 使用者認證功能，包含註冊、登入、JWT Token 管理、忘記密碼、重設密碼
- `member-management`: 會員管理功能，包含個人資料 CRUD、後台會員列表/搜尋/詳情、啟用停用、角色變更
- `project-foundation`: 專案基礎架構，包含 Solution 結構、資料庫設定、Identity 設定、種子資料、中介軟體
- `frontend-app`: 前端應用程式，包含 Vue 3 專案架構、路由、狀態管理、API 整合、所有頁面

### Modified Capabilities

（無，這是全新專案）

## Impact

- **新增程式碼**：後端三個專案 + 測試專案 + 前端 Vue 專案
- **資料庫**：PostgreSQL 建立 Identity 相關資料表 + 擴充的 ApplicationUser 欄位
- **外部依賴**：Gmail SMTP 服務（寄送密碼重設信）
- **API**：建立 `/api/auth/*` 和 `/api/members/*` 兩組 API 端點
- **部署需求**：需要 .NET 10 SDK、Node.js、PostgreSQL
