import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Member } from '../_models/member';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members: Member[] = [];
  constructor(private http: HttpClient) {}

  //Data return type : - since our get request has been defined to return an array of member, d method will automatically detect dt its to return Observable of member array

  // getMembers(): Observable<Member[]> { // Not really needed to define d method return type once the request return type has been defined
  getMembers() {
    if (this.members.length > 0) {
      // we return the members as an observable using the 'of' rxjs operator
      return of(this.members);
    }
    return this.http.get<Member[]>(this.baseUrl + 'users').pipe(
      map((value) => {
        this.members = value;
        return value;
      })
    );
  }

  //ds return a single member using their username
  getMember(username: string) {
    // we first use the username arg to find the member in our members[]
    const member = this.members.find((value) => value.userName === username);
    if (member) {
      return of(member);
    }
    return this.http.get<Member>(this.baseUrl + `users/${username}`);
  }

  //ds method update a member
  updateMember(member: Member) {
    // when we update d details of a member, we need to update dt details too inside our members array
    return this.http.put(this.baseUrl + 'users', member).pipe(
      map(() => {
        // we look for the index of the passed member arg from our members array
        const index = this.members.indexOf(member);
        // now dt we av d index, we update the member with dt index to the passed member arg
        this.members[index] = member;
      })
    );
  }

  //this method set the photo with d passed id as the main photo for the user
  setMainPhoto(photoId: number) {
    //we do not need to send a body so we send an empty object
    return this.http.put(this.baseUrl + 'users/set-main-photo/' + photoId, {});
  }

  // ds method delete the photo with the given id
  deletePhoto(photoId: number) {
    return this.http.delete(this.baseUrl + 'users/delete-photo/' + photoId);
  }
}
