import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-test-error',
  templateUrl: './test-error.component.html',
  styleUrls: ['./test-error.component.css']
})
export class TestErrorComponent {
  private BaseUrl = environment.ApiUrl
  ValidationErrors:string[]=[];
  
   constructor(private http:HttpClient) {}

   Get404Error(){
    this.http.get(this.BaseUrl+"buggy/not-found").subscribe({
      next:Response=>console.log(Response),
      error:error=>console.log(error)
    })
   }



   Get401Error(){
    this.http.get(this.BaseUrl+"buggy/auth").subscribe({
      next:Response=>console.log(Response),
      error:error=>
        console.log(error)
     
      
    })
   }



   Get400Error(){
    this.http.get(this.BaseUrl+"buggy/bad-request").subscribe({
      next:Response=>console.log(Response),
      error:error=>console.log(error)
    })
   }



   Get500Error(){
    this.http.get(this.BaseUrl+"buggy/server-error").subscribe({
      next:Response=>console.log(Response),
      error:error=>console.log(error)
    })
   }



   Get400ValidationError(){
    this.http.post( this.BaseUrl+"account/register",{'password':"12"}).subscribe({
      next:Response=>console.log(Response),
      error:error=>{
        console.log(error)
        this.ValidationErrors=error
      }
    })
   }

}
