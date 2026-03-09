# 宮廟會員系統 (Temple Membership System) - MVP 實作計畫

## Context

建立一個宮廟管理系統，首先從會員系統開始。目標是讓會員可以透過 Email 註冊、登入、找回密碼，後台管理員可以查詢會員資料。這是全新專案，目錄目前為空。

## 技術選型

| 項目 | 選擇 | 理由 |
|------|------|------|
| 後端 | .NET 10 Web API | 使用者指定，LTS 版本 |
| 前端 | Vue 3 + TypeScript + Vite | 使用者指定 |
| 資料庫 | **PostgreSQL** | 使用者指定 |
| 架構 | 三層架構 (Api / Core / Infrastructure) | MVP 夠用，不過度設計 |
| 使用者管理 | ASP.NET Core Identity | 安全成熟，免造輪子 |
| 認證 | JWT + Refresh Token | SPA 標準做法 |
| ORM | Entity Framework Core | 與 Identity 無縫整合 |
| 前端 UI | Element Plus | 中文社群大、元件豐富 |
| 狀態管理 | Pinia | Vue 3 官方推薦 |
| Email | MailKit + Gmail SMTP | 開發與正式都用真實寄信 |
| 驗證 | FluentValidation | 比 DataAnnotations 更彈性 |

---

## 專案結構

### 後端

```
E:\Web\temples\
├── temples.sln
├── src\
│   ├── Temples.Api\                        # Web API 進入點
│   │   ├── Controllers\
│   │   │   ├── AuthController.cs
│   │   │   └── MembersController.cs
│   │   ├── Middleware\
│   │   │   └── ExceptionHandlingMiddleware.cs
│   │   ├── Program.cs
│   │   ├── appsettings.json
│   │   └── appsettings.Development.json
│   │
│   ├── Temples.Core\                       # 核心商業邏輯 + 實體
│   │   ├── Entities\
│   │   │   └── ApplicationUser.cs          # 繼承 IdentityUser
│   │   ├── DTOs\
│   │   │   ├── Auth\                       # Login/Register/ForgotPassword/ResetPassword
│   │   │   └── Members\                    # Profile/Update/Search
│   │   ├── Interfaces\
│   │   │   ├── IAuthService.cs
│   │   │   ├── IMemberService.cs
│   │   │   └── IEmailService.cs
│   │   └── Services\
│   │       ├── AuthService.cs
│   │       ├── MemberService.cs
│   │       └── EmailService.cs
│   │
│   └── Temples.Infrastructure\             # 資料存取
│       ├── Data\
│       │   ├── AppDbContext.cs
│       │   └── Migrations\
│       ├── Configurations\
│       │   └── ApplicationUserConfiguration.cs
│       └── DependencyInjection.cs
│
└── tests\
    └── Temples.Tests\
```

### 前端

```
E:\Web\temples\frontend\
├── src\
│   ├── api\                                # API 呼叫層
│   │   ├── client.ts                       # Axios 實例 + JWT interceptor
│   │   ├── auth.ts
│   │   └── members.ts
│   ├── types\                              # TypeScript 類型（對應後端 DTO）
│   ├── stores\                             # Pinia stores
│   │   └── auth.ts
│   ├── views\
│   │   ├── LoginView.vue
│   │   ├── RegisterView.vue
│   │   ├── ForgotPasswordView.vue
│   │   ├── ResetPasswordView.vue
│   │   └── ProfileView.vue
│   ├── components\layout\
│   ├── router\index.ts
│   └── App.vue
```

---

## 資料庫設計

使用 ASP.NET Core Identity 自動管理使用者資料表，擴充 `ApplicationUser`：

```csharp
public class ApplicationUser : IdentityUser
{
    public string DisplayName { get; set; }        // 顯示名稱
    public string? ChineseName { get; set; }       // 中文姓名
    public DateOnly? Birthday { get; set; }        // 生日
    public string? Gender { get; set; }            // 性別
    public string? Address { get; set; }           // 地址
    public string? MemberNumber { get; set; }      // 會員編號（預留）
    public DateTime? JoinDate { get; set; }        // 加入日期
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
}
```

Identity 自動建立：AspNetUsers, AspNetRoles, AspNetUserRoles, AspNetUserTokens, AspNetUserClaims

---

## API 端點

### 認證 `/api/auth`

| 方法 | 路徑 | 說明 | 需認證 |
|------|------|------|--------|
| POST | `/api/auth/register` | 註冊 | 否 |
| POST | `/api/auth/login` | 登入，回傳 JWT | 否 |
| POST | `/api/auth/forgot-password` | 寄密碼重設信 | 否 |
| POST | `/api/auth/reset-password` | 重設密碼 | 否 |
| POST | `/api/auth/refresh-token` | 更新 JWT | 否 |

### 會員 `/api/members`

| 方法 | 路徑 | 說明 | 需認證 |
|------|------|------|--------|
| GET | `/api/members/me` | 取得自己的 profile | 是 |
| PUT | `/api/members/me` | 更新自己的 profile | 是 |
| PUT | `/api/members/me/password` | 變更密碼 | 是 |
| GET | `/api/members` | 會員列表（分頁+搜尋） | 是（Admin） |
| GET | `/api/members/{id}` | 取得指定會員 | 是（Admin） |
| DELETE | `/api/members/{id}` | 停用會員 | 是（Admin） |

### 統一回應格式

```json
{ "success": true, "data": { ... }, "message": null, "errors": [] }
```

---

## 認證流程

- **登入**：Email + Password → 後端 Identity 驗證 → 產生 JWT Access Token (15 分鐘) + Refresh Token (7 天) → 前端存 localStorage
- **密碼重設**：輸入 Email → 後端產生 reset token → 寄信 → 使用者點連結 → 輸入新密碼 → 驗證 token 並重設

---

## 關鍵套件

### 後端 NuGet

- `Npgsql.EntityFrameworkCore.PostgreSQL` — PostgreSQL EF Core provider
- `Microsoft.AspNetCore.Authentication.JwtBearer` — JWT 認證
- `Microsoft.AspNetCore.Identity.EntityFrameworkCore` — Identity + EF Core
- `Microsoft.EntityFrameworkCore.Design` — Migrations 工具
- `FluentValidation.DependencyInjectionExtensions` — 請求驗證
- `MailKit` — 寄送 email
- `Swashbuckle.AspNetCore` — Swagger

### 前端 npm

- `vue`, `vue-router`, `pinia`, `axios`
- `element-plus`, `@element-plus/icons-vue`
- `unplugin-auto-import`, `unplugin-vue-components`

---

## 實作順序

### Phase 1: 專案建立與基礎架構
1. 初始化 git repo + .gitignore
2. 建立 Solution + 三個後端專案 (Api / Core / Infrastructure)
3. 設定專案參考關係
4. 安裝 NuGet 套件
5. 建立 ApplicationUser 實體
6. 建立 AppDbContext (繼承 IdentityDbContext)
7. 設定 Program.cs（Identity, JWT, Swagger, CORS, PostgreSQL 連線）
8. 建立首次 Migration
9. 種子資料（預設 Admin 角色 + 帳號）

### Phase 2: 後端認證 API
1. 建立 Auth DTOs
2. 實作 IAuthService + AuthService（封裝 Identity 操作 + JWT 產生）
3. 實作 AuthController
4. 實作 IEmailService + EmailService（使用 MailKit + Gmail SMTP）
5. 加入 FluentValidation
6. 測試認證 API

### Phase 3: 後端會員 API
1. 建立 Member DTOs
2. 實作 IMemberService + MemberService
3. 實作 MembersController
4. 加入 Authorization Policy（Admin 限制）
5. 實作 ExceptionHandlingMiddleware
6. 測試會員 API

### Phase 4: 前端建立
1. 用 Vite 初始化 Vue 3 + TypeScript 專案
2. 安裝套件（Element Plus, Axios, Pinia）
3. 設定 Vite proxy 代理後端 API
4. 建立 Axios client + JWT interceptor
5. 建立 TypeScript 類型（對應後端 DTO）
6. 設定 Router + 導航守衛
7. 建立 Pinia auth store

### Phase 5: 前端頁面
1. Layout 元件（Header + Footer）
2. 登入頁面
3. 註冊頁面
4. 忘記密碼頁面
5. 重設密碼頁面
6. 會員個人資料頁面
7. 前後端聯調測試

### Phase 6: 收尾
1. 完整流程測試
2. README.md（啟動步驟）

---

## 驗證方式

1. **後端**：`dotnet build` 編譯成功 → `dotnet run` 啟動 → Swagger UI 測試所有 API
2. **前端**：`npm run build` 編譯成功 → `npm run dev` 啟動 → 測試完整登入/註冊/密碼重設流程
3. **整合**：前後端同時啟動，透過前端 UI 走完所有流程

## 資料庫連線與自動 Migration

**連線資訊**（透過 User Secrets 管理，不進版控）：
- Host: localhost
- Port: 5432
- Database: temples
- Username: postgres
- Password: （透過 User Secrets 設定）

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=temples;Username=postgres;Password=YOUR_PASSWORD"
```

**自動 Migration**：程式啟動時自動套用，在 `Program.cs` 中加入：

```csharp
// 啟動時自動執行 Migration
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();  // 自動套用所有未執行的 migration
}
```

**Migration 工作流程**（進版控）：
1. 修改 Entity → `dotnet ef migrations add <MigrationName>` → 產生 Migration 檔案
2. Migration 檔案在 `Temples.Infrastructure/Data/Migrations/` 目錄下，**納入 git 版控**
3. 程式啟動時自動偵測並套用 → 資料庫 schema 自動更新
4. 團隊其他人 git pull 後重啟程式即自動更新 DB

---

## Email 設定（Gmail SMTP）

開發與正式環境都使用 Gmail SMTP 寄送真實信件。

**前置準備**：
1. 準備一個 Gmail 帳號
2. 開啟 Google 帳號的「兩步驟驗證」
3. 到 https://myaccount.google.com/apppasswords 產生「應用程式密碼」

**設定方式**：使用 .NET User Secrets（不進版控）

```bash
dotnet user-secrets set "Email:SmtpHost" "smtp.gmail.com"
dotnet user-secrets set "Email:SmtpPort" "587"
dotnet user-secrets set "Email:SenderEmail" "your-email@gmail.com"
dotnet user-secrets set "Email:SenderName" "宮廟系統"
dotnet user-secrets set "Email:Password" "xxxx-xxxx-xxxx-xxxx"  # 應用程式密碼
```

`appsettings.json` 只放結構，不放敏感值：
```json
{
  "Email": {
    "SmtpHost": "",
    "SmtpPort": 587,
    "SenderEmail": "",
    "SenderName": "宮廟系統",
    "Password": ""
  }
}
```

## 前置條件

- 確認 .NET 10 SDK 已安裝
- 確認 Node.js 已安裝
- 確認 PostgreSQL 已安裝且可連線
- 確認 Gmail 帳號已開啟兩步驟驗證並產生應用程式密碼
