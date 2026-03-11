<template>
  <div style="min-height: 100vh; display: flex; flex-direction: column">
    <PublicHeader @logout="handleLogout" />
    <main style="flex: 1">
      <router-view />
    </main>
    <PublicFooter />
    <IdleWarningDialog
      :visible="idleTimeout.showWarning.value"
      :remaining-seconds="idleTimeout.remainingSeconds.value"
      @extend="idleTimeout.extend"
      @logout="handleIdleLogout"
    />
  </div>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router'
import { useIdleTimeout } from '../../composables/useIdleTimeout'
import PublicHeader from './PublicHeader.vue'
import PublicFooter from './PublicFooter.vue'
import IdleWarningDialog from '../IdleWarningDialog.vue'

const router = useRouter()
const idleTimeout = useIdleTimeout()

const handleLogout = () => {
  router.push('/login')
}

const handleIdleLogout = () => {
  idleTimeout.doLogout()
  router.push('/login')
}
</script>
