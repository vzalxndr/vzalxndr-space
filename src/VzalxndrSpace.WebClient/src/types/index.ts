export interface Goal {
  id: string
  title: string
  description: string
  createdAtUtc: string
  status: number // 0 = Active, 1 = Completed, 2 = Archived
  completedAtUtc: string | null
  sessionsCount: number
  totalTimeSpentMinutes: number
}

export interface CreateGoalDto {
  title: string
  description?: string
}

export interface UpdateGoalDto {
  title: string
  description?: string
}

export interface Session {
  sessionId: string
  goalId: string
  startTimeUtc: string
  targetDurationMinutes: number
  status: number
}

// --- STATS DTOs ---
export interface HeatmapItem {
  date: string
  totalMinutes: number
}

export interface SessionsSummary {
  totalSessions: number
  totalFocusMinutes: number
  completedSessions: number
  interruptedSessions: number
}

export interface GoalsSummary {
  totalGoalsCreated: number
  totalGoalsCompleted: number
  goalsBreakdown: {
    goalId: string
    title: string
    totalMinutes: number
  }[]
}
