import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, take } from 'rxjs';
import { AccountService } from '../_Services/account.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private accountService: AccountService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    this.accountService.CurrentUser$.pipe(take(1)).subscribe({
  next: user=>{
    if(user) {  request=request.clone({
    setHeaders:{
      Authorization: `Bearer ${user.Token}`
    }
    })}
  }
}); 
    return next.handle(request);
  }
}
