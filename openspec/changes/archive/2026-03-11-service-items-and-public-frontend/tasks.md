## 1. SystemSetting 擴充 + 圖片上傳 API

- [x] 1.1 SystemSetting 實體新增 Address, Fax, LineUrl, FacebookUrl, GoogleMapUrl, LogoUrl 欄位
- [x] 1.2 SystemSettingConfiguration 新增 MaxLength 設定
- [x] 1.3 SystemSettingResponse / UpdateSystemSettingRequest / PublicSettingResponse DTOs 新增對應欄位
- [x] 1.4 SystemSettingService 更新 3 個方法的屬性映射
- [x] 1.5 UpdateSystemSettingRequestValidator 新增 URL/MaxLength 驗證
- [x] 1.6 建立 EF Migration: AddSystemSettingFields
- [x] 1.7 建立 IFileUploadService 介面
- [x] 1.8 建立 FileUploadService（驗證類型 jpg/png/gif/webp、5MB 限制、存至 wwwroot/uploads/）
- [x] 1.9 建立 UploadController（POST /api/upload/image [Authorize]）
- [x] 1.10 DI 註冊 FileUploadService、Program.cs 啟動時建立 uploads 目錄
- [x] 1.11 前端 upload API 封裝（uploadImage）
- [x] 1.12 前端 TypeScript 介面更新（Settings 3 個介面擴充）
- [x] 1.13 Settings Pinia Store 擴充新欄位
- [x] 1.14 SystemSettingsView.vue 新增所有欄位 + Logo 上傳
- [x] 1.15 驗證 dotnet build + npm run build 成功

## 2. 服務項目後端

- [x] 2.1 建立 ServiceItem 實體
- [x] 2.2 建立 ServiceItemOption 實體
- [x] 2.3 建立 ServiceItemConfiguration + ServiceItemOptionConfiguration（decimal(18,2)）
- [x] 2.4 AppDbContext 新增 2 個 DbSet
- [x] 2.5 建立 EF Migration: AddServiceItems
- [x] 2.6 建立 DTOs（ServiceItemResponse, ServiceItemOptionResponse, Create/Update Request, PublicResponse 等 10 個檔案）
- [x] 2.7 建立 IServiceItemRepository 介面
- [x] 2.8 建立 ServiceItemRepository（Include Options、ActiveList、ActiveById）
- [x] 2.9 建立 IServiceItemService 介面
- [x] 2.10 建立 ServiceItemService（CRUD + Options 差異更新 + 排序 + 公開 API）
- [x] 2.11 建立 CreateServiceItemRequestValidator + UpdateServiceItemRequestValidator
- [x] 2.12 建立 ServiceItemsController（8 個端點）
- [x] 2.13 Permissions 新增 serviceItems.view/edit/delete + Groups + DisplayNames
- [x] 2.14 DI 註冊 IServiceItemRepository, IServiceItemService
- [x] 2.15 SeedData 更新 WebAdmin 預設權限
- [x] 2.16 驗證 dotnet build 成功

## 3. 服務項目後台管理前端

- [x] 3.1 安裝 @wangeditor/editor + @wangeditor/editor-for-vue
- [x] 3.2 建立 WangEditor.vue 封裝元件（v-model、圖片上傳配置）
- [x] 3.3 建立 shims-wangeditor.d.ts 類型宣告
- [x] 3.4 前端 TypeScript 介面新增 ServiceItem/Option 相關（12 個介面）
- [x] 3.5 建立 serviceItems API 封裝（8 個方法）
- [x] 3.6 建立 ServiceItemListView.vue（el-table 列表、縮圖、Options 數量、排序、狀態、刪除）
- [x] 3.7 建立 ServiceItemDetailView.vue（建立/編輯共用、圖片上傳、WangEditor、子選項動態管理）
- [x] 3.8 路由新增 service-items / service-items/create / service-items/:id
- [x] 3.9 AdminLayout 側邊選單新增「服務項目」（需 serviceItems.view 權限）
- [x] 3.10 驗證 npm run build 成功

## 4. 公開前端

- [x] 4.1 建立 public.css 宮廟風格樣式（Header/Footer/Card/Detail）
- [x] 4.2 建立 PublicHeader.vue（Logo 回首頁、服務項目下拉導覽、金色登入按鈕）
- [x] 4.3 建立 PublicFooter.vue（深藍底金色文字、聯絡資訊、社群連結、Copyright）
- [x] 4.4 重構 PublicLayout.vue 使用 Header + Footer 元件
- [x] 4.5 建立 serviceItems Pinia Store（公開列表快取）
- [x] 4.6 建立 HomeView.vue（首頁：Logo + 宮廟名稱 + 服務項目卡片 Grid）
- [x] 4.7 建立 ServiceItemsView.vue（服務項目完整列表）
- [x] 4.8 建立 ServiceItemDetailView.vue（詳情頁：大圖 + 富文字 + Options 價格列表 + 線上登記按鈕僅展示）
- [x] 4.9 路由新增 /（首頁）、/services、/services/:id
- [x] 4.10 main.ts 引入 public.css
- [x] 4.11 Header 重新設計為宮廟風格（參考截圖：深藍格紋背景、金色文字、圓角登入按鈕）
- [x] 4.12 驗證 npm run build 成功

## 進度追蹤

| 分項 | 完成度 | 狀態 |
|------|--------|------|
| SystemSetting 擴充 + 圖片上傳 | 15/15 | ✅ 完成 |
| 服務項目後端 | 16/16 | ✅ 完成 |
| 服務項目後台管理前端 | 10/10 | ✅ 完成 |
| 公開前端 | 12/12 | ✅ 完成 |
| **合計** | **53/53** | **✅ 全部完成** |
