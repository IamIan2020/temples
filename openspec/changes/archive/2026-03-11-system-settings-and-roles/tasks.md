## 1. 系統設定 - 後端

- [x] 1.1 建立 SystemSetting 實體（CompanyName, WebsiteName, Phone, TaxId, Copyright, SessionTimeoutMinutes, UpdatedAt）
- [x] 1.2 建立 ISystemSettingRepository 介面與 SystemSettingRepository 實作
- [x] 1.3 建立 DTOs（SystemSettingResponse, UpdateSystemSettingRequest, PublicSettingResponse）
- [x] 1.4 建立 ISystemSettingService 介面與 SystemSettingService 實作
- [x] 1.5 建立 UpdateSystemSettingRequestValidator（CompanyName/WebsiteName 必填，SessionTimeoutMinutes 範圍 1-480）
- [x] 1.6 修改 AppDbContext 加入 DbSet<SystemSetting>，建立 EF Configuration
- [x] 1.7 修改 SeedData 種子預設系統設定（Id=1）
- [x] 1.8 建立 SystemSettingsController（GET /public 匿名、GET 和 PUT 需 SystemAdmin）
- [x] 1.9 註冊 DI + 建立 EF Migration + 驗證 dotnet build

## 2. 系統設定 - 前端

- [x] 2.1 定義 TypeScript 類型（PublicSettingResponse, SystemSettingResponse, UpdateSystemSettingRequest）
- [x] 2.2 建立 settings API service（getPublicSettings, getSettings, updateSettings）
- [x] 2.3 建立 Settings Pinia Store（loadPublicSettings, localStorage 快取）
- [x] 2.4 修改 main.ts 初始化載入公開設定
- [x] 2.5 修改 PublicLayout 使用 settingsStore.websiteName 取代硬編碼
- [x] 2.6 修改 AdminLayout 標題使用 settingsStore + 新增系統管理子選單
- [x] 2.7 建立 SystemSettingsView.vue（el-form 所有設定欄位 + 儲存）
- [x] 2.8 新增路由 /backstage/settings + 更新 document.title
- [x] 2.9 驗證 npm run build 成功

## 3. 自動登出

- [x] 3.1 建立 useIdleTimeout composable（監聽活動事件、計時器、警告觸發）
- [x] 3.2 建立 IdleWarningDialog 元件（el-dialog 倒數計時、繼續使用/立即登出按鈕）
- [x] 3.3 整合到 AdminLayout 和 PublicLayout（已登入時啟動閒置偵測）
- [x] 3.4 實作跨 Tab 登出同步（監聯 localStorage storage 事件）
- [x] 3.5 驗證 npm run build 成功

## 4. 權限管理 - 後端

- [x] 4.1 建立 Permissions 權限常數類別（members.view/edit/delete, settings.view/edit, roles.manage + 群組定義）
- [x] 4.2 建立 ApplicationRole 實體（繼承 IdentityRole，加 Description, IsBuiltIn）
- [x] 4.3 修改 AppDbContext 改用 ApplicationRole + 修改 Program.cs Identity 設定
- [x] 4.4 修改 SeedData 使用 ApplicationRole 建立內建角色 + 設定預設 RoleClaims
- [x] 4.5 建立 Role DTOs（RoleResponse, CreateRoleRequest, UpdateRoleRequest, PermissionGroupResponse）
- [x] 4.6 建立 IRoleService 介面與 RoleService 實作（CRUD + 權限管理）
- [x] 4.7 建立 Role Validators（CreateRoleRequestValidator, UpdateRoleRequestValidator）
- [x] 4.8 建立 RolesController（GET/POST/PUT/DELETE /api/roles, GET /api/roles/permissions）
- [x] 4.9 修改 AuthService：JWT Token 加入 permission claims + LoginResponse 加入 permissions
- [x] 4.10 設定 Policy-based 授權（Program.cs AddAuthorization + 更新 MembersController 和 SystemSettingsController）
- [x] 4.11 註冊 DI + 建立 EF Migration + 驗證 dotnet build

## 5. 權限管理 - 前端

- [x] 5.1 定義 TypeScript 類型（RoleResponse, CreateRoleRequest, UpdateRoleRequest, PermissionGroupResponse + UserInfo 加 permissions）
- [x] 5.2 建立 roles API service
- [x] 5.3 更新 Auth Store（hasPermission 方法，SystemAdmin 永遠 true）
- [x] 5.4 建立 RoleListView.vue（角色表格、新增/編輯/刪除按鈕，內建不可刪）
- [x] 5.5 建立 RoleDetailView.vue（新增/編輯共用，權限按群組 checkbox 選擇）
- [x] 5.6 新增路由（/backstage/roles, /backstage/roles/create, /backstage/roles/:id）
- [x] 5.7 更新 AdminLayout 選單權限控制（hasPermission 動態顯示）
- [x] 5.8 路由守衛加入權限檢查（route meta permission）
- [x] 5.9 驗證 npm run build 成功
