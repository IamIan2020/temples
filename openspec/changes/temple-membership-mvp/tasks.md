## 1. 專案建立與基礎架構

- [x] 1.1 初始化 git repo + .gitignore
- [x] 1.2 建立 Solution 與三個後端專案（Api / Core / Infrastructure）+ 測試專案，設定專案參考關係
- [x] 1.3 安裝後端 NuGet 套件（Npgsql、Identity、JWT、FluentValidation、MailKit、Swagger）
- [x] 1.4 建立 ApplicationUser 實體（繼承 IdentityUser，擴充所有自訂欄位）
- [x] 1.5 建立 AppDbContext（繼承 IdentityDbContext）與 ApplicationUser Configuration
- [x] 1.6 設定 Program.cs（Identity、JWT、Swagger、CORS、PostgreSQL 連線、自動 Migration）
- [x] 1.7 建立統一回應格式 ApiResponse<T> 與 ExceptionHandlingMiddleware
- [x] 1.8 建立種子資料（三個角色 + ianadmin 預設帳號）
- [ ] 1.9 建立首次 EF Core Migration 並驗證資料庫建立成功

## 2. 後端認證 API

- [x] 2.1 建立 Auth DTOs（RegisterRequest、LoginRequest、LoginResponse、ForgotPasswordRequest、ResetPasswordRequest、RefreshTokenRequest）
- [x] 2.2 建立 Auth FluentValidation Validators
- [x] 2.3 實作 IEmailService + EmailService（MailKit + Gmail SMTP）
- [x] 2.4 實作 IAuthService + AuthService（註冊、登入、JWT 產生、Refresh Token、忘記密碼、重設密碼）
- [x] 2.5 實作 AuthController（所有認證端點）
- [x] 2.6 註冊所有認證相關服務至 DI Container
- [x] 2.7 驗證認證 API（編譯成功 + Swagger 測試）

## 3. 後端會員管理 API

- [x] 3.1 建立 Member DTOs（MemberProfileResponse、UpdateProfileRequest、ChangePasswordRequest、MemberListRequest、MemberDetailResponse、ChangeRoleRequest、ChangeStatusRequest）
- [x] 3.2 建立 Member FluentValidation Validators
- [x] 3.3 實作 IMemberService + MemberService（Profile CRUD、會員列表分頁搜尋、角色變更、狀態變更）
- [x] 3.4 實作 MembersController（所有會員端點 + Authorization Policy）
- [x] 3.5 註冊會員相關服務至 DI Container
- [x] 3.6 驗證會員 API（編譯成功 + Swagger 測試）

## 4. 後端測試

- [ ] 4.1 撰寫 AuthService 單元測試（註冊、登入、Token 刷新、忘記密碼、重設密碼）
- [ ] 4.2 撰寫 MemberService 單元測試（Profile CRUD、列表搜尋、角色變更、狀態變更）
- [ ] 4.3 執行所有測試並確認通過

## 5. 前端專案建立

- [x] 5.1 用 Vite 初始化 Vue 3 + TypeScript 專案
- [x] 5.2 安裝前端套件（Element Plus、Axios、Pinia、Vue Router、icons）
- [x] 5.3 設定 Vite proxy 代理後端 API
- [x] 5.4 建立 Axios client + JWT interceptor（自動附加 Token、401 自動刷新）
- [x] 5.5 建立 TypeScript 類型定義（對應後端所有 DTO）
- [x] 5.6 建立 API 呼叫層（auth.ts、members.ts）
- [x] 5.7 建立 Pinia auth store（登入、登出、註冊、Token 管理）
- [x] 5.8 設定 Vue Router + 導航守衛（認證檢查、角色權限檢查）

## 6. 前端頁面

- [x] 6.1 建立 Layout 元件（Header + 側邊導航，根據角色動態顯示選單）
- [x] 6.2 建立登入頁面（Email + 密碼表單、驗證、錯誤處理）
- [x] 6.3 建立註冊頁面（Email + 密碼 + 確認密碼 + 顯示名稱表單）
- [x] 6.4 建立忘記密碼頁面（Email 輸入表單）
- [x] 6.5 建立重設密碼頁面（從 URL 取得 Token，新密碼表單）
- [x] 6.6 建立個人資料頁面（顯示/編輯 Profile + 變更密碼）
- [x] 6.7 建立後台會員列表頁面（分頁表格 + 搜尋）
- [x] 6.8 建立後台會員詳情頁面（查看/編輯 + 停用/啟用 + 角色變更）
- [x] 6.9 前端編譯驗證（npm run build 成功且無錯誤）

## 7. 整合與收尾

- [ ] 7.1 前後端聯調測試（完整流程：註冊→登入→Profile→後台管理）
- [ ] 7.2 建立 README.md（啟動步驟、環境需求、設定說明）
