## Why

宮廟網站目前只有後台管理功能，前台缺乏獨立首頁和實際內容頁面。需要新增「服務項目」功能讓管理員可以管理各種法會服務（點燈、祭改、功德主等），同時建立公開前台讓一般訪客瀏覽服務詳情。此外系統設定需擴充更多聯絡資訊欄位（地址、傳真、社群連結、Logo），並重新設計前台 Header/Footer 為宮廟風格。

## What Changes

- SystemSetting 擴充 6 個欄位：Address, Fax, LineUrl, FacebookUrl, GoogleMapUrl, LogoUrl
- 新增圖片上傳 API（`POST /api/upload/image`）
- 新增 ServiceItem / ServiceItemOption 實體（含 EF Configuration、Migration）
- 新增服務項目完整 CRUD API（8 個端點，含公開 API）
- 新增權限：serviceItems.view / serviceItems.edit / serviceItems.delete
- 新增 WangEditor 富文字編輯器元件（支援圖片上傳）
- 新增後台服務項目管理頁面（列表 + 編輯，含子選項管理）
- 重新設計前台 PublicLayout：拆分為 PublicHeader + PublicFooter
- 新增前台首頁（HomeView）、服務項目列表頁、服務項目詳情頁
- Header 設計為宮廟風格：左側 Logo、中間導覽、右側金色登入按鈕

## Capabilities

### New Capabilities
- `file-upload`: 圖片上傳服務（IFileUploadService、UploadController、前端 API）
- `service-items-backend`: 服務項目後端 CRUD（實體、DTOs、Repository、Service、Controller、Validators）
- `service-items-admin`: 服務項目後台管理前端（列表頁、編輯頁、WangEditor 富文字編輯器）
- `public-home`: 前台首頁（宮廟簡介 + 服務項目卡片 Grid）
- `public-services`: 前台服務項目瀏覽（列表頁 + 詳情頁，含方案價格展示）
- `public-layout`: 前台 Header/Footer 宮廟風格設計

### Modified Capabilities
- `system-settings`: 擴充 6 個欄位（Address, Fax, LineUrl, FacebookUrl, GoogleMapUrl, LogoUrl）+ 前後端對應更新
- `frontend-app`: 路由新增首頁、服務項目相關頁面、後台服務項目管理
- `admin-backend`: AdminLayout 側邊欄新增「服務項目」選單

## Impact

- **後端**: SystemSetting 擴充 + Migration、新增 ServiceItem/ServiceItemOption 實體 + Migration、新增 UploadController/ServiceItemsController、Permissions 新增 3 個權限、SeedData 更新 WebAdmin 預設權限
- **前端**: 新增 WangEditor 元件、5 個新頁面（HomeView、ServiceItemsView、ServiceItemDetailView、ServiceItemListView、ServiceItemDetailView）、PublicHeader/PublicFooter 元件、public.css 樣式、serviceItems store/API
- **資料庫**: SystemSettings 新增 6 欄位、新增 ServiceItems/ServiceItemOptions 表
- **API**: 新增 `/api/upload/image`、`/api/service-items`（8 個端點）
- **套件**: 新增 @wangeditor/editor、@wangeditor/editor-for-vue
