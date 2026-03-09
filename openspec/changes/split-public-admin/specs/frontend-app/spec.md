## MODIFIED Requirements

### Requirement: 路由與導航守衛

前端 SHALL 設定 Vue Router，路由分為兩大區塊：
- 前台路由（`/`）：登入、註冊、忘記密碼、重設密碼、個人資料（需登入）
- 後台路由（`/backstage`）：後台登入、會員列表、會員詳情（需 WebAdmin+ 角色）

導航守衛 MUST 檢查認證狀態和角色權限：
- 前台受保護路由：未登入導向 `/login`
- 後台路由：未登入導向 `/backstage/login`，Member 角色導向 `/profile`

#### Scenario: 未登入存取前台受保護頁面

- **WHEN** 未登入的使用者存取 `/profile`
- **THEN** 系統導向 `/login`

#### Scenario: 未登入存取後台頁面

- **WHEN** 未登入的使用者存取 `/backstage/members`
- **THEN** 系統導向 `/backstage/login`

#### Scenario: Member 存取後台頁面

- **WHEN** Member 角色的使用者存取 `/backstage/members`
- **THEN** 系統導向 `/profile`

### Requirement: Layout 元件

前端 SHALL 提供兩個獨立的 Layout 元件：
- `PublicLayout.vue`：前台使用，簡潔 Header（無側邊欄）
- `AdminLayout.vue`：後台使用，Header + 側邊導航欄

各 Layout 根據其使用場景獨立設計，不共用同一元件。

#### Scenario: 前台頁面使用 PublicLayout

- **WHEN** 使用者瀏覽前台路由（`/login`、`/profile` 等）
- **THEN** 頁面使用 PublicLayout 渲染

#### Scenario: 後台頁面使用 AdminLayout

- **WHEN** 管理員瀏覽後台路由（`/backstage/*`）
- **THEN** 頁面使用 AdminLayout 渲染
