import { Component } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.css']
})
export class ConfirmDialogComponent {
  Title = ""
  Message = ''
  BtnOkText = ""
  BtnCancelText = ''
  result = false
  constructor(public bsModalRef: BsModalRef) {}

  confirm() {
    this.result = true
    this.bsModalRef.hide()
  }

  decline() {
    this.bsModalRef.hide();
  }

}
