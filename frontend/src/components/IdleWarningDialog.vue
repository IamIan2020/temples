<template>
  <el-dialog
    :model-value="visible"
    title="閒置提醒"
    width="400px"
    :close-on-click-modal="false"
    :close-on-press-escape="false"
    :show-close="false"
  >
    <div style="text-align: center">
      <p style="font-size: 16px; margin-bottom: 16px">您已閒置一段時間</p>
      <p style="font-size: 32px; font-weight: bold; color: #e6a23c; margin-bottom: 16px">
        {{ remainingSeconds }}
      </p>
      <p style="color: #909399">秒後將自動登出</p>
    </div>
    <template #footer>
      <el-button type="primary" @click="$emit('extend')">繼續使用</el-button>
      <el-button @click="$emit('logout')">立即登出</el-button>
    </template>
  </el-dialog>
</template>

<script setup lang="ts">
defineProps<{
  visible: boolean
  remainingSeconds: number
}>()

defineEmits<{
  extend: []
  logout: []
}>()
</script>
