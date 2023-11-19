import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
import { MemberModel } from '../_Models/MemberModel';
import { environment } from 'src/environments/environment';
import { UserModel } from '../_Models/UserModel';
import { map, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  private baseurl: string = environment.ApiUrl
  members: MemberModel[] = []

  constructor(private http: HttpClient) { }

  GetMembers() {
    
  if (this.members.length>0) return of(this.members)

      return this.http.get<MemberModel[]>(this.baseurl + "users").pipe(
        map(res=>{
          this.members=res
          return this.members
        })
      );

  }

  GetMember(UserName: string) {
    if(this.members.find(x=>x.UserName===UserName)){
      return of(this.members.find(x=>x.UserName===UserName))
      
    }
    return this.http.get<MemberModel>(this.baseurl + "users/" + UserName)
  }

  UpdateMember(member: MemberModel) {
    return this.http.put(this.baseurl + "users", member).pipe(
      map(_=>{
        const index=this.members.indexOf(member);
        this.members[index]={...this.members[index],...member}
      })
    )
  }

}
