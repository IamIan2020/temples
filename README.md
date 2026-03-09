# 宮廟會員系統 (Temple Membership System)

## 技術棧

| 項目 | 技術 |
|------|------|
| 後端 | .NET 10 Web API |
| 前端 | Vue 3 + TypeScript + Vite |
| 資料庫 | PostgreSQL |
| UI 框架 | Element Plus |
| 認證 | ASP.NET Core Identity + JWT |
| ORM | Entity Framework Core |
| Email | MailKit + Gmail SMTP |

## 前置條件

- .NET 10 SDK
- Node.js 18+
- PostgreSQL

## 快速開始

### 1. 設定資料庫連線

```bash
cd src/Temples.Api
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=temples;Username=postgres;Password=YOUR_PASSWORD"
dotnet user-secrets set "Jwt:Key" "YourSuperSecretKeyAtLeast32Characters!"
```

### 2. 設定 Email（選用，密碼重設功能需要）

```bash
dotnet user-secrets set "Email:SmtpHost" "smtp.gmail.com"
dotnet user-secrets set "Email:SmtpPort" "587"
dotnet user-secrets set "Email:SenderEmail" "your-email@gmail.com"
dotnet user-secrets set "Email:SenderName" "宮廟系統"
dotnet user-secrets set "Email:Password" "xxxx-xxxx-xxxx-xxxx"
```

### 3. 啟動後端

```bash
cd src/Temples.Api
dotnet run
```

後端會自動執行 Migration 並建立種子資料（角色 + 預設管理員）。

Swagger UI: https://localhost:5001/swagger

### 4. 啟動前端

```bash
cd frontend
npm install
npm run dev
```

前端: http://localhost:5173

### 預設帳號

| 帳號 | 密碼 | 角色 |
|------|------|------|
| ianadmin | my0919linda! | SystemAdmin |

> ⚠️ 正式上線前請變更預設密碼。

## 專案結構

```
temples/
├── src/
│   ├── Temples.Api/          # Web API 進入點
│   ├── Temples.Core/         # 核心商業邏輯
│   └── Temples.Infrastructure/ # 資料存取層
├── tests/
│   └── Temples.Tests/        # 測試專案
└── frontend/                 # Vue 3 前端
```

## API 端點

### 認證 `/api/auth`

| 方法 | 路徑 | 說明 |
|------|------|------|
| POST | /api/auth/register | 註冊 |
| POST | /api/auth/login | 登入 |
| POST | /api/auth/refresh-token | 刷新 Token |
| POST | /api/auth/forgot-password | 忘記密碼 |
| POST | /api/auth/reset-password | 重設密碼 |

### 會員 `/api/members`

| 方法 | 路徑 | 說明 | 權限 |
|------|------|------|------|
| GET | /api/members/me | 取得自己的 Profile | 登入 |
| PUT | /api/members/me | 更新自己的 Profile | 登入 |
| PUT | /api/members/me/password | 變更密碼 | 登入 |
| GET | /api/members | 會員列表 | WebAdmin+ |
| GET | /api/members/{id} | 會員詳情 | WebAdmin+ |
| PUT | /api/members/{id}/role | 變更角色 | SystemAdmin |
| PUT | /api/members/{id}/status | 啟用/停用 | WebAdmin+ |
| DELETE | /api/members/{id} | 停用會員 | WebAdmin+ |
