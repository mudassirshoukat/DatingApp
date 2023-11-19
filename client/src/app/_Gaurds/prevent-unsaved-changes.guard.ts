import { CanDeactivateFn } from '@angular/router';
import { MemberEditComponent } from '../_components/members/member-edit/member-edit.component';

export const preventUnsavedChangesGuard: CanDeactivateFn<MemberEditComponent> = (component:MemberEditComponent) :boolean => {
  if(component.editform?.dirty)
  return confirm("Are Sure Want To Continue? Any Unsaved changes will be lost")

  return true;
};
