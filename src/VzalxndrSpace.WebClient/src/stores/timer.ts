import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { apiClient } from '@/api/client'
import type { Session } from '@/types'
import { useGoalsStore } from './goals'
import axios from 'axios'

export const useTimerStore = defineStore('timer', () => {
  const activeSession = ref<Session | null>(null)
  const timeLeftSeconds = ref(0)
  let intervalId: ReturnType<typeof setInterval> | null = null

  const isRunning = computed(() => activeSession.value !== null)

  const formattedTime = computed(() => {
    const m = Math.floor(Math.max(0, timeLeftSeconds.value) / 60)
      .toString()
      .padStart(2, '0')
    const s = (Math.max(0, timeLeftSeconds.value) % 60).toString().padStart(2, '0')
    return `${m}:${s}`
  })

  const tick = () => {
    if (timeLeftSeconds.value > 0) {
      timeLeftSeconds.value--
    } else {
    }
  }

  const startSession = async (goalId: string, durationMinutes: number = 25) => {
    try {
      const { data } = await apiClient.post<Session>('/Sessions/start', {
        goalId,
        targetDurationMinutes: durationMinutes,
      })

      activeSession.value = data
      timeLeftSeconds.value = durationMinutes * 60

      localStorage.setItem('activeSession', JSON.stringify(data))

      if (intervalId) clearInterval(intervalId)
      intervalId = setInterval(tick, 1000)
    } catch (error) {
      console.error('Error starting session:', error)
      throw error
    }
  }

  const stopSession = async () => {
    if (!activeSession.value) return

    try {
      await apiClient.post(`/Sessions/${activeSession.value.sessionId}/stop`)
      clearTimerState()

      const goalsStore = useGoalsStore()
      await goalsStore.fetchGoals()
    } catch (error) {
      if (axios.isAxiosError(error) && error.response?.status === 409) {
        clearTimerState()
        alert('The session lasted less than 3 minutes and was not saved (Discarded).')
      } else {
        console.error('Error stopping session:', error)
        throw error
      }
    }
  }

  const clearTimerState = () => {
    activeSession.value = null
    timeLeftSeconds.value = 0
    if (intervalId) clearInterval(intervalId)
    localStorage.removeItem('activeSession')
  }

  const restoreSession = () => {
    const saved = localStorage.getItem('activeSession')
    if (saved) {
      const session: Session = JSON.parse(saved)
      const startTime = new Date(session.startTimeUtc).getTime()
      const now = Date.now()
      const elapsedSeconds = Math.floor((now - startTime) / 1000)
      const targetSeconds = session.targetDurationMinutes * 60

      const remaining = targetSeconds - elapsedSeconds

      if (remaining > -3600) {
        activeSession.value = session
        timeLeftSeconds.value = remaining
        intervalId = setInterval(tick, 1000)
      } else {
        clearTimerState()
      }
    }
  }

  return {
    activeSession,
    timeLeftSeconds,
    isRunning,
    formattedTime,
    startSession,
    stopSession,
    restoreSession,
  }
})
