<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useGoalsStore } from '@/stores/goals'
import { useTimerStore } from '@/stores/timer'
import { useRouter } from 'vue-router'

const goalsStore = useGoalsStore()
const timerStore = useTimerStore()
const router = useRouter()

const newGoalTitle = ref('')
const newGoalDesc = ref('')
const isCreating = ref(false)

onMounted(() => {
  goalsStore.fetchGoals()
})

const handleCreate = async () => {
  if (!newGoalTitle.value) return
  isCreating.value = true
  try {
    await goalsStore.createGoal({ title: newGoalTitle.value, description: newGoalDesc.value })
    newGoalTitle.value = ''
    newGoalDesc.value = ''
  } finally {
    isCreating.value = false
  }
}

const goToTimer = (goalId: string) => {
  router.push({ name: 'timer', query: { goalId } })
}
</script>

<template>
  <div class="space-y-8 max-w-5xl mx-auto">
    <div class="bg-zinc-900 border border-zinc-800 p-6 rounded-2xl">
      <h3 class="text-lg font-medium mb-4">New Goal</h3>
      <div class="flex gap-4 items-start">
        <div class="flex-1 space-y-3">
          <input
            v-model="newGoalTitle"
            type="text"
            placeholder="Goal title (e.g. Create a portfolio)"
            class="w-full px-4 py-2 bg-zinc-950 border border-zinc-700 rounded-lg focus:ring-2 focus:ring-indigo-500 outline-none"
          />
          <input
            v-model="newGoalDesc"
            type="text"
            placeholder="Description (optional)"
            class="w-full px-4 py-2 bg-zinc-950 border border-zinc-700 rounded-lg focus:ring-2 focus:ring-indigo-500 outline-none text-sm"
          />
        </div>
        <button
          @click="handleCreate"
          :disabled="isCreating || !newGoalTitle"
          class="px-6 py-2 bg-indigo-600 hover:bg-indigo-500 disabled:opacity-50 rounded-lg font-medium transition-colors h-[42px]"
        >
          Create
        </button>
      </div>
    </div>

    <div v-if="goalsStore.isLoading" class="text-zinc-500">Loading goals...</div>

    <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      <div
        v-for="goal in goalsStore.goals"
        :key="goal.id"
        class="bg-zinc-900 border border-zinc-800 p-5 rounded-2xl flex flex-col hover:border-zinc-700 transition-colors"
      >
        <div class="flex-1">
          <h4 class="text-lg font-bold text-white mb-1">{{ goal.title }}</h4>
          <p class="text-sm text-zinc-400 mb-4 line-clamp-2">
            {{ goal.description || 'No description' }}
          </p>

          <div class="flex space-x-4 text-xs font-medium text-zinc-500 mb-6">
            <span class="bg-zinc-950 px-2 py-1 rounded"
              >⏱ {{ goal.totalTimeSpentMinutes }} min</span
            >
            <span class="bg-zinc-950 px-2 py-1 rounded">🍅 {{ goal.sessionsCount }} sessions</span>
          </div>
        </div>

        <button
          @click="goToTimer(goal.id)"
          class="w-full py-2 bg-zinc-800 hover:bg-zinc-700 text-white rounded-lg font-medium transition-colors flex justify-center items-center gap-2"
        >
          <span
            v-if="timerStore.isRunning && timerStore.activeSession?.goalId === goal.id"
            class="text-indigo-400"
          >
            ▶ Return to timer
          </span>
          <span v-else> ▶ Focus </span>
        </button>
      </div>
    </div>
  </div>
</template>
