<script setup lang="ts">
import { ref } from 'vue'
import { useAuthStore } from '@/stores/auth'

const authStore = useAuthStore()
const password = ref('')
const errorMsg = ref('')
const isLoading = ref(false)

const handleLogin = async () => {
  if (!password.value) return

  errorMsg.value = ''
  isLoading.value = true

  try {
    await authStore.login(password.value)
  } catch {
    errorMsg.value = 'Invalid password or server error'
  } finally {
    isLoading.value = false
  }
}
</script>

<template>
  <div
    class="min-h-screen flex items-center justify-center bg-zinc-950 text-zinc-100 selection:bg-indigo-500/30"
  >
    <div class="w-full max-w-sm p-8 bg-zinc-900 border border-zinc-800 rounded-2xl shadow-xl">
      <div class="mb-8 text-center">
        <h1 class="text-2xl font-bold tracking-tight text-white">VzalxndrSpace</h1>
        <p class="text-sm text-zinc-400 mt-1">Admin panel login</p>
      </div>

      <form @submit.prevent="handleLogin" class="space-y-4">
        <div>
          <label for="password" class="sr-only">Password</label>
          <input
            id="password"
            v-model="password"
            type="password"
            required
            placeholder="Enter password..."
            class="w-full px-4 py-3 bg-zinc-950 border border-zinc-700 rounded-xl text-zinc-100 placeholder-zinc-500 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all"
          />
        </div>

        <p v-if="errorMsg" class="text-sm text-red-400 text-center">{{ errorMsg }}</p>

        <button
          type="submit"
          :disabled="isLoading"
          class="w-full py-3 px-4 bg-indigo-600 hover:bg-indigo-500 text-white rounded-xl font-medium transition-colors focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2 focus:ring-offset-zinc-900 disabled:opacity-50 disabled:cursor-not-allowed"
        >
          {{ isLoading ? 'Logging in...' : 'Log in' }}
        </button>
      </form>
    </div>
  </div>
</template>
