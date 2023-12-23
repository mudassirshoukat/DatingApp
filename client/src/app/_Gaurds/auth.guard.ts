import { inject } from "@angular/core";
import { AccountService } from "../_Services/account.service";
import { CanActivateFn } from "@angular/router";
import { ToastrService } from 'ngx-toastr';






export const authGuard: CanActivateFn =
  (route, state) => {
    let service = inject(AccountService);
    let toast = inject(ToastrService);
    if (service.IsLogIn()) {
    
      return true
    }
    toast.error("Danied");

    return false
  };



