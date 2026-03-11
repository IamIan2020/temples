<template>
  <div>
    <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 16px">
      <h2 style="margin: 0">服務項目管理</h2>
      <el-button type="primary" @click="router.push('/backstage/service-items/create')">新增分類</el-button>
    </div>

    <el-card v-loading="loading">
      <el-table :data="items" style="width: 100%">
        <el-table-column label="圖片" width="100">
          <template #default="{ row }">
            <img v-if="row.headerImage" :src="row.headerImage" alt="" style="width: 60px; height: 40px; object-fit: cover; border-radius: 4px" />
            <span v-else style="color: #909399">無圖片</span>
          </template>
        </el-table-column>
        <el-table-column prop="title" label="分類名稱" />
        <el-table-column label="商品數" width="80" align="center">
          <template #default="{ row }">{{ row.options.length }}</template>
        </el-table-column>
        <el-table-column prop="sortOrder" label="排序" width="80" align="center" />
        <el-table-column label="狀態" width="80" align="center">
          <template #default="{ row }">
            <el-tag :type="row.isActive ? 'success' : 'info'" size="small">
              {{ row.isActive ? '啟用' : '停用' }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column label="操作" width="160" align="center">
          <template #default="{ row }">
            <el-button size="small" @click="router.push(`/backstage/service-items/${row.id}`)">編輯</el-button>
            <el-button size="small" type="danger" @click="handleDelete(row.id, row.title)">刪除</el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { ElMessage, ElMessageBox } from 'element-plus'
import { serviceItemsApi } from '../../api/serviceItems'
import type { ServiceItemResponse } from '../../types/api'

const router = useRouter()
const loading = ref(false)
const items = ref<ServiceItemResponse[]>([])

const loadItems = async () => {
  loading.value = true
  try {
    const res = await serviceItemsApi.getAll()
    items.value = res.data.data ?? []
  } catch {
    ElMessage.error('載入失敗')
  } finally {
    loading.value = false
  }
}

const handleDelete = async (id: number, title: string) => {
  try {
    await ElMessageBox.confirm(`確定要刪除「${title}」分類及其所有商品嗎？`, '確認刪除', {
      type: 'warning',
      confirmButtonText: '刪除',
      cancelButtonText: '取消',
    })
    await serviceItemsApi.delete(id)
    ElMessage.success('已刪除')
    await loadItems()
  } catch {
    // 取消刪除
  }
}

onMounted(loadItems)
</script>
