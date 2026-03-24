import axios from 'axios'
import { useAuthStore } from '@/stores/auth.ts'
import router from '@/router'

const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5089/api'

export const apiClient = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json',
  },
})

// Перехватчик запросов: добавляем токен
apiClient.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token')
    if (token) {
      config.headers.Authorization = `Bearer ${token}`
    }
    return config
  },
  (error) => Promise.reject(error),
)

// Перехватчик ответов: глобальная обработка 401 Unauthorized
apiClient.interceptors.response.use(
  (response) => response,
  async (error) => {
    if (error.response && error.response.status === 401) {
      const authStore = useAuthStore()
      authStore.clearAuth() // Очищаем стейт без вызова API логаута
      await router.push('/login')
    }
    return Promise.reject(error)
  },
)
