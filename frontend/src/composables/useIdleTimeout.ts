import { ref, onMounted, onUnmounted, watch } from 'vue'
import { useSettingsStore } from '../stores/settings'
import { useAuthStore } from '../stores/auth'

export function useIdleTimeout() {
  const settingsStore = useSettingsStore()
  const authStore = useAuthStore()

  const showWarning = ref(false)
  const remainingSeconds = ref(60)
  let lastActivity = Date.now()
  let checkInterval: ReturnType<typeof setInterval> | null = null
  let countdownInterval: ReturnType<typeof setInterval> | null = null

  const events = ['mousemove', 'keydown', 'click', 'scroll', 'touchstart']

  function resetActivity() {
    lastActivity = Date.now()
    if (showWarning.value) {
      showWarning.value = false
      remainingSeconds.value = 60
      if (countdownInterval) {
        clearInterval(countdownInterval)
        countdownInterval = null
      }
    }
  }

  function startCountdown() {
    remainingSeconds.value = 60
    showWarning.value = true
    countdownInterval = setInterval(() => {
      remainingSeconds.value--
      if (remainingSeconds.value <= 0) {
        doLogout()
      }
    }, 1000)
  }

  function doLogout() {
    stop()
    authStore.logout()
  }

  function checkIdle() {
    if (!authStore.isAuthenticated) return

    const timeoutMs = settingsStore.sessionTimeoutMinutes * 60 * 1000
    const warningMs = timeoutMs - 60 * 1000 // 登出前 1 分鐘警告
    const elapsed = Date.now() - lastActivity

    if (elapsed >= timeoutMs) {
      doLogout()
    } else if (elapsed >= warningMs && !showWarning.value) {
      startCountdown()
    }
  }

  function extend() {
    resetActivity()
  }

  function start() {
    lastActivity = Date.now()
    events.forEach(event => {
      document.addEventListener(event, resetActivity, { passive: true })
    })
    checkInterval = setInterval(checkIdle, 1000)
  }

  function stop() {
    events.forEach(event => {
      document.removeEventListener(event, resetActivity)
    })
    if (checkInterval) {
      clearInterval(checkInterval)
      checkInterval = null
    }
    if (countdownInterval) {
      clearInterval(countdownInterval)
      countdownInterval = null
    }
    showWarning.value = false
    remainingSeconds.value = 60
  }

  // 監聽登入狀態
  watch(() => authStore.isAuthenticated, (isAuth) => {
    if (isAuth) {
      start()
    } else {
      stop()
    }
  })

  onMounted(() => {
    if (authStore.isAuthenticated) {
      start()
    }
  })

  onUnmounted(() => {
    stop()
  })

  return {
    showWarning,
    remainingSeconds,
    extend,
    doLogout,
    start,
    stop,
  }
}
