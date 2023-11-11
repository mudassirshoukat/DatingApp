import { Component, EventEmitter, Input, Output } from '@angular/core';
import { AccountService } from '../_Services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  Model: any = {};
  @Output() CancelRegisteration = new EventEmitter();

  constructor(private accountServices: AccountService) { }
  Register() {
    this.accountServices.Register(this.Model).subscribe({
      next: () => {
        this.Cancel();
      },
      error: error => console.log(error)

    })

  }
  Cancel() {
    this.CancelRegisteration.emit(false);
  }
}
