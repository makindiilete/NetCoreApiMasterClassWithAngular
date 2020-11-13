import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Member } from '../_models/member';

@Injectable({
  providedIn: 'root',
})
export class MembersService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) {}

  //Data return type : - since our get request has been defined to return an array of member, d method will automatically detect dt its to return Observable of member array

  // getMembers(): Observable<Member[]> { // Not really needed to define d method return type once the request return type has been defined
  getMembers() {
    return this.http.get<Member[]>(this.baseUrl + 'users');
  }

  //ds return a single member using their username
  getMember(username: string) {
    return this.http.get<Member>(this.baseUrl + `users/${username}`);
  }
}
