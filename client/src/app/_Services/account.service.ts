import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, map } from 'rxjs';
import { User } from '../_Models/User';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private CurrentUserSource = new BehaviorSubject<User | null>(null)
  CurrentUser$ = this.CurrentUserSource.asObservable()
  private BaseUrl = "https://localhost:44381/api/Account/"


  constructor(private http: HttpClient) { }


  LogIn(model: any) {
    return this.http.post<User>(this.BaseUrl + "Login", model).pipe(
      map((response: User) => {
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
    return this.http.post<User>(this.BaseUrl + "Register", model).pipe(
      map(user => {
        if (user) {
          this.CurrentUserSource.next(user);
          localStorage.setItem("user", JSON.stringify(user))
        }
        return user;
      })


    )

  }


  SetCurrentUser(User: User) {
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
