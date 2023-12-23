import { PhotoModel } from "./PhotoModel";

export interface MemberModel {
  Id: number;
  UserName: string;
  PhotoUrl: string;
  Age: number;
  KnownAs: string;
  LastActive: string;
  Created: string;
  Gender: string;
  Introduction: string;
  LookingFor: string;
  Interests: string;
  City: string;
  Country: string;
  Photos: PhotoModel[];
}
