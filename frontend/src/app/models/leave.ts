export interface LeaveDto {
  leaveId: number;
  employeeId: number;
  employeeName: string | null;
  fromDate: string;
  toDate: string;
  leaveType: string;
  reason: string;
  status: string;
  approvedByName: string | null;
  appliedAt: string;
  reviewedAt: string | null;
}

export interface ApplyLeaveDto {
  fromDate: string;
  toDate: string;
  leaveType: string;
  reason: string;
}

export const LEAVE_TYPES = ['Sick', 'Casual', 'Earned'] as const;
export const LEAVE_STATUSES = ['Pending', 'Approved', 'Rejected'] as const;
