import { inject } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { MessageService } from '../_Services/message.service';
import { MemberModel } from '../_Models/MemberModel';
import { MembersService } from '../_Services/members.service';

export const memberDetailedResolver: ResolveFn<MemberModel> = (route, state) => {
  let memberService=inject(MembersService)
  
  return memberService.GetMember(route.paramMap.get('UserName')!);
};
