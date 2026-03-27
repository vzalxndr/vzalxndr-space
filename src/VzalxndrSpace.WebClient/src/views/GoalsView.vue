<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { useGoalsStore } from '@/stores/goals'
import { useRouter } from 'vue-router'

const goalsStore = useGoalsStore()
const router = useRouter()

const newGoalTitle = ref('')
const newGoalDesc = ref('')
const isCreating = ref(false)

// Edit mode state
const editingId = ref<string | null>(null)
const editTitle = ref('')
const editDesc = ref('')

onMounted(() => {
  goalsStore.fetchGoals()
})

// Filter: show only active goals (status === 0)
const activeGoals = computed(() => goalsStore.goals.filter((g) => g.status === 0))

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

// --- CRUD Actions ---

// Define a minimal type for the goal parameter to satisfy TS
interface GoalItem {
  id: string
  title: string
  description: string
}

const startEdit = (goal: GoalItem) => {
  editingId.value = goal.id
  editTitle.value = goal.title
  editDesc.value = goal.description
}

const cancelEdit = () => {
  editingId.value = null
}

const saveEdit = async (id: string) => {
  if (!editTitle.value) return
  await goalsStore.updateGoal(id, { title: editTitle.value, description: editDesc.value })
  editingId.value = null
}

const handleComplete = async (id: string) => {
  if (window.confirm('Are you sure you want to mark this goal as completed? 🎉')) {
    await goalsStore.completeGoal(id)
  }
}

const handleArchive = async (id: string) => {
  if (window.confirm('Move this goal to archive? It will be hidden from active goals.')) {
    await goalsStore.archiveGoal(id)
  }
}
</script>

<template>
  <div class="space-y-8 max-w-5xl mx-auto">
    <!-- Create form -->
    <div class="bg-zinc-900 border border-zinc-800 p-6 rounded-2xl">
      <h3 class="text-lg font-medium mb-4">New Goal</h3>
      <div class="flex gap-4 items-start">
        <div class="flex-1 space-y-3">
          <input
            v-model="newGoalTitle"
            type="text"
            placeholder="Goal title..."
            class="w-full px-4 py-2 bg-zinc-950 border border-zinc-700 rounded-lg focus:ring-2 focus:ring-indigo-500 outline-none"
          />
          <input
            v-model="newGoalDesc"
            type="text"
            placeholder="Description..."
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

    <!-- Goals list -->
    <div v-if="goalsStore.isLoading" class="text-zinc-500">Loading goals...</div>
    <div v-else-if="activeGoals.length === 0" class="text-zinc-500 text-center py-10">
      No active goals. Create your first one!
    </div>

    <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      <div
        v-for="goal in activeGoals"
        :key="goal.id"
        class="bg-zinc-900 border border-zinc-800 p-5 rounded-2xl flex flex-col hover:border-zinc-700 transition-colors group relative"
      >
        <!-- Action buttons -->
        <div
          class="absolute top-4 right-4 opacity-0 group-hover:opacity-100 transition-opacity flex gap-2"
        >
          <button
            @click="startEdit(goal)"
            title="Edit"
            class="p-1.5 bg-zinc-800 hover:bg-zinc-700 rounded text-zinc-400 hover:text-white"
          >
            ✏️
          </button>
          <button
            @click="handleComplete(goal.id)"
            title="Complete"
            class="p-1.5 bg-zinc-800 hover:bg-green-500/20 rounded text-zinc-400 hover:text-green-400"
          >
            ✅
          </button>
          <button
            @click="handleArchive(goal.id)"
            title="Archive"
            class="p-1.5 bg-zinc-800 hover:bg-red-500/20 rounded text-zinc-400 hover:text-red-400"
          >
            🗑️
          </button>
        </div>

        <!-- View mode -->
        <div v-if="editingId !== goal.id" class="flex-1 mt-2">
          <h4 class="text-lg font-bold text-white mb-1 pr-16">{{ goal.title }}</h4>
          <p class="text-sm text-zinc-400 mb-4 line-clamp-2">
            {{ goal.description || 'No description' }}
          </p>
          <div class="flex space-x-3 text-xs font-medium text-zinc-500 mb-6">
            <span class="bg-zinc-950 px-2 py-1 rounded border border-zinc-800"
              >⏱ {{ goal.totalTimeSpentMinutes }} min</span
            >
            <span class="bg-zinc-950 px-2 py-1 rounded border border-zinc-800"
              >🍅 {{ goal.sessionsCount }} sessions</span
            >
          </div>
        </div>

        <!-- Edit mode -->
        <div v-else class="flex-1 space-y-3 mt-2 mb-4">
          <input
            v-model="editTitle"
            type="text"
            class="w-full px-3 py-1.5 bg-zinc-950 border border-zinc-700 rounded focus:ring-1 focus:ring-indigo-500 outline-none text-sm"
          />
          <textarea
            v-model="editDesc"
            rows="2"
            class="w-full px-3 py-1.5 bg-zinc-950 border border-zinc-700 rounded focus:ring-1 focus:ring-indigo-500 outline-none text-sm resize-none"
          ></textarea>
          <div class="flex gap-2">
            <button
              @click="saveEdit(goal.id)"
              class="flex-1 py-1.5 bg-indigo-600 hover:bg-indigo-500 rounded text-white text-sm"
            >
              Save
            </button>
            <button
              @click="cancelEdit"
              class="flex-1 py-1.5 bg-zinc-800 hover:bg-zinc-700 rounded text-white text-sm"
            >
              Cancel
            </button>
          </div>
        </div>

        <button
          v-if="editingId !== goal.id"
          @click="goToTimer(goal.id)"
          class="w-full py-2.5 bg-zinc-950 hover:bg-zinc-800 border border-zinc-800 text-white rounded-xl font-medium transition-colors flex justify-center items-center gap-2"
        >
          ▶ Focus
        </button>
      </div>
    </div>
  </div>
</template>
