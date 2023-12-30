import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MemberModel } from '../_Models/MemberModel';
import { environment } from 'src/environments/environment';
import { UserModel } from '../_Models/UserModel';
import { map, of, take } from 'rxjs';
import { PaginationResult } from '../_Models/PaginationModel';
import { UserQueryParams } from '../_Models/QueryParams/UserQueryParams';
import { AccountService } from './account.service';


@Injectable({
  providedIn: 'root'
})
export class MembersService {
  private baseurl: string = environment.ApiUrl
  members: MemberModel[] = [];
  memberCache = new Map();
  queryPrms: UserQueryParams | undefined;
  user: UserModel | undefined;



  constructor(private http: HttpClient, private accountService: AccountService) {
    this.accountService.CurrentUser$.pipe(take(1)).subscribe({
      next: Response => {
        if (Response) {
          this.queryPrms = new UserQueryParams(Response);
          this.user = Response
        }
      }
    });
  }

  getQueryPrms() {
    return this.queryPrms;
  }

  setQueryPrms(prms: UserQueryParams) {
    this.queryPrms = prms;

  }

  resetQueryPrms() {
    if (this.user) {
      this.queryPrms = new UserQueryParams(this.user);
      return this.queryPrms

    }
    return;
  }


  GetMembers(prms: UserQueryParams) {
    
    var response = this.memberCache.get(Object.values(prms).join('-'));
    if (response) return of(response);

    var params = this.GetPaginatedHeader(prms.PageNumber, prms.PageSize);
    params = params.append('MinAge', prms.MinAge);
    params = params.append('MaxAge', prms.MaxAge);
    params = params.append('Gender', prms.Gender);
    params = params.append('OrderBy', prms.OrderBy);

   
    return this.GetpaginatedResult<MemberModel[]>(this.baseurl + "users", params).pipe(
      map(res => {
        console.log("my members")
        console.log("res")
        this.memberCache.set(Object.values(prms).join('-'), res);
        return res;
      })
    );

  }



  GetMember(UserName: string) {
    const member = [...this.memberCache.values()]
      .reduce((arr, e) => arr.concat(e.Result), [])
      .find((member: MemberModel) => member.UserName === UserName);

    if (member) return of(member)
    return this.http.get<MemberModel>(this.baseurl + "users/" + UserName)
  }
  

  UpdateMember(member: MemberModel) {
    return this.http.put(this.baseurl + "users", member).pipe(
      map(_ => {
        const index = this.members.indexOf(member);
        this.members[index] = { ...this.members[index], ...member }
      })
    )
  }



 

  private GetpaginatedResult<T>(url: string, params: HttpParams) {

    const paginatedResult: PaginationResult<T> = new PaginationResult<T>;

    return this.http.get<T>(url, { observe: 'response', params }).pipe(

      map(response => {
        if (response.body) {
          paginatedResult.Result = response.body;

        }
        const pagination = response.headers.get('Pagination');
        if (pagination) {
          paginatedResult.Pagination = JSON.parse(pagination);
        }

        return paginatedResult;
      })

    );
  }

  private GetPaginatedHeader(pageNumber: number, pageSize: number) {
    var params = new HttpParams();
    params = params.append("PageNumber", pageNumber);
    params = params.append("PageSize", pageSize);

    return params;
  }
 


}
