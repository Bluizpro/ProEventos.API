

import { CanActivateFn } from '@angular/router';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

export const authGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const toaster = inject(ToastrService);

  if (localStorage.getItem('user') !== null) {
    return true;
  } else {
    toaster.info('Usuário não autenticado!');
    router.navigate(['/user/login']);
    return false;
  }
};

  
