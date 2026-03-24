Вот обработанный код: ```vue id="x7p2lm"
<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useTimerStore } from '@/stores/timer'
import { useGoalsStore } from '@/stores/goals'

const timerStore = useTimerStore()
const goalsStore = useGoalsStore()
const route = useRoute()
const router = useRouter()

const selectedGoalId = ref<string | null>(null)
const durationMinutes = ref(25)

onMounted(async () => {
  if (!goalsStore.goals.length) {
    await goalsStore.fetchGoals()
  }

  if (route.query.goalId) {
    selectedGoalId.value = route.query.goalId as string
  } else if (!timerStore.isRunning && goalsStore.goals.length > 0) {
    selectedGoalId.value = goalsStore.goals[0]?.id || null
  }
})

const currentGoalName = computed(() => {
  if (timerStore.isRunning) {
    return (
      goalsStore.goals.find((g) => g.id === timerStore.activeSession?.goalId)?.title ||
      'Unknown goal'
    )
  }
  return goalsStore.goals.find((g) => g.id === selectedGoalId.value)?.title || 'Select a goal...'
})

const handleStart = async () => {
  if (!selectedGoalId.value) return
  await timerStore.startSession(selectedGoalId.value, durationMinutes.value)
}

const handleStop = async () => {
  await timerStore.stopSession()
  router.push('/')
}
</script>

<template>
  <div class="flex-1 flex items-center justify-center h-full">
    <div
      class="bg-zinc-900 border border-zinc-800 p-10 rounded-3xl w-full max-w-md text-center shadow-2xl relative overflow-hidden"
    >
      <div
        v-if="timerStore.isRunning"
        class="absolute inset-0 bg-indigo-500/5 animate-pulse rounded-3xl pointer-events-none"
      ></div>

      <div class="relative z-10">
        <h2 class="text-zinc-400 font-medium mb-2 uppercase tracking-wider text-sm">
          Current goal
        </h2>
        <h3 class="text-xl font-bold text-white mb-8">{{ currentGoalName }}</h3>

        <div v-if="!timerStore.isRunning" class="space-y-8">
          <div class="flex justify-center items-center gap-4 text-zinc-300">
            <button
              @click="durationMinutes = 10"
              :class="{ 'text-white bg-zinc-800': durationMinutes === 10 }"
              class="px-3 py-1 rounded hover:bg-zinc-800"
            >
              10m
            </button>
            <button
              @click="durationMinutes = 25"
              :class="{ 'text-white bg-zinc-800': durationMinutes === 25 }"
              class="px-3 py-1 rounded hover:bg-zinc-800"
            >
              25m
            </button>
            <button
              @click="durationMinutes = 50"
              :class="{ 'text-white bg-zinc-800': durationMinutes === 50 }"
              class="px-3 py-1 rounded hover:bg-zinc-800"
            >
              50m
            </button>
          </div>

          <div class="text-[6rem] font-bold leading-none tracking-tighter text-zinc-100 font-mono">
            {{ durationMinutes }}:00
          </div>

          <button
            @click="handleStart"
            class="w-full py-4 bg-indigo-600 hover:bg-indigo-500 text-white rounded-2xl font-bold text-lg shadow-lg shadow-indigo-500/20 transition-all active:scale-95"
          >
            Start session
          </button>
        </div>

        <div v-else class="space-y-8">
          <div
            class="text-[6rem] font-bold leading-none tracking-tighter text-indigo-400 font-mono drop-shadow-[0_0_15px_rgba(99,102,241,0.3)]"
          >
            {{ timerStore.formattedTime }}
          </div>

          <p
            v-if="timerStore.timeLeftSeconds <= 0"
            class="text-green-400 font-medium animate-bounce"
          >
            Time is up! Great job.
          </p>

          <button
            @click="handleStop"
            class="w-full py-4 bg-zinc-800 hover:bg-red-500/20 hover:text-red-400 text-zinc-300 border border-zinc-700 hover:border-red-500/30 rounded-2xl font-bold text-lg transition-all active:scale-95"
          >
            Stop session
          </button>
          <p class="text-xs text-zinc-500 mt-2">Less than 3 minutes = discarded (not saved)</p>
        </div>
      </div>
    </div>
  </div>
</template>
```
