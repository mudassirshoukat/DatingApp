import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { LikeQueryParams } from '../_Models/QueryParams/LikeQueryParams';
// import { MembersService } from './members.service';
import { LikeMembersModel } from '../_Models/LikeMembersModel';
import { map } from 'rxjs';
import { PaginationResult } from '../_Models/PaginationModel';

@Injectable({
  providedIn: 'root'
})
export class LikeService {
  baseUrl: string = environment.ApiUrl
  queryPrms: LikeQueryParams;
  
  constructor(private http: HttpClient) {
    this.queryPrms = new LikeQueryParams();
  }

  getLikes() {
    var likePrms = this.getPaginatedHeader(this.getQueryPrms());
    return this.getpaginatedResult<LikeMembersModel[]>(this.baseUrl + `Like`, likePrms)
    
  

  }
 
  addLike(userName: string) {
    return this.http.post(this.baseUrl + 'Like/' + userName, {})
  }



  getQueryPrms() {
    return this.queryPrms;
  }

  setQueryPrms(prms: LikeQueryParams) {
    this.queryPrms = prms;

  }

  private getpaginatedResult<T>(url: string, params: HttpParams) {
 
    const paginatedResult: PaginationResult<T> = new PaginationResult<T>;

    return this.http.get<T>(url,{observe:'response',params}).pipe(
      map(res => {
      
        if (res.body) {
          paginatedResult.Result = res.body;
        }

        const pagination = res.headers.get('Pagination');
        if (pagination) {
          paginatedResult.Pagination = JSON.parse(pagination);

        }

        return paginatedResult;

      })
    )
  }


  private getPaginatedHeader(prms: LikeQueryParams) {
    var likePrms = new HttpParams();
    likePrms = likePrms.append("UserId", prms.UserId);
    likePrms = likePrms.append("PageNumber", prms.PageNumber);
    likePrms = likePrms.append("PageSize", prms.PageSize);
    likePrms = likePrms.append("Predicate", prms.Predicate);
    return likePrms
  }

}
