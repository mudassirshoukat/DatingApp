import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, delay, finalize } from 'rxjs';
import { BusyLoadingService } from '../_Services/busy-loading.service';

@Injectable()
export class BusyLoadingInterceptor implements HttpInterceptor {

  constructor(private loadingService:BusyLoadingService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
   this.loadingService.busy()
    return next.handle(request).pipe(
      delay(1000),
      finalize(()=>{this.loadingService.idle()})
    );
  }
}
