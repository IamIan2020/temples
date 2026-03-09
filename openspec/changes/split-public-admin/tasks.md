## 1. 頁面目錄重組

- [x] 1.1 建立前台頁面目錄 `views/public/`，將現有 LoginView、RegisterView、ForgotPasswordView、ResetPasswordView、ProfileView 搬移至 `views/public/`
- [x] 1.2 建立後台頁面目錄 `views/backstage/`，將現有 `views/admin/` 下的 MemberListView、MemberDetailView 搬移至 `views/backstage/`
- [x] 1.3 建立後台登入頁面 `views/backstage/LoginView.vue`（管理員專用登入，登入後驗證角色，非管理員顯示「您沒有後台管理權限」）

## 2. Layout 元件

- [x] 2.1 建立 `PublicLayout.vue`（前台 Layout：簡潔 Header，無側邊欄，未登入顯示登入/註冊連結，已登入顯示使用者名稱和登出按鈕）
- [x] 2.2 建立 `AdminLayout.vue`（後台 Layout：Header 標示「管理後台」+ 側邊導航欄，包含會員管理連結）
- [x] 2.3 移除或保留舊的 `Layout.vue`（確認無其他地方引用後移除）

## 3. 路由重構

- [x] 3.1 重構 `router/index.ts`：前台路由（`/login`、`/register`、`/forgot-password`、`/reset-password`、`/profile`）使用 PublicLayout
- [x] 3.2 重構 `router/index.ts`：後台路由（`/backstage/login`、`/backstage/members`、`/backstage/members/:id`）使用 AdminLayout
- [x] 3.3 更新導航守衛：前台受保護路由未登入導向 `/login`，後台路由未登入導向 `/backstage/login`，Member 存取後台導向 `/profile`

## 4. Auth Store 更新

- [x] 4.1 在 Pinia auth store 新增 `isAdmin` computed（判斷是否為 WebAdmin 或 SystemAdmin）
- [x] 4.2 更新登出邏輯：前台登出導向 `/login`，後台登出導向 `/backstage/login`

## 5. 前台頁面調整

- [x] 5.1 更新前台 ProfileView：Email 欄位設為唯讀不可編輯
- [x] 5.2 更新前台 LoginView：登入成功後導向 `/profile`，新增「註冊」和「忘記密碼」連結

## 6. 後台頁面調整

- [x] 6.1 更新後台 MemberListView、MemberDetailView 的路由連結（從 `/admin` 改為 `/backstage`）

## 7. 編譯驗證與收尾

- [x] 7.1 執行 `npm run build` 確認前端編譯成功無錯誤
- [x] 7.2 重新部署並驗證前後台分離功能正常（前台登入→Profile、後台登入→會員管理）
