export interface AuditLogDto {
  auditId: number;
  entityName: string;
  recordId: string;
  action: string;
  oldValues: string | null;
  newValues: string | null;
  createdBy: number | null;
  createdOn: string;
}
