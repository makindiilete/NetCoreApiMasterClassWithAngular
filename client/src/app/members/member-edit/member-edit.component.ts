import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { Member } from '../../_models/member';
import { User } from '../../_models/user';
import { AccountService } from '../../_services/account.service';
import { MembersService } from '../../_services/members.service';
import { take } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css'],
})
export class MemberEditComponent implements OnInit {
  // ds gives us access to the form in our view using the 'editForm' template var
  @ViewChild('editForm') editForm: NgForm;
  member: Member;
  user: User;

  // ds shows a confirmation alert if user try to close the tab, close the browser etc..
  @HostListener('window:beforeunload', ['$event']) unloadNotification(
    $event: any
  ) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }

  //we'll use acctService to fetch d currently loggedIn user
  //we'll use memberService to fetch the details of the loggedIn user using the getMember() method
  constructor(
    private accountService: AccountService,
    private memberService: MembersService,
    private toastr: ToastrService
  ) {
    //Getting d loggedIn user
    this.accountService.currentUser$.pipe(take(1)).subscribe((user) => {
      this.user = user;
      console.log('User ooooo = ', user);
    });
  }

  // when d component loads, we fetch d member details using the loadMember() method
  ngOnInit(): void {
    this.loadMember();
  }

  //ds method call d getMember method frm memberService to fetch d details of the current user
  loadMember() {
    this.memberService.getMember(this.user.username).subscribe((member) => {
      this.member = member;
    });
  }

  //method to update a member
  updateMember() {
    this.memberService.updateMember(this.member).subscribe((res) => {
      this.toastr.success('Member Updated Successfully!');
      // we reset the form to the passed updated details
      // ds reset our form state so our submit button is disabled again and the alert is hidden
      this.editForm.reset(this.member);
      console.log('res = ', res);
    });
  }
}
