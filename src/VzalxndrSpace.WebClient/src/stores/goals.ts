import { defineStore } from 'pinia'
import { ref } from 'vue'
import { apiClient } from '@/api/client'
import type { Goal, CreateGoalDto } from '@/types'

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
    try {
      const { data } = await apiClient.post<Goal>('/Goals', payload)
      if (data && data.id) {
        goals.value.unshift(data)
      } else {
        await fetchGoals()
      }
    } catch (error) {
      console.error('Error while creating goal:', error)
      throw error
    }
  }

  return {
    goals,
    isLoading,
    fetchGoals,
    createGoal,
  }
})
