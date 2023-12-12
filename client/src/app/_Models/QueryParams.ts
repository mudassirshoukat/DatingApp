import { UserModel } from "./UserModel";

export class QueryParams {
    Gender:string;
    MaxAge=99; 
    MinAge=18;
    PageNumber=1;
    PageSize=5;
    OrderBy:string="LastActive"
    constructor(user:UserModel) {
        this.Gender=user.Gender==='male'?'female':'male'
    }
}