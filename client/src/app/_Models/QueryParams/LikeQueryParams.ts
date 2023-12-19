import { PaginationQueryParams } from "./PaginationQueryParams";

export class LikeQueryParams extends PaginationQueryParams {
    constructor() {
        super();
        this.Predicate = "Liked"
    }
    // PageNumber=1;
    // PageSize=5;

    UserId=0
    Predicate: string
}