## Why

目前系統缺少三個關鍵功能：(1) 無法透過後台管理網站基本資訊（名稱、公司資訊等），前端硬編碼「宮廟系統」；(2) 登入後無閒置登出機制，使用者離開後帳號長期保持登入狀態，存在安全風險；(3) 角色權限為硬編碼（SystemAdmin/WebAdmin/Member），無法新增自訂角色或細粒度設定權限。

## What Changes

- 新增「系統設定」功能：後台可管理公司名稱、網站名稱、電話、統編、Copyright、閒置登出時間
- 前台 / 後台 Layout 動態套用系統設定（網站名稱、Copyright）
- 新增前端閒置偵測與自動登出機制，含警告對話框與倒數計時
- 擴充 ASP.NET Core Identity 角色為 ApplicationRole（加入 Description、IsBuiltIn）
- 新增權限常數系統（Claims-based permissions）
- 新增角色 CRUD API（可新增自訂角色、設定權限，內建角色不可刪除）
- JWT Token 加入 permission claims
- **BREAKING**: 現有 Controller 從 `[Authorize(Roles = "...")]` 改為 `[Authorize(Policy = "...")]`（Policy-based 授權）
- LoginResponse / UserInfo 加入 permissions 欄位

## Capabilities

### New Capabilities
- `system-settings`: 系統設定 CRUD（後端實體、API、前端管理頁面、前台套用）
- `idle-timeout`: 前端閒置偵測與自動登出機制（composable、警告對話框、跨 Tab 同步）
- `role-management`: 角色與權限管理（ApplicationRole、權限常數、角色 CRUD API、前端管理頁面）

### Modified Capabilities
- `frontend-app`: 路由新增系統設定與角色管理頁面、AdminLayout 側邊欄新增選單、權限守衛
- `admin-backend`: AdminLayout 加入系統設定/權限群組選單項目、權限控制顯示
- `public-frontend`: PublicLayout 動態顯示網站名稱、閒置登出整合
- `user-auth`: JWT 加入 permission claims、LoginResponse 加入 permissions、Policy-based 授權

## Impact

- **後端**: 新增 SystemSetting 實體 + Migration、ApplicationRole 取代 IdentityRole（Migration）、新增 SystemSettingsController 和 RolesController、修改 AuthService（JWT claims）、修改 MembersController（Policy-based）、修改 Program.cs（授權設定）
- **前端**: 新增 settings store、roles API、3 個後台頁面（SystemSettingsView、RoleListView、RoleDetailView）、useIdleTimeout composable、IdleWarningDialog 元件、修改兩個 Layout
- **資料庫**: 新增 SystemSettings 表、修改 AspNetRoles 表（加 Description、IsBuiltIn 欄位）、新增 RoleClaims 資料
- **API**: 新增 `/api/system-settings`、`/api/roles` 端點
