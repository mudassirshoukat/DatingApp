import { PaginationQueryParams } from "./PaginationQueryParams";
import { UserModel } from "../UserModel";

export class UserQueryParams extends PaginationQueryParams {
    
    constructor(user: UserModel) {
        super();
        this.Gender = user.Gender === 'male' ? 'female' : 'male'
    }
    MaxAge=99; 
    MinAge=18;
    Gender: string;
    OrderBy: string = "LastActive"
    
}