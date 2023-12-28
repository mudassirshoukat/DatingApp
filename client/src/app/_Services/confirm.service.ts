import { Injectable } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ConfirmDialogComponent } from '../_components/modals/confirm-dialog/confirm-dialog.component';
import { Observable, map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ConfirmService {
  bsModalRef?: BsModalRef<ConfirmDialogComponent>

  constructor(private modalService: BsModalService) { }

  confim(
    title = "Confirmation",
    message = "Are you sure want to do this?",
    btnOkText = "Ok",
    btnCancelText = "Cancel"

  ): Observable<boolean> {
    const config = {
      initialState: {
        Title: title,
        Message: message,
        BtnOkText: btnOkText,
        BtnCancelText: btnCancelText
      }
    }
    this.bsModalRef = this.modalService.show(ConfirmDialogComponent, config);
    return this.bsModalRef.onHidden!.pipe(
      map(() => {
        return this.bsModalRef!.content!.result;
      })
    )
  }
}
