import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/login',
      name: 'Login',
      component: () => import('../views/LoginView.vue'),
      meta: { guest: true },
    },
    {
      path: '/register',
      name: 'Register',
      component: () => import('../views/RegisterView.vue'),
      meta: { guest: true },
    },
    {
      path: '/forgot-password',
      name: 'ForgotPassword',
      component: () => import('../views/ForgotPasswordView.vue'),
      meta: { guest: true },
    },
    {
      path: '/reset-password',
      name: 'ResetPassword',
      component: () => import('../views/ResetPasswordView.vue'),
      meta: { guest: true },
    },
    {
      path: '/',
      redirect: '/profile',
    },
    {
      path: '/profile',
      name: 'Profile',
      component: () => import('../views/ProfileView.vue'),
      meta: { requiresAuth: true },
    },
    {
      path: '/admin/members',
      name: 'MemberList',
      component: () => import('../views/admin/MemberListView.vue'),
      meta: { requiresAuth: true, roles: ['SystemAdmin', 'WebAdmin'] },
    },
    {
      path: '/admin/members/:id',
      name: 'MemberDetail',
      component: () => import('../views/admin/MemberDetailView.vue'),
      meta: { requiresAuth: true, roles: ['SystemAdmin', 'WebAdmin'] },
    },
  ],
})

router.beforeEach((to, _from, next) => {
  const authStore = useAuthStore()

  if (to.meta.requiresAuth && !authStore.isAuthenticated) {
    return next({ name: 'Login' })
  }

  if (to.meta.guest && authStore.isAuthenticated) {
    return next({ name: 'Profile' })
  }

  if (to.meta.roles) {
    const requiredRoles = to.meta.roles as string[]
    const hasRole = authStore.userRoles.some((r) => requiredRoles.includes(r))
    if (!hasRole) {
      return next({ name: 'Profile' })
    }
  }

  next()
})

export default router
