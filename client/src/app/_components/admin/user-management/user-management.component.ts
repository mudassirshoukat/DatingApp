import { Component } from '@angular/core';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { take } from 'rxjs';
import { UserModel } from 'src/app/_Models/UserModel';
import { AdminService } from 'src/app/_Services/admin.service';
import { RoleModalComponent } from '../../modals/role-modal/role-modal.component';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent {
  users: UserModel[] = []
  bsModalRef: BsModalRef<RoleModalComponent> = new BsModalRef<RoleModalComponent>();
  availableRoles = ['Admin', 'Moderator', 'Member'];

  constructor(private adminService: AdminService, private modalService: BsModalService) {
    this.getUsersWithRoles();
  }

  getUsersWithRoles() {
    this.adminService.getUsersWithRoles().pipe(take(1)).subscribe({
      next: res => {
        if (res) this.users = res
      }
    })
  }

  openRolesModal(user: UserModel) {

    const config = {
      class: "modal-dialog-centered",
      initialState: {
        userName: user.UserName,
        availableRoles: this.availableRoles,
        selectedRoles: [...user.Roles]
      }
    }
    this.bsModalRef = this.modalService.show(RoleModalComponent, config);
    this.bsModalRef.onHide?.subscribe({
      next:()=>{
        const selectedRoles= this.bsModalRef.content?.selectedRoles;
        if(!this.arrayEqual(selectedRoles!,user.Roles)){
          this.adminService.updateUserRoles(user.UserName,selectedRoles!).subscribe({
            next:roles=> user.Roles=roles
            
          })
        }
      }
    })

}


private arrayEqual(arr1:any[],arr2:any[]){
  return JSON.stringify(arr1.sort())===JSON.stringify(arr2.sort());
}
}
