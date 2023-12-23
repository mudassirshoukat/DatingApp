import { Directive, Input, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { UserModel } from '../_Models/UserModel';
import { AccountService } from '../_Services/account.service';
import { take } from 'rxjs';

@Directive({
  selector: '[appHasRole]'
})
export class HasRoleDirective implements OnInit {
  @Input() appHasRole: string[] = []

  user: UserModel = {} as UserModel

  constructor(private viewContainerRef: ViewContainerRef, private templateRef: TemplateRef<any>, private accounService: AccountService) {
    this.accounService.CurrentUser$.pipe(take(1)).subscribe({
      next: user => {
        if (user)
          this.user = user
      }
    })
  }



  ngOnInit(): void {
    if (this.user.Roles.some(r => this.appHasRole.includes(r))) {
      this.viewContainerRef.createEmbeddedView(this.templateRef);
    }

    else {
      this.viewContainerRef.clear();
    }
  }

}
