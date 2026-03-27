import { defineStore } from 'pinia'
import { ref } from 'vue'
import { apiClient } from '@/api/client'
import type { HeatmapItem, SessionsSummary, GoalsSummary } from '@/types'

export const useStatsStore = defineStore('stats', () => {
  const heatmap = ref<HeatmapItem[]>([])
  const sessionsSummary = ref<SessionsSummary | null>(null)
  const goalsSummary = ref<GoalsSummary | null>(null)
  const isLoading = ref(false)

  const fetchAllStats = async () => {
    isLoading.value = true
    try {
      const [heatmapRes, sessionsRes, goalsRes] = await Promise.all([
        apiClient.get<HeatmapItem[]>('/Stats/heatmap'),
        apiClient.get<SessionsSummary>('/Stats/sessions-summary'),
        apiClient.get<GoalsSummary>('/Stats/goals-summary'),
      ])

      heatmap.value = heatmapRes.data
      sessionsSummary.value = sessionsRes.data
      goalsSummary.value = goalsRes.data
    } catch (error) {
      console.error('Error while loading stats:', error)
    } finally {
      isLoading.value = false
    }
  }

  return {
    heatmap,
    sessionsSummary,
    goalsSummary,
    isLoading,
    fetchAllStats,
  }
})
