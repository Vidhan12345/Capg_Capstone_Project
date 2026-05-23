import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'statusColor',
  standalone: true
})
export class StatusColorPipe implements PipeTransform {
  transform(status: string): string {
    const map: Record<string, string> = {
      'Active': 'primary',
      'Inactive': 'warn',
      'Present': 'primary',
      'Absent': 'warn',
      'HalfDay': 'accent',
      'Approved': 'primary',
      'Rejected': 'warn',
      'Pending': 'accent',
      'NotStarted': '',
      'InProgress': 'primary',
      'Completed': 'primary',
      'OnHold': 'accent'
    };
    return map[status] || '';
  }
}
