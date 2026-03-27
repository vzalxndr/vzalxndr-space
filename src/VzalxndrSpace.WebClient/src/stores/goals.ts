import { defineStore } from 'pinia'
import { ref } from 'vue'
import { apiClient } from '@/api/client'
import type { Goal, CreateGoalDto, UpdateGoalDto } from '@/types'

export const useGoalsStore = defineStore('goals', () => {
  const goals = ref<Goal[]>([])
  const isLoading = ref(false)

  const fetchGoals = async () => {
    isLoading.value = true
    try {
      const { data } = await apiClient.get<Goal[]>('/Goals')
      goals.value = data
    } catch (error) {
      console.error('Error while loading goals:', error)
    } finally {
      isLoading.value = false
    }
  }

  const createGoal = async (payload: CreateGoalDto) => {
    await apiClient.post('/Goals', payload)
    await fetchGoals()
  }

  const updateGoal = async (id: string, payload: UpdateGoalDto) => {
    await apiClient.put(`/Goals/${id}`, payload)
    await fetchGoals()
  }

  const completeGoal = async (id: string) => {
    await apiClient.patch(`/Goals/${id}/complete`)
    await fetchGoals()
  }

  const archiveGoal = async (id: string) => {
    await apiClient.patch(`/Goals/${id}/archive`)
    await fetchGoals()
  }

  return {
    goals,
    isLoading,
    fetchGoals,
    createGoal,
    updateGoal,
    completeGoal,
    archiveGoal,
  }
})
