import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, map } from 'rxjs';
import { UserModel } from '../_Models/UserModel';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private CurrentUserSource = new BehaviorSubject<UserModel | null>(null)
  CurrentUser$ = this.CurrentUserSource.asObservable()
  private BaseUrl = environment.ApiUrl


  constructor(private http: HttpClient) { }


  LogIn(model: any) {
    return this.http.post<UserModel>(this.BaseUrl + "account/Login", model).pipe(
      map((response: UserModel) => {
        const user = response;
        if (user) {
          localStorage.setItem("user", JSON.stringify(user))
          this.CurrentUserSource.next(user)

        }
        return user;
      })
    )

  }


  Register(model: any) {
    return this.http.post<UserModel>(this.BaseUrl + "account/Register", model).pipe(
      map(user => {
        if (user) {
          this.CurrentUserSource.next(user);
          localStorage.setItem("user", JSON.stringify(user))
        }
        return user;
      })


    )

  }


  SetCurrentUser(User: UserModel) {
    this.CurrentUserSource.next(User);
  }

  Logout() {
    localStorage.removeItem("user");
    this.CurrentUserSource.next(null);
  }

  IsLogIn(): boolean {
    let check: boolean = false
    this.CurrentUser$.pipe(
      map(user => {
        if (user) check = true;

      })
    ).subscribe()
    return check
  }


 

}
