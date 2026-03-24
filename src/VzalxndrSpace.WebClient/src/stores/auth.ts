import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { apiClient } from '@/api/client'
import router from '@/router'

export const useAuthStore = defineStore('auth', () => {
  const token = ref<string | null>(localStorage.getItem('token'))

  const isAuthenticated = computed(() => !!token.value)

  const clearAuth = () => {
    token.value = null
    localStorage.removeItem('token')
  }

  const login = async (password: string) => {
    try {
      const { data } = await apiClient.post<{ token: string }>('/Auth/login', { password })
      token.value = data.token
      localStorage.setItem('token', data.token)
      await router.push('/')
    } catch (error) {
      console.error('Login failed', error)
      throw error
    }
  }

  const logout = async () => {
    clearAuth()
    await router.push('/login')
  }

  return {
    token,
    isAuthenticated,
    login,
    logout,
    clearAuth,
  }
})
