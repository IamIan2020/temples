## Context

這是一個全新的宮廟會員管理系統專案。目前沒有任何既有程式碼或資料庫。系統需要支援會員自助註冊/登入、管理員後台管理會員，以及密碼重設等功能。技術棧已由使用者指定：.NET 10 + Vue 3 + PostgreSQL。

## Goals / Non-Goals

**Goals:**

- 建立可運行的 MVP 會員系統，涵蓋完整的認證與會員管理流程
- 採用業界標準的安全做法（Identity + JWT + PBKDF2）
- 前後端分離架構，後端提供 RESTful API
- 三種角色的權限控制（SystemAdmin / WebAdmin / Member）
- 程式啟動時自動 Migration，簡化部署流程

**Non-Goals:**

- 不做多租戶（Multi-Tenant）架構
- 不做 OAuth / 社群登入（Google、Facebook 等）
- 不做即時通知（WebSocket / SignalR）
- 不做國際化（i18n），僅支援繁體中文
- 不做 Docker 容器化部署
- 不做 CI/CD Pipeline

## Decisions

### 1. 三層架構 vs Clean Architecture

**選擇：三層架構（Api / Core / Infrastructure）**

- 替代方案：Clean Architecture（4-5 層）
- 理由：MVP 階段功能單純，三層架構已足夠。Clean Architecture 的額外抽象層在此階段是過度設計。未來如需擴展可重構。

### 2. ASP.NET Core Identity vs 自建認證

**選擇：ASP.NET Core Identity**

- 替代方案：自建 User/Role 資料表 + 手寫認證邏輯
- 理由：Identity 提供成熟的密碼雜湊、Token 管理、角色系統。自建容易有安全漏洞，且開發時間更長。

### 3. JWT 儲存位置：localStorage vs HttpOnly Cookie

**選擇：localStorage**

- 替代方案：HttpOnly Cookie
- 理由：SPA 架構下 localStorage 較為簡單直覺。搭配 Refresh Token 機制，Access Token 短效期（15 分鐘）降低風險。MVP 階段優先考慮開發效率。

### 4. Email 服務：Gmail SMTP vs 第三方服務

**選擇：MailKit + Gmail SMTP**

- 替代方案：SendGrid、Mailgun 等第三方服務
- 理由：使用者指定。開發和正式環境都用真實寄信，避免開發時用假信件但上線出問題。Gmail SMTP 免費且足夠 MVP 使用。

### 5. 前端 UI 框架：Element Plus vs 其他

**選擇：Element Plus**

- 替代方案：Vuetify、Ant Design Vue、Naive UI
- 理由：使用者指定。中文社群大、元件豐富、文件完善，適合後台管理系統。

### 6. 資料庫 Migration 策略：自動 vs 手動

**選擇：程式啟動時自動執行 Migration**

- 替代方案：手動執行 `dotnet ef database update`
- 理由：簡化部署流程，團隊成員 pull 後啟動即可。MVP 階段單一實例，不會有並發 Migration 問題。

## Risks / Trade-offs

- **[JWT 存 localStorage 有 XSS 風險]** → 透過短效期 Access Token（15 分鐘）+ Refresh Token 降低影響。未來可升級為 HttpOnly Cookie。
- **[Gmail SMTP 有發信上限]** → 每日 500 封對 MVP 足夠。規模增長後切換至 SendGrid 等服務。
- **[自動 Migration 在多實例部署可能衝突]** → MVP 階段為單一實例，不受影響。規模化時改為 CI/CD 執行 Migration。
- **[密碼重設 Token 透過 URL 傳遞]** → Identity 產生的 Token 有時效性（預設 1 天），降低被濫用風險。
- **[種子帳號密碼寫在程式碼中]** → 僅用於開發環境，正式上線前需變更。在文件中明確標註警告。
