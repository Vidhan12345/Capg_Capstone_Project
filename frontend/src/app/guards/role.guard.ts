import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const roleGuard = (route: any) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const allowedRoles: string[] = route?.data?.['roles'] ?? [];

  if (!allowedRoles.length) return true;
  if (authService.hasRole(...allowedRoles)) return true;
  return router.parseUrl('/unauthorized');
};
