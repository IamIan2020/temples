<template>
  <div>
    <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 16px">
      <h2 style="margin: 0">權限群組管理</h2>
      <el-button type="primary" @click="$router.push('/backstage/roles/create')">新增角色</el-button>
    </div>

    <el-table v-loading="loading" :data="roles" border stripe>
      <el-table-column prop="name" label="角色名稱" width="200" />
      <el-table-column prop="description" label="描述" />
      <el-table-column label="權限數量" width="120" align="center">
        <template #default="{ row }">
          {{ row.permissions.length }}
        </template>
      </el-table-column>
      <el-table-column label="類型" width="100" align="center">
        <template #default="{ row }">
          <el-tag v-if="row.isBuiltIn" type="info" size="small">內建</el-tag>
          <el-tag v-else type="success" size="small">自訂</el-tag>
        </template>
      </el-table-column>
      <el-table-column label="操作" width="180" align="center">
        <template #default="{ row }">
          <el-button size="small" @click="$router.push(`/backstage/roles/${row.id}`)">編輯</el-button>
          <el-button
            size="small"
            type="danger"
            :disabled="row.isBuiltIn"
            @click="handleDelete(row)"
          >
            刪除
          </el-button>
        </template>
      </el-table-column>
    </el-table>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import { rolesApi } from '../../api/roles'
import type { RoleResponse } from '../../types/api'

const loading = ref(false)
const roles = ref<RoleResponse[]>([])

const loadRoles = async () => {
  loading.value = true
  try {
    const res = await rolesApi.getAllRoles()
    roles.value = res.data.data ?? []
  } catch {
    ElMessage.error('載入角色列表失敗')
  } finally {
    loading.value = false
  }
}

const handleDelete = async (role: RoleResponse) => {
  try {
    await ElMessageBox.confirm(`確定要刪除角色「${role.name}」嗎？`, '確認刪除', {
      type: 'warning',
      confirmButtonText: '確定',
      cancelButtonText: '取消',
    })
    await rolesApi.deleteRole(role.id)
    ElMessage.success('角色已刪除')
    await loadRoles()
  } catch (err: unknown) {
    if (err !== 'cancel') {
      ElMessage.error('刪除失敗')
    }
  }
}

onMounted(loadRoles)
</script>
