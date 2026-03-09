<template>
  <div>
    <h2>會員管理</h2>
    <el-card>
      <div style="display: flex; margin-bottom: 16px">
        <el-input v-model="search" placeholder="搜尋 Email、名稱..." style="width: 300px; margin-right: 8px" clearable @keyup.enter="handleSearch" />
        <el-button type="primary" @click="handleSearch">搜尋</el-button>
      </div>

      <el-table :data="members" v-loading="loading" style="width: 100%">
        <el-table-column prop="email" label="Email" min-width="200" />
        <el-table-column prop="displayName" label="顯示名稱" width="150" />
        <el-table-column label="角色" width="120">
          <template #default="{ row }">
            <el-tag :type="getRoleTagType(row.roles[0])">{{ row.roles[0] }}</el-tag>
          </template>
        </el-table-column>
        <el-table-column label="狀態" width="80">
          <template #default="{ row }">
            <el-tag :type="row.isActive ? 'success' : 'danger'">{{ row.isActive ? '啟用' : '停用' }}</el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="createdAt" label="建立時間" width="180">
          <template #default="{ row }">
            {{ new Date(row.createdAt).toLocaleString('zh-TW') }}
          </template>
        </el-table-column>
        <el-table-column label="操作" width="100">
          <template #default="{ row }">
            <el-button type="primary" size="small" @click="router.push(`/admin/members/${row.id}`)">查看</el-button>
          </template>
        </el-table-column>
      </el-table>

      <el-pagination
        v-if="totalCount > 0"
        style="margin-top: 16px; justify-content: center"
        layout="total, prev, pager, next"
        :total="totalCount"
        :page-size="pageSize"
        :current-page="currentPage"
        @current-change="handlePageChange"
      />
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { membersApi } from '../../api/members'
import { ElMessage } from 'element-plus'
import type { MemberProfileResponse } from '../../types/api'

const router = useRouter()
const loading = ref(false)
const members = ref<MemberProfileResponse[]>([])
const search = ref('')
const currentPage = ref(1)
const pageSize = ref(20)
const totalCount = ref(0)

const getRoleTagType = (role: string) => {
  switch (role) {
    case 'SystemAdmin': return 'danger'
    case 'WebAdmin': return 'warning'
    default: return 'info'
  }
}

const loadMembers = async () => {
  loading.value = true
  try {
    const { data } = await membersApi.getMemberList({
      search: search.value || undefined,
      page: currentPage.value,
      pageSize: pageSize.value,
    })
    if (data.data) {
      members.value = data.data.items
      totalCount.value = data.data.totalCount
    }
  } catch {
    ElMessage.error('載入會員列表失敗')
  } finally {
    loading.value = false
  }
}

const handleSearch = () => {
  currentPage.value = 1
  loadMembers()
}

const handlePageChange = (page: number) => {
  currentPage.value = page
  loadMembers()
}

onMounted(loadMembers)
</script>
