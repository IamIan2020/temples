## Context

宮廟會員系統 MVP 已完成基本的會員註冊/登入/管理功能，使用 .NET 10 + Vue 3 + PostgreSQL。目前角色為硬編碼的 SystemAdmin/WebAdmin/Member，網站名稱硬編碼在前端，無閒置登出機制。系統需要更靈活的設定與權限管理能力。

現有架構：Clean Architecture（Temples.Api / Temples.Core / Temples.Infrastructure），ASP.NET Core Identity + JWT，前端使用 Pinia + Vue Router + Element Plus。

## Goals / Non-Goals

**Goals:**
- 後台可管理網站基本資訊，前端動態套用
- 可設定閒置登出時間，前端自動偵測並登出
- 可新增自訂角色並設定細粒度權限
- 現有功能改用 Policy-based 授權，權限可透過角色管理調整

**Non-Goals:**
- 不實作頁面級或欄位級的權限控制（僅 API + 選單層級）
- 不實作多租戶系統設定
- 不實作權限繼承或角色層級關係
- 不實作即時權限變更推播（使用者需重新登入取得新權限）

## Decisions

### 1. SystemSetting 使用單一列表（Single Row Table）

**選擇**: 一個 SystemSetting 實體，所有欄位為 column，Id 永遠為 1。
**替代方案**: Key-Value 表（每個設定一行）。
**理由**: 設定欄位數量少且固定（6 個），強型別比 key-value 更安全，編譯期即可發現錯誤。

### 2. 擴充 IdentityRole 為 ApplicationRole

**選擇**: 建立 `ApplicationRole : IdentityRole`，加入 Description 和 IsBuiltIn 欄位。
**替代方案**: 另建 RoleMetadata 表關聯 IdentityRole。
**理由**: 直接擴充更自然，EF Core Identity 原生支援自訂 Role 類型，減少 JOIN 查詢。

### 3. Claims-based Permission + Policy-based Authorization

**選擇**: 定義權限常數（如 `members.view`），存為 RoleClaims，JWT 發行時將 permission claims 寫入 token，Controller 用 `[Authorize(Policy = "members.view")]`。
**替代方案**: 繼續使用 Role-based（`[Authorize(Roles = "...")]`）。
**理由**: Role-based 無法支援自訂角色的細粒度控制。Claims + Policy 是 ASP.NET Core 標準做法，且 SystemAdmin 可透過 policy assertion 永遠通過。

### 4. Permission Claims 寫入 JWT Token

**選擇**: 登入時查詢角色的所有 permission claims，寫入 JWT。
**替代方案**: 每次 API 請求從 DB 查詢權限。
**理由**: 目前權限數量少（<10 個），token 大小增加可忽略。避免每次請求都查 DB，效能較好。缺點是權限變更需重新登入，但這對管理系統是可接受的。

### 5. 前端閒置偵測使用 Composable

**選擇**: Vue composable（`useIdleTimeout`）監聽使用者活動事件，搭配 `setInterval` 檢查。
**替代方案**: Web Worker 或 Service Worker。
**理由**: 簡單有效，不需要額外複雜度。Composable 可在 Layout 元件中使用，生命週期自動管理。

### 6. 公開設定 API 不需認證

**選擇**: `GET /api/system-settings/public` 為匿名端點，回傳 websiteName、copyright、sessionTimeoutMinutes。
**理由**: 登入頁面也需要顯示網站名稱，此時使用者尚未登入。回傳的資訊非敏感。

## Risks / Trade-offs

- **[ApplicationRole Migration 風險]** → 修改 IdentityRole 型別需要 EF Migration，可能影響現有 AspNetRoles 資料。緩解：Migration 僅新增欄位（AddColumn），不重建表。
- **[JWT Token 大小增長]** → Permission claims 增加 token 大小。緩解：目前權限 <10 個，影響極小。未來權限增多時可改為 middleware 查 DB。
- **[權限變更延遲]** → 修改角色權限後已登入使用者需重新登入。緩解：前端可在偵測到權限不符時提示重新登入。
- **[閒置偵測多 Tab]** → 使用者可能開多個 Tab，一個 Tab 登出其他應同步。緩解：監聽 localStorage storage 事件。
