import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../../_Services/account.service';
import { ToastrService } from 'ngx-toastr';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  
  registerForm: FormGroup = new FormGroup({});
  @Output() CancelRegisteration = new EventEmitter();
  maxDate: Date = new Date();
  validationErrors:string[]|undefined;

  constructor(
    private accountServices: AccountService,
     private toast: ToastrService,
    private fb: FormBuilder,
    private route:Router
    ) {

  }

  ngOnInit(): void {
    this.maxDate?.setFullYear(this.maxDate.getFullYear() - 18);
    this.initializeForm();

  }

  initializeForm() {
    this.registerForm = this.fb.group({
      Gender: ["Male",],
      KnownAs: ["", Validators.required],
      DateOfBirth: ["", Validators.required],
      City: ["", Validators.required],
      Country: ["", Validators.required],
      UserName: ["", Validators.required],
      Password: ["", [Validators.required, Validators.minLength(6), Validators.maxLength(15)]],
      ConfirmPassword: ["", [Validators.required, this.matchesValues('Password')]]

    });
    this.registerForm.controls["Password"].valueChanges.subscribe({
      next: _ => this.registerForm.controls["ConfirmPassword"].updateValueAndValidity()

    })
  }

  matchesValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control.value === control.parent?.get(matchTo)?.value ? null : { notMatching: true }
    }

  }





  Register() {
    const date= this.getDateOnly(this.registerForm.controls['DateOfBirth'].value);
    const value= {...this.registerForm.value,DateOfBirth:date};
console.log(value)
    this.accountServices.Register(value).subscribe({
      next: () => {
        this.route.navigateByUrl('/members')
        
      },
      error: error => this.validationErrors=error

    })

  }

private getDateOnly(dob:string|undefined){
  if(!dob) return;
  var theDob=new Date(dob);
  return new Date(theDob.setMinutes(theDob.getMinutes()-theDob.getTimezoneOffset()))
  .toISOString().slice(0,10);
}


  Cancel() {
    this.CancelRegisteration.emit(false);
  }
}
