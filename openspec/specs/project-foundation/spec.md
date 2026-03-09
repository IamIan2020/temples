## ADDED Requirements

### Requirement: Solution 與專案結構

系統 SHALL 採用三層架構，包含以下專案：
- `Temples.Api`：Web API 進入點，包含 Controllers 和 Middleware
- `Temples.Core`：核心商業邏輯，包含 Entities、DTOs、Interfaces、Services
- `Temples.Infrastructure`：資料存取層，包含 DbContext、Configurations、Migrations
- `Temples.Tests`：測試專案

專案參考關係：
- Api → Core, Infrastructure
- Infrastructure → Core
- Tests → Api, Core, Infrastructure

#### Scenario: Solution 建立成功

- **WHEN** 執行 `dotnet build` 於 Solution 根目錄
- **THEN** 所有專案編譯成功且無錯誤

### Requirement: PostgreSQL 資料庫連線

系統 SHALL 使用 PostgreSQL 作為資料庫，透過 EF Core + Npgsql provider 連線。連線字串 MUST 透過 .NET User Secrets 管理，不可寫入版控。

#### Scenario: 資料庫連線設定

- **WHEN** 開發者設定 User Secrets 中的 `ConnectionStrings:DefaultConnection`
- **THEN** 系統能透過 EF Core 成功連線至 PostgreSQL 資料庫

### Requirement: ASP.NET Core Identity 設定

系統 SHALL 使用 ASP.NET Core Identity 管理使用者認證。AppDbContext MUST 繼承 `IdentityDbContext<ApplicationUser>`。

#### Scenario: Identity 服務註冊

- **WHEN** 應用程式啟動
- **THEN** Identity 服務已註冊，包含 UserManager、RoleManager、SignInManager

### Requirement: ApplicationUser 實體

系統 SHALL 定義 `ApplicationUser` 繼承 `IdentityUser`，擴充以下欄位：
- `DisplayName` (string, 必填)：顯示名稱
- `ChineseName` (string?, 選填)：中文姓名
- `Birthday` (DateOnly?, 選填)：生日
- `Gender` (string?, 選填)：性別
- `Address` (string?, 選填)：地址
- `MemberNumber` (string?, 選填)：會員編號
- `JoinDate` (DateTime?, 選填)：加入日期
- `CreatedAt` (DateTime, 必填)：建立時間
- `UpdatedAt` (DateTime?, 選填)：更新時間
- `IsActive` (bool, 預設 true)：是否啟用

#### Scenario: ApplicationUser 欄位完整

- **WHEN** 查看 ApplicationUser 實體定義
- **THEN** 包含上述所有擴充欄位，且類型正確

### Requirement: 自動 Migration

系統 SHALL 在程式啟動時自動執行未套用的 EF Core Migration。Migration 檔案 MUST 納入 git 版控。

#### Scenario: 程式啟動時自動 Migration

- **WHEN** 應用程式啟動且資料庫有未套用的 Migration
- **THEN** 系統自動執行所有未套用的 Migration，資料庫 schema 更新完成

### Requirement: 種子資料

系統 SHALL 在啟動時建立預設角色與管理員帳號：
- 角色：SystemAdmin、WebAdmin、Member
- 預設帳號：ianadmin（SystemAdmin 角色）

#### Scenario: 首次啟動建立種子資料

- **WHEN** 應用程式首次啟動且資料庫為空
- **THEN** 系統建立三個角色和一個 SystemAdmin 帳號

#### Scenario: 重複啟動不重建種子資料

- **WHEN** 應用程式再次啟動且種子資料已存在
- **THEN** 系統跳過種子資料建立，不產生錯誤

### Requirement: Swagger API 文件

系統 SHALL 在開發環境啟用 Swagger UI，供開發者測試 API。

#### Scenario: Swagger UI 可存取

- **WHEN** 開發者在瀏覽器存取 `/swagger`
- **THEN** 顯示所有 API 端點的互動式文件

### Requirement: CORS 設定

系統 SHALL 設定 CORS 允許前端開發伺服器（localhost）存取 API。

#### Scenario: 前端跨域請求

- **WHEN** 前端從 `localhost:5173` 發送 API 請求
- **THEN** 請求不被 CORS 政策阻擋

### Requirement: 統一回應格式

所有 API 回應 SHALL 使用統一格式：`{ success, data, message, errors }`。

#### Scenario: 成功回應

- **WHEN** API 請求成功
- **THEN** 回傳 `{ "success": true, "data": {...}, "message": null, "errors": [] }`

#### Scenario: 失敗回應

- **WHEN** API 請求失敗
- **THEN** 回傳 `{ "success": false, "data": null, "message": "錯誤訊息", "errors": ["詳細錯誤"] }`

### Requirement: 全域例外處理

系統 SHALL 提供 ExceptionHandlingMiddleware，攔截未處理的例外並回傳統一格式的錯誤回應。

#### Scenario: 未處理例外

- **WHEN** API 處理過程中發生未預期的例外
- **THEN** 中介軟體攔截例外，回傳 HTTP 500 + 統一錯誤格式，不洩漏內部堆疊資訊
