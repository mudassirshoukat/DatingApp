import { Component } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-role-modal',
  templateUrl: './role-modal.component.html',
  styleUrls: ['./role-modal.component.css']
})
export class RoleModalComponent {
  UserName="";
  availableRoles:any[]=[]
  selectedRoles:any[]=[]

  constructor(public bsModalRef:BsModalRef){}

    updateChecked(role:string){
      var index= this.selectedRoles.indexOf(role);
      index!==-1?this.selectedRoles.splice(index,1):this.selectedRoles.push(role)
    }

}
