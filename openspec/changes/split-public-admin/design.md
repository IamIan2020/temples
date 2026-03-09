## Context

目前前端使用單一 Layout，所有角色共用相同入口（`/login`）。一般會員登入後看到的導航選單包含後台管理項目（雖然無權限存取），後台管理入口也暴露在顯眼路徑。

現有前端結構：
- `src/views/` — 所有頁面平鋪在同一層
- `src/components/Layout.vue` — 單一 Layout，根據角色動態顯示選單
- `src/router/index.ts` — 單一路由表

## Goals / Non-Goals

**Goals:**
- 前台（公開區域）與後台（管理區域）使用不同 Layout 和路由
- 前台入口：`/login`、`/register`、`/profile` 等
- 後台入口：使用隱晦路徑 `/backstage`（避免 `/admin` 被猜到）
- 前台登入後僅顯示個人資料頁面（可檢視/編輯，Email 不可變更）
- 後台登入僅限 WebAdmin / SystemAdmin
- 路由守衛強化：Member 進後台會被導回前台

**Non-Goals:**
- 不修改後端 API（現有 API 和權限控制已滿足）
- 不新增前台額外功能（如公告、活動等，留給後續 change）
- 不改變認證機制（JWT + Refresh Token 維持不變）

## Decisions

### 1. 後台路徑使用 `/backstage` 而非 `/admin`

**選擇**：`/backstage`
**替代方案**：`/admin`、`/manage`、`/console`
**理由**：`/admin` 是最常被掃描的路徑，`/backstage` 較不常見且語意明確。未來可考慮加入 IP 白名單或額外驗證。

### 2. 前後台使用獨立 Layout 元件

**選擇**：建立 `PublicLayout.vue` 和 `AdminLayout.vue` 兩個獨立元件
**替代方案**：在現有 Layout 中用 v-if 切換
**理由**：獨立 Layout 更容易維護，前後台的視覺風格可以完全不同。前台 Layout 簡潔（Header + 內容），後台 Layout 保留側邊導航。

### 3. 前後台共用同一個 Auth Store

**選擇**：共用 Pinia auth store，不分離
**理由**：JWT Token 機制相同，只是路由守衛的角色檢查邏輯不同。Store 新增 helper（如 `isAdmin` computed）來簡化判斷。

### 4. 前台登入成功後導向 `/profile`

**選擇**：前台會員登入後固定導向 `/profile`
**替代方案**：導向首頁 `/`
**理由**：目前前台沒有首頁內容，直接導到個人資料頁最實用。

### 5. 頁面目錄結構重新組織

```
src/views/
├── public/            # 前台頁面
│   ├── LoginView.vue
│   ├── RegisterView.vue
│   ├── ForgotPasswordView.vue
│   ├── ResetPasswordView.vue
│   └── ProfileView.vue
├── backstage/         # 後台頁面
│   ├── LoginView.vue
│   ├── MemberListView.vue
│   └── MemberDetailView.vue
└── (移除舊的 admin/ 目錄)
```

## Risks / Trade-offs

- **[風險] 後台路徑被發現** → 路徑隱晦只是第一層防護，真正的安全依賴 JWT + 角色驗證。即使路徑被發現，無合法 Token 也無法存取。
- **[風險] 前後台 Layout 重複代碼** → 可接受的 trade-off，兩個 Layout 的邏輯其實差異很大（前台無側邊欄、後台有），不值得抽象共用。
- **[Trade-off] 現有書籤/連結失效** → `/admin/members` 改為 `/backstage/members`，舊連結會 404。可接受，系統尚未正式上線。
