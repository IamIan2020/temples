## 架構設計

### 後端架構

沿用現有的 Clean Architecture 分層：
- **Temples.Core**: 實體、DTOs、介面、Service、Validators
- **Temples.Infrastructure**: EF Configuration、Repository、DI 註冊
- **Temples.Api**: Controller

### 服務項目資料模型

```
ServiceItem (1) ──── (N) ServiceItemOption
├── Id                    ├── Id
├── HeaderImage?          ├── ServiceItemId (FK)
├── Title                 ├── Title
├── SortOrder             ├── Price (decimal)
├── IsActive              ├── PriceUnit?
├── HtmlContent?          ├── SubTitle?
├── CreatedAt             ├── Description?
├── UpdatedAt             ├── MaxDonorCount?
└── Options[]             ├── MaxTotalAmount?
                          ├── CurrentDonorCount
                          ├── CurrentTotalAmount
                          ├── SortOrder
                          └── IsActive
```

### Option 更新策略

採用「全量替換」策略：
- 前端送完整 Options 陣列
- `Id == null` → 新增
- `Id` 存在於現有資料 → 更新
- 現有 Option 不在請求列表中 → 刪除

### 圖片上傳

- 獨立 `POST /api/upload/image` 端點
- 驗證：jpg/png/gif/webp，最大 5MB
- 存放：`wwwroot/uploads/images/{guid}.{ext}`
- WangEditor 內圖片也使用同一端點

### API 端點設計

| 端點 | 方法 | 權限 |
|------|------|------|
| `GET /api/service-items` | 後台列表 | serviceItems.view |
| `GET /api/service-items/{id}` | 後台詳情 | serviceItems.view |
| `POST /api/service-items` | 建立 | serviceItems.edit |
| `PUT /api/service-items/{id}` | 更新 | serviceItems.edit |
| `DELETE /api/service-items/{id}` | 刪除 | serviceItems.delete |
| `PUT /api/service-items/sort` | 排序 | serviceItems.edit |
| `GET /api/service-items/public` | 公開列表 | AllowAnonymous |
| `GET /api/service-items/public/{id}` | 公開詳情 | AllowAnonymous |

### 前台設計

- **Header**: 左側 Logo（回首頁）→ 中間導覽（服務項目下拉）→ 右側金色登入按鈕
- **Footer**: 深藍底、金色文字、聯絡資訊、社群圖示連結
- **首頁**: 宮廟 Logo + 名稱 + 服務項目卡片 Grid
- **服務詳情**: 大圖 + 標題 + 富文字內容 + 方案列表（價格、說明）
- **購物車**: 本期不做，「線上登記」按鈕僅展示

### 決策記錄

1. 富文字編輯器選用 WangEditor v5（中文友善、Vue 3 支援）
2. 公開 API 使用同一 Controller 的 `/public` 路徑前綴
3. 購物車/線上登記功能延後實作
