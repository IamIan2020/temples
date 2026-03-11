<template>
  <div>
    <div style="display: flex; align-items: center; margin-bottom: 16px">
      <el-button @click="$router.push('/backstage/roles')" style="margin-right: 12px">返回</el-button>
      <h2 style="margin: 0">{{ isEdit ? '編輯角色' : '新增角色' }}</h2>
    </div>

    <el-card v-loading="loading" style="max-width: 700px">
      <el-form :model="form" label-width="100px" @submit.prevent="handleSave">
        <el-form-item label="角色名稱" required>
          <el-input v-model="form.name" placeholder="請輸入角色名稱" :maxlength="100" />
        </el-form-item>
        <el-form-item label="描述">
          <el-input
            v-model="form.description"
            type="textarea"
            placeholder="請輸入角色描述"
            :maxlength="500"
            :rows="3"
          />
        </el-form-item>
        <el-form-item label="權限設定">
          <div v-if="permissionGroups.length === 0" style="color: #909399">載入中...</div>
          <div v-for="group in permissionGroups" :key="group.groupName" style="margin-bottom: 12px">
            <div style="font-weight: bold; margin-bottom: 4px">{{ group.groupName }}</div>
            <el-checkbox-group v-model="form.permissions">
              <el-checkbox
                v-for="perm in group.permissions"
                :key="perm.code"
                :value="perm.code"
              >
                {{ perm.displayName }}
              </el-checkbox>
            </el-checkbox-group>
          </div>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" :loading="saving" @click="handleSave">
            {{ isEdit ? '更新' : '建立' }}
          </el-button>
        </el-form-item>
      </el-form>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { ElMessage } from 'element-plus'
import { rolesApi } from '../../api/roles'
import type { PermissionGroupResponse } from '../../types/api'

const route = useRoute()
const router = useRouter()

const roleId = computed(() => route.params.id as string | undefined)
const isEdit = computed(() => !!roleId.value)

const loading = ref(false)
const saving = ref(false)
const permissionGroups = ref<PermissionGroupResponse[]>([])

const form = ref({
  name: '',
  description: '' as string | null,
  permissions: [] as string[],
})

onMounted(async () => {
  loading.value = true
  try {
    // 載入權限群組
    const permRes = await rolesApi.getAllPermissions()
    permissionGroups.value = permRes.data.data ?? []

    // 編輯模式：載入角色資料
    if (roleId.value) {
      const roleRes = await rolesApi.getRoleById(roleId.value)
      const role = roleRes.data.data!
      form.value = {
        name: role.name,
        description: role.description,
        permissions: [...role.permissions],
      }
    }
  } catch {
    ElMessage.error('載入資料失敗')
  } finally {
    loading.value = false
  }
})

const handleSave = async () => {
  if (!form.value.name) {
    ElMessage.warning('請填寫角色名稱')
    return
  }
  saving.value = true
  try {
    if (isEdit.value) {
      await rolesApi.updateRole(roleId.value!, form.value)
      ElMessage.success('角色已更新')
    } else {
      await rolesApi.createRole(form.value)
      ElMessage.success('角色已建立')
    }
    router.push('/backstage/roles')
  } catch {
    ElMessage.error(isEdit.value ? '更新失敗' : '建立失敗')
  } finally {
    saving.value = false
  }
}
</script>
