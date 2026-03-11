import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { authApi } from '../api/auth'
import type { UserInfo, LoginRequest, RegisterRequest } from '../types/api'

export const useAuthStore = defineStore('auth', () => {
  const user = ref<UserInfo | null>(null)
  const accessToken = ref<string | null>(null)

  const isAuthenticated = computed(() => !!accessToken.value)
  const userRoles = computed(() => user.value?.roles ?? [])
  const isAdmin = computed(() =>
    userRoles.value.includes('SystemAdmin') || userRoles.value.includes('WebAdmin')
  )
  const isSystemAdmin = computed(() => userRoles.value.includes('SystemAdmin'))
  const userPermissions = computed(() => user.value?.permissions ?? [])

  function hasPermission(permission: string): boolean {
    if (isSystemAdmin.value) return true
    return userPermissions.value.includes(permission)
  }

  function init() {
    const storedToken = localStorage.getItem('accessToken')
    const storedUser = localStorage.getItem('user')
    if (storedToken && storedUser) {
      accessToken.value = storedToken
      user.value = JSON.parse(storedUser)
    }

    // 跨 Tab 登出同步
    window.addEventListener('storage', (event) => {
      if (event.key === 'accessToken' && !event.newValue) {
        user.value = null
        accessToken.value = null
      }
    })
  }

  async function login(data: LoginRequest) {
    const res = await authApi.login(data)
    const loginData = res.data.data!
    accessToken.value = loginData.accessToken
    user.value = loginData.user
    localStorage.setItem('accessToken', loginData.accessToken)
    localStorage.setItem('refreshToken', loginData.refreshToken)
    localStorage.setItem('user', JSON.stringify(loginData.user))
  }

  async function register(data: RegisterRequest) {
    const res = await authApi.register(data)
    const loginData = res.data.data!
    accessToken.value = loginData.accessToken
    user.value = loginData.user
    localStorage.setItem('accessToken', loginData.accessToken)
    localStorage.setItem('refreshToken', loginData.refreshToken)
    localStorage.setItem('user', JSON.stringify(loginData.user))
  }

  function logout() {
    user.value = null
    accessToken.value = null
    localStorage.removeItem('accessToken')
    localStorage.removeItem('refreshToken')
    localStorage.removeItem('user')
  }

  return {
    user,
    accessToken,
    isAuthenticated,
    userRoles,
    isAdmin,
    isSystemAdmin,
    userPermissions,
    hasPermission,
    init,
    login,
    register,
    logout,
  }
})
