import { Component, EventEmitter, Input, Output } from '@angular/core';
import { AccountService } from '../../_Services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  Model: any = {};
  @Output() CancelRegisteration = new EventEmitter();

  constructor(private accountServices: AccountService,private toast:ToastrService) { }
  Register() {
    this.accountServices.Register(this.Model).subscribe({
      next: () => {
        this.Cancel();
      },
      error: error => this.toast.error(error.error,"Failed")

    })

  }
  Cancel() {
    this.CancelRegisteration.emit(false);
  }
}
