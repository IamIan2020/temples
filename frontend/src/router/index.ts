import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import { useSettingsStore } from '../stores/settings'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    // 前台路由（使用 PublicLayout）
    {
      path: '/',
      component: () => import('../components/layout/PublicLayout.vue'),
      children: [
        {
          path: '',
          redirect: '/profile',
        },
        {
          path: 'profile',
          name: 'Profile',
          component: () => import('../views/public/ProfileView.vue'),
          meta: { requiresAuth: true },
        },
      ],
    },
    // 前台公開頁面（不使用 Layout）
    {
      path: '/login',
      name: 'Login',
      component: () => import('../views/public/LoginView.vue'),
      meta: { guest: true },
    },
    {
      path: '/register',
      name: 'Register',
      component: () => import('../views/public/RegisterView.vue'),
      meta: { guest: true },
    },
    {
      path: '/forgot-password',
      name: 'ForgotPassword',
      component: () => import('../views/public/ForgotPasswordView.vue'),
      meta: { guest: true },
    },
    {
      path: '/reset-password',
      name: 'ResetPassword',
      component: () => import('../views/public/ResetPasswordView.vue'),
      meta: { guest: true },
    },
    // 後台登入頁面（不使用 Layout）
    {
      path: '/backstage/login',
      name: 'BackstageLogin',
      component: () => import('../views/backstage/LoginView.vue'),
      meta: { guest: true, backstageGuest: true },
    },
    // 後台路由（使用 AdminLayout）
    {
      path: '/backstage',
      component: () => import('../components/layout/AdminLayout.vue'),
      meta: { requiresAuth: true, requiresAdmin: true },
      children: [
        {
          path: '',
          redirect: '/backstage/members',
        },
        {
          path: 'members',
          name: 'MemberList',
          component: () => import('../views/backstage/MemberListView.vue'),
          meta: { permission: 'members.view' },
        },
        {
          path: 'members/:id',
          name: 'MemberDetail',
          component: () => import('../views/backstage/MemberDetailView.vue'),
          meta: { permission: 'members.view' },
        },
        {
          path: 'settings',
          name: 'SystemSettings',
          component: () => import('../views/backstage/SystemSettingsView.vue'),
          meta: { permission: 'settings.view' },
        },
        {
          path: 'roles',
          name: 'RoleList',
          component: () => import('../views/backstage/RoleListView.vue'),
          meta: { permission: 'roles.manage' },
        },
        {
          path: 'roles/create',
          name: 'RoleCreate',
          component: () => import('../views/backstage/RoleDetailView.vue'),
          meta: { permission: 'roles.manage' },
        },
        {
          path: 'roles/:id',
          name: 'RoleEdit',
          component: () => import('../views/backstage/RoleDetailView.vue'),
          meta: { permission: 'roles.manage' },
        },
      ],
    },
  ],
})

router.beforeEach((to, _from, next) => {
  const authStore = useAuthStore()

  // 後台路由：需要登入 + 管理員角色
  if (to.meta.requiresAdmin) {
    if (!authStore.isAuthenticated) {
      return next({ name: 'BackstageLogin' })
    }
    if (!authStore.isAdmin) {
      return next({ name: 'Profile' })
    }
  }

  // 權限檢查：route meta 中的 permission
  if (to.meta.permission && typeof to.meta.permission === 'string') {
    if (!authStore.hasPermission(to.meta.permission)) {
      return next({ path: '/backstage' })
    }
  }

  // 前台受保護路由：需要登入
  if (to.meta.requiresAuth && !to.meta.requiresAdmin && !authStore.isAuthenticated) {
    return next({ name: 'Login' })
  }

  // Guest 頁面：已登入時重導
  if (to.meta.guest && authStore.isAuthenticated) {
    if (to.meta.backstageGuest) {
      // 後台登入頁：已登入的管理員導向後台
      if (authStore.isAdmin) {
        return next({ name: 'MemberList' })
      }
      return next({ name: 'Profile' })
    }
    return next({ name: 'Profile' })
  }

  next()
})

// 更新頁面標題
router.afterEach((to) => {
  const settingsStore = useSettingsStore()
  const title = to.name ? `${String(to.name)} - ${settingsStore.websiteName}` : settingsStore.websiteName
  document.title = title
})

export default router
