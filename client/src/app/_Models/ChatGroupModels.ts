export interface GroupModel{
    Name:string
    Connections:ConnectionModel[]
}

export interface ConnectionModel{
    ConnectionId :string
    UserName :string
}
