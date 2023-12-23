import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AccountService } from '../_Services/account.service';
import { ToastrService } from 'ngx-toastr';
import { Observable, map, of, pipe, take } from 'rxjs';

export const adminGuard: CanActivateFn  =  ()=>  {

  let accountService = inject(AccountService);
  let toastService = inject(ToastrService);

 return accountService.CurrentUser$.pipe(
    map(user => {
      if (!user ) {
      console.log("user is empty")
        toastService.error("You cannot enter this area");
        return false;
      }
      if (user.Roles.includes("Admin") || user.Roles.includes("Moderator")) {
      console.log("user can access")
        return true;
      } else {
      console.log("else statment runs")
        toastService.error("you cannot enter this area")
        return false;
      }
    })
  ) 
};
