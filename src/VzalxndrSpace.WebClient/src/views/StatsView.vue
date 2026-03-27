<script setup lang="ts">
import { onMounted } from 'vue'
import { useStatsStore } from '@/stores/stats'

const statsStore = useStatsStore()

onMounted(() => {
  statsStore.fetchAllStats()
})
</script>

<template>
  <div class="max-w-5xl mx-auto space-y-8">
    <div class="flex justify-between items-end">
      <h2 class="text-2xl font-bold text-white">Activity Statistics</h2>
      <button
        @click="statsStore.fetchAllStats"
        class="text-sm text-zinc-400 hover:text-white flex items-center gap-1"
      >
        🔄 Refresh
      </button>
    </div>

    <div v-if="statsStore.isLoading" class="text-zinc-500">Loading data...</div>

    <template v-else>
      <div class="grid grid-cols-2 md:grid-cols-4 gap-4">
        <div class="bg-zinc-900 border border-zinc-800 p-5 rounded-2xl">
          <p class="text-zinc-400 text-sm mb-1">Total focus minutes</p>
          <p class="text-3xl font-bold text-indigo-400">
            {{ statsStore.sessionsSummary?.totalFocusMinutes || 0 }}
          </p>
        </div>
        <div class="bg-zinc-900 border border-zinc-800 p-5 rounded-2xl">
          <p class="text-zinc-400 text-sm mb-1">Completed sessions</p>
          <p class="text-3xl font-bold text-green-400">
            {{ statsStore.sessionsSummary?.completedSessions || 0 }}
          </p>
        </div>
        <div class="bg-zinc-900 border border-zinc-800 p-5 rounded-2xl">
          <p class="text-zinc-400 text-sm mb-1">Interrupted sessions</p>
          <p class="text-3xl font-bold text-red-400">
            {{ statsStore.sessionsSummary?.interruptedSessions || 0 }}
          </p>
        </div>
        <div class="bg-zinc-900 border border-zinc-800 p-5 rounded-2xl">
          <p class="text-zinc-400 text-sm mb-1">Goals completed</p>
          <p class="text-3xl font-bold text-white">
            {{ statsStore.goalsSummary?.totalGoalsCompleted || 0 }} /
            {{ statsStore.goalsSummary?.totalGoalsCreated || 0 }}
          </p>
        </div>
      </div>

      <div class="bg-zinc-900 border border-zinc-800 p-6 rounded-2xl">
        <h3 class="text-lg font-medium text-white mb-6">Activity Heatmap (Minutes)</h3>

        <div class="w-full overflow-x-auto pb-4">
          <div class="flex gap-1 min-w-max">
            <div v-for="week in 52" :key="week" class="flex flex-col gap-1">
              <div
                v-for="day in 7"
                :key="day"
                class="w-4 h-4 rounded-sm bg-zinc-800 hover:ring-2 ring-indigo-500 cursor-pointer"
                title="0 minutes"
              ></div>
            </div>
          </div>
          <div class="mt-4 text-xs text-zinc-500 flex justify-end items-center gap-2">
            Less
            <div class="w-3 h-3 bg-zinc-800 rounded-sm"></div>
            <div class="w-3 h-3 bg-indigo-900 rounded-sm"></div>
            <div class="w-3 h-3 bg-indigo-700 rounded-sm"></div>
            <div class="w-3 h-3 bg-indigo-500 rounded-sm"></div>
            More
          </div>
        </div>

        <details class="mt-4">
          <summary class="text-xs text-zinc-600 cursor-pointer">Show raw API data</summary>
          <pre class="text-[10px] text-zinc-500 mt-2 bg-zinc-950 p-2 rounded">{{
            statsStore.heatmap
          }}</pre>
        </details>
      </div>
    </template>
  </div>
</template>
