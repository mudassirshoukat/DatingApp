export interface PaginationModel {
    CurrentPage: number;
    ItemsPerPage:number;
    TotalItems: number;
    TotalPages: number;
}


export class PaginationResult<T>{
    Result?: T;
    Pagination?: PaginationModel;
}