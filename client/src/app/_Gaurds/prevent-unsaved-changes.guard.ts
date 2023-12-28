import { CanDeactivateFn } from '@angular/router';
import { MemberEditComponent } from '../_components/members/member-edit/member-edit.component';
import { inject } from '@angular/core';
import { ConfirmService } from '../_Services/confirm.service';
import { Observable, of } from 'rxjs';

export const preventUnsavedChangesGuard: CanDeactivateFn<MemberEditComponent>
  = (component: MemberEditComponent): Observable<boolean> => {

    let confirmService = inject(ConfirmService)
    if (component.editform?.dirty)
      return confirmService.confim();

    return of(true);
  };
