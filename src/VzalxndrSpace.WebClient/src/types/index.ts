export interface Goal {
  id: string
  title: string
  description: string
  createdAtUtc: string
  status: number
  completedAtUtc: string | null
  sessionsCount: number
  totalTimeSpentMinutes: number
}

export interface CreateGoalDto {
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
