<script setup lang="ts">
import { useAuthStore } from '@/stores/auth'

const authStore = useAuthStore()

const handleLogout = async () => {
  await authStore.logout()
}
</script>

<template>
  <div class="min-h-screen flex bg-zinc-950 text-zinc-100">
    <aside class="w-64 flex flex-col bg-zinc-900 border-r border-zinc-800">
      <div class="h-16 flex items-center px-6 border-b border-zinc-800">
        <span class="text-lg font-bold tracking-tight text-white">VzalxndrSpace</span>
      </div>

      <nav class="flex-1 p-4 space-y-2 overflow-y-auto">
        <!-- Исправленная ссылка на Goals -->
        <router-link
          :to="{ name: 'goals' }"
          exact-active-class="bg-zinc-800/50 text-indigo-400"
          class="flex items-center px-3 py-2 text-zinc-400 hover:bg-zinc-800/50 hover:text-zinc-100 rounded-lg font-medium transition-colors"
        >
          🎯 Goals
        </router-link>

        <router-link
          :to="{ name: 'stats' }"
          active-class="bg-zinc-800/50 text-indigo-400"
          class="flex items-center px-3 py-2 text-zinc-400 hover:bg-zinc-800/50 hover:text-zinc-100 rounded-lg font-medium transition-colors"
        >
          📊 Statistics
        </router-link>
      </nav>

      <div class="p-4 border-t border-zinc-800">
        <button
          @click="handleLogout"
          class="w-full flex items-center justify-center px-4 py-2 text-sm text-zinc-400 hover:text-white hover:bg-zinc-800 rounded-lg transition-colors"
        >
          Log out
        </button>
      </div>
    </aside>

    <main class="flex-1 flex flex-col h-screen overflow-hidden">
      <header
        class="h-16 flex items-center justify-between px-8 border-b border-zinc-800 bg-zinc-950/50 backdrop-blur-sm"
      >
        <h2 class="text-xl font-semibold text-white">Dashboard</h2>
      </header>

      <div class="flex-1 overflow-auto p-8">
        <router-view />
      </div>
    </main>
  </div>
</template>
