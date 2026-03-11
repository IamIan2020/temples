## MODIFIED Requirements

### Requirement: 路由與導航守衛

前端 SHALL 設定 Vue Router，路由分為兩大區塊：
- 前台路由（`/`）：登入、註冊、忘記密碼、重設密碼、個人資料（需登入）
- 後台路由（`/backstage`）：後台登入、會員列表、會員詳情、系統設定、角色列表、角色新增/編輯（需 WebAdmin+ 角色）

導航守衛 MUST 檢查認證狀態和角色權限：
- 前台受保護路由：未登入導向 `/login`
- 後台路由：未登入導向 `/backstage/login`，Member 角色導向 `/profile`
- 後台路由 MUST 檢查 route meta 中定義的 permission，無權限時導向 `/backstage/members` 或顯示提示

#### Scenario: 未登入存取前台受保護頁面

- **WHEN** 未登入的使用者存取 `/profile`
- **THEN** 系統導向 `/login`

#### Scenario: 未登入存取後台頁面

- **WHEN** 未登入的使用者存取 `/backstage/members`
- **THEN** 系統導向 `/backstage/login`

#### Scenario: Member 存取後台頁面

- **WHEN** Member 角色的使用者存取 `/backstage/members`
- **THEN** 系統導向 `/profile`

#### Scenario: 無權限存取後台功能

- **WHEN** 已登入的管理員存取自己沒有權限的後台頁面（如無 settings.view 權限存取 /backstage/settings）
- **THEN** 系統導向 `/backstage/members` 或顯示權限不足提示

## ADDED Requirements

### Requirement: 頁面標題動態更新

前端 SHALL 使用 Settings Store 的 websiteName 作為 document.title 的前綴。

#### Scenario: 頁面標題顯示網站名稱

- **WHEN** 使用者瀏覽任何頁面
- **THEN** 瀏覽器標題顯示 `{websiteName}` 或 `{頁面名稱} - {websiteName}`
