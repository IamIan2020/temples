<template>
  <div>
    <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 16px">
      <h2 style="margin: 0">商品管理</h2>
      <el-button type="primary" @click="openDialog(null)">新增商品</el-button>
    </div>

    <el-card v-loading="loading">
      <!-- 分類篩選 -->
      <div style="margin-bottom: 16px">
        <el-select v-model="filterCategoryId" placeholder="全部分類" clearable style="width: 200px" @change="applyFilter">
          <el-option v-for="cat in categories" :key="cat.id" :label="cat.title" :value="cat.id" />
        </el-select>
      </div>

      <el-table :data="filteredProducts" style="width: 100%">
        <el-table-column prop="categoryTitle" label="分類" width="150" />
        <el-table-column prop="title" label="商品名稱" />
        <el-table-column label="價格" width="150">
          <template #default="{ row }">
            {{ formatPrice(row.price) }}<span v-if="row.priceUnit" style="color: #909399"> / {{ row.priceUnit }}</span>
          </template>
        </el-table-column>
        <el-table-column prop="subTitle" label="副標題" />
        <el-table-column label="名額" width="100" align="center">
          <template #default="{ row }">
            <span v-if="row.maxDonorCount">{{ row.currentDonorCount }} / {{ row.maxDonorCount }}</span>
            <span v-else style="color: #909399">無限</span>
          </template>
        </el-table-column>
        <el-table-column label="上架時間" width="180">
          <template #default="{ row }">
            <template v-if="row.startDate || row.endDate">
              <div v-if="row.startDate" style="font-size: 12px">{{ row.startDate.substring(0, 10) }} 起</div>
              <div v-if="row.endDate" style="font-size: 12px">{{ row.endDate.substring(0, 10) }} 止</div>
            </template>
            <span v-else style="color: #909399">不限</span>
          </template>
        </el-table-column>
        <el-table-column prop="sortOrder" label="排序" width="70" align="center" />
        <el-table-column label="狀態" width="70" align="center">
          <template #default="{ row }">
            <el-tag :type="row.isActive ? 'success' : 'info'" size="small">
              {{ row.isActive ? '啟用' : '停用' }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column label="操作" width="140" align="center">
          <template #default="{ row }">
            <el-button size="small" @click="openDialog(row)">編輯</el-button>
            <el-button size="small" type="danger" @click="handleDelete(row)">刪除</el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <!-- 新增/編輯商品 Dialog -->
    <el-dialog v-model="showDialog" :title="editingProduct ? '編輯商品' : '新增商品'" width="600px">
      <el-form :model="productForm" label-width="100px">
        <el-form-item label="所屬分類" required>
          <el-select v-model="productForm.serviceItemId" placeholder="選擇分類" style="width: 100%">
            <el-option v-for="cat in categories" :key="cat.id" :label="cat.title" :value="cat.id" />
          </el-select>
        </el-form-item>
        <el-form-item label="商品名稱" required>
          <el-input v-model="productForm.title" placeholder="如：光明燈、太歲燈" />
        </el-form-item>
        <el-row :gutter="12">
          <el-col :span="12">
            <el-form-item label="價格" required>
              <el-input-number v-model="productForm.price" :min="0" :precision="0" style="width: 100%" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="單位">
              <el-input v-model="productForm.priceUnit" placeholder="如：盞、位" />
            </el-form-item>
          </el-col>
        </el-row>
        <el-form-item label="副標題">
          <el-input v-model="productForm.subTitle" placeholder="副標題" />
        </el-form-item>
        <el-form-item label="商品說明">
          <el-input v-model="productForm.description" type="textarea" :rows="3" placeholder="商品說明" />
        </el-form-item>
        <el-row :gutter="12">
          <el-col :span="12">
            <el-form-item label="名額上限">
              <el-input-number v-model="productForm.maxDonorCount" :min="0" style="width: 100%" placeholder="0 = 無限" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="金額上限">
              <el-input-number v-model="productForm.maxTotalAmount" :min="0" :precision="0" style="width: 100%" />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row :gutter="12">
          <el-col :span="12">
            <el-form-item label="上架日期">
              <el-date-picker v-model="productForm.startDate" type="datetime" placeholder="不限" style="width: 100%" format="YYYY-MM-DD HH:mm" value-format="YYYY-MM-DDTHH:mm:00" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="下架日期">
              <el-date-picker v-model="productForm.endDate" type="datetime" placeholder="不限" style="width: 100%" format="YYYY-MM-DD HH:mm" value-format="YYYY-MM-DDTHH:mm:00" />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row :gutter="12">
          <el-col :span="12">
            <el-form-item label="排序">
              <el-input-number v-model="productForm.sortOrder" :min="0" style="width: 100%" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="啟用">
              <el-switch v-model="productForm.isActive" />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
      <template #footer>
        <el-button @click="showDialog = false">取消</el-button>
        <el-button type="primary" :loading="saving" @click="handleSave">儲存商品</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import { serviceItemsApi, productsApi } from '../../api/serviceItems'
import type { ProductResponse, ServiceItemResponse } from '../../types/api'

const loading = ref(false)
const saving = ref(false)
const products = ref<ProductResponse[]>([])
const categories = ref<ServiceItemResponse[]>([])
const filterCategoryId = ref<number | null>(null)
const showDialog = ref(false)
const editingProduct = ref<ProductResponse | null>(null)

const productForm = ref({
  serviceItemId: 0,
  title: '',
  price: 0,
  priceUnit: null as string | null,
  subTitle: null as string | null,
  description: null as string | null,
  maxDonorCount: null as number | null,
  maxTotalAmount: null as number | null,
  sortOrder: 0,
  isActive: true,
  startDate: null as string | null,
  endDate: null as string | null,
})

const filteredProducts = computed(() => {
  if (!filterCategoryId.value) return products.value
  return products.value.filter(p => p.serviceItemId === filterCategoryId.value)
})

const formatPrice = (price: number) => {
  return new Intl.NumberFormat('zh-TW', { style: 'currency', currency: 'TWD', minimumFractionDigits: 0 }).format(price)
}

const openDialog = (product: ProductResponse | null) => {
  editingProduct.value = product
  if (product) {
    productForm.value = {
      serviceItemId: product.serviceItemId,
      title: product.title,
      price: product.price,
      priceUnit: product.priceUnit,
      subTitle: product.subTitle,
      description: product.description,
      maxDonorCount: product.maxDonorCount,
      maxTotalAmount: product.maxTotalAmount,
      sortOrder: product.sortOrder,
      isActive: product.isActive,
      startDate: product.startDate,
      endDate: product.endDate,
    }
  } else {
    productForm.value = {
      serviceItemId: filterCategoryId.value ?? (categories.value[0]?.id ?? 0),
      title: '',
      price: 0,
      priceUnit: null,
      subTitle: null,
      description: null,
      maxDonorCount: null,
      maxTotalAmount: null,
      sortOrder: products.value.length,
      isActive: true,
      startDate: null,
      endDate: null,
    }
  }
  showDialog.value = true
}

const handleSave = async () => {
  if (!productForm.value.serviceItemId) {
    ElMessage.warning('請選擇分類')
    return
  }
  if (!productForm.value.title.trim()) {
    ElMessage.warning('請輸入商品名稱')
    return
  }
  saving.value = true
  try {
    if (editingProduct.value) {
      await productsApi.update(editingProduct.value.id, productForm.value)
      ElMessage.success('商品已更新')
    } else {
      await productsApi.create(productForm.value)
      ElMessage.success('商品已新增')
    }
    showDialog.value = false
    await loadProducts()
  } catch {
    ElMessage.error('儲存商品失敗')
  } finally {
    saving.value = false
  }
}

const handleDelete = async (product: ProductResponse) => {
  try {
    await ElMessageBox.confirm(`確定要刪除「${product.title}」嗎？`, '確認刪除', {
      type: 'warning',
      confirmButtonText: '刪除',
      cancelButtonText: '取消',
    })
    await productsApi.delete(product.id)
    ElMessage.success('商品已刪除')
    await loadProducts()
  } catch {
    // 取消
  }
}

const applyFilter = () => {
  // computed 會自動更新
}

const loadProducts = async () => {
  loading.value = true
  try {
    const res = await productsApi.getAll()
    products.value = res.data.data ?? []
  } catch {
    ElMessage.error('載入商品失敗')
  } finally {
    loading.value = false
  }
}

const loadCategories = async () => {
  try {
    const res = await serviceItemsApi.getAll()
    categories.value = res.data.data ?? []
  } catch {
    ElMessage.error('載入分類失敗')
  }
}

onMounted(async () => {
  await Promise.all([loadProducts(), loadCategories()])
})
</script>
