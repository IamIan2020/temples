<template>
  <div>
    <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 16px">
      <h2 style="margin: 0">分類管理</h2>
      <el-button type="primary" @click="openDialog(null)">新增分類</el-button>
    </div>

    <el-card v-loading="loading">
      <p style="color: #909399; font-size: 13px; margin: 0 0 12px">拖曳列來調整排序</p>
      <el-table ref="tableRef" :data="items" style="width: 100%" row-key="id">
        <el-table-column width="40" align="center">
          <template #default>
            <el-icon class="drag-handle" style="cursor: grab; color: #909399"><Rank /></el-icon>
          </template>
        </el-table-column>
        <el-table-column prop="title" label="分類名稱" />
        <el-table-column label="商品數" width="80" align="center">
          <template #default="{ row }">{{ row.options.length }}</template>
        </el-table-column>
        <el-table-column label="狀態" width="80" align="center">
          <template #default="{ row }">
            <el-tag :type="row.isActive ? 'success' : 'info'" size="small">
              {{ row.isActive ? '啟用' : '停用' }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column label="操作" width="200" align="center">
          <template #default="{ row }">
            <el-button size="small" type="primary" @click="openDialog(row)">編輯</el-button>
            <el-button size="small" type="danger" @click="handleDelete(row.id, row.title)">刪除</el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <!-- 新增/編輯分類 Dialog -->
    <el-dialog v-model="showDialog" :title="editingItem ? '編輯分類' : '新增分類'" width="calc(100% - 40px)" top="20px" class="fullscreen-dialog">
      <el-form :model="form" label-width="80px">
        <el-form-item label="分類名稱" required>
          <el-input v-model="form.title" placeholder="如：點燈服務、祭改服務" autofocus />
        </el-form-item>
        <el-form-item v-if="editingItem" label="啟用">
          <el-switch v-model="form.isActive" />
        </el-form-item>
        <el-form-item label="分類說明">
          <WangEditor v-model="form.htmlContent" style="width: 100%" />
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="showDialog = false">取消</el-button>
        <el-button type="primary" :loading="saving" @click="handleSave">{{ editingItem ? '儲存' : '建立' }}</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, nextTick } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import { Rank } from '@element-plus/icons-vue'
import Sortable from 'sortablejs'
import { serviceItemsApi } from '../../api/serviceItems'
import type { ServiceItemResponse } from '../../types/api'
import WangEditor from '../../components/WangEditor.vue'

const loading = ref(false)
const saving = ref(false)
const items = ref<ServiceItemResponse[]>([])
const showDialog = ref(false)
const editingItem = ref<ServiceItemResponse | null>(null)
const tableRef = ref()

const form = ref({
  title: '',
  isActive: true,
  htmlContent: null as string | null,
})

const openDialog = (item: ServiceItemResponse | null) => {
  editingItem.value = item
  if (item) {
    form.value = { title: item.title, isActive: item.isActive, htmlContent: item.htmlContent }
  } else {
    form.value = { title: '', isActive: true, htmlContent: null }
  }
  showDialog.value = true
}

const handleSave = async () => {
  if (!form.value.title.trim()) {
    ElMessage.warning('請輸入分類名稱')
    return
  }
  saving.value = true
  try {
    if (editingItem.value) {
      await serviceItemsApi.update(editingItem.value.id, {
        title: form.value.title,
        headerImage: editingItem.value.headerImage,
        sortOrder: editingItem.value.sortOrder,
        isActive: form.value.isActive,
        htmlContent: form.value.htmlContent,
        options: [],
      })
      ElMessage.success('分類已更新')
    } else {
      await serviceItemsApi.create({
        title: form.value.title.trim(),
        headerImage: null,
        sortOrder: items.value.length,
        isActive: true,
        htmlContent: form.value.htmlContent,
        options: [],
      })
      ElMessage.success('分類已建立')
    }
    showDialog.value = false
    await loadItems()
  } catch {
    ElMessage.error('儲存失敗')
  } finally {
    saving.value = false
  }
}

const initSortable = () => {
  const el = tableRef.value?.$el?.querySelector('.el-table__body-wrapper tbody')
  if (!el) return
  Sortable.create(el, {
    handle: '.drag-handle',
    animation: 150,
    onEnd: async ({ oldIndex, newIndex }) => {
      if (oldIndex === undefined || newIndex === undefined || oldIndex === newIndex) return
      const movedItem = items.value.splice(oldIndex, 1)[0]!
      items.value.splice(newIndex, 0, movedItem)
      try {
        await serviceItemsApi.updateSortOrder({
          items: items.value.map((item, index) => ({ id: item.id, sortOrder: index })),
        })
      } catch {
        ElMessage.error('排序更新失敗')
        await loadItems()
      }
    },
  })
}

const loadItems = async () => {
  loading.value = true
  try {
    const res = await serviceItemsApi.getAll()
    items.value = res.data.data ?? []
    await nextTick()
    initSortable()
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

<style>
.fullscreen-dialog .el-dialog__body {
  padding: 20px;
  max-height: calc(100vh - 140px);
  overflow-y: auto;
}
</style>
