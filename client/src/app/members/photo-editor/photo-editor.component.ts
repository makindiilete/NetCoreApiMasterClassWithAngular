import { Component, Input, OnInit } from '@angular/core';
import { Member } from '../../_models/member';
import { FileUploader } from 'ng2-file-upload';
import { environment } from '../../../environments/environment';
import { User } from '../../_models/user';
import { AccountService } from '../../_services/account.service';
import { take } from 'rxjs/operators';
import { MembersService } from '../../_services/members.service';
import { Photo } from '../../_models/photo';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css'],
})
export class PhotoEditorComponent implements OnInit {
  // ds component will receive a 'member' prop
  @Input() member: Member;
  uploader: FileUploader;
  hasBaseDropzoneOver = false;
  baseUrl = environment.apiUrl;
  user: User;
  constructor(
    private accountService: AccountService,
    private memberService: MembersService
  ) {
    this.accountService.currentUser$
      .pipe(take(1))
      .subscribe((user) => (this.user = user));
  }

  ngOnInit(): void {
    this.initializeUploader();
  }

  // we call ds method to update the main photo of the user
  setMainPhoto(photo: Photo) {
    this.memberService.setMainPhoto(photo.id).subscribe(() => {
      // we set our user main image to the url of the photo we've updated as the main photo on the API
      this.user.mainImage = photo.url;
      // we call accountService to update the user so the new main photo is also updated in the localStorage
      this.accountService.setCurrentUser(this.user);
      //we update the photoUrl (main image) field in our member model to the new main image
      this.member.photoUrl = photo.url;
      // we loop tru d array of photos in the members model and set the 'isMain' field of the current main image to false while we set the new main image 'isMain' property to true
      this.member.photos.forEach((value) => {
        // we set d old main image 'isMain' property to false
        if (value.isMain) {
          value.isMain = false;
        }
        // we set the 'isMain' property of the new image to true
        if (value.id === photo.id) {
          value.isMain = true;
        }
      });
    });
  }

  //we call ds method to delete a photo
  deletePhoto(photoId: number) {
    this.memberService.deletePhoto(photoId).subscribe(() => {
      // we set our member photos to all the photos remaining after we've removed the one deleted
      this.member.photos = this.member.photos.filter(
        (value) => value.id != photoId
      );
    });
  }

  fileOverBase(e: any) {
    this.hasBaseDropzoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      // d upload endpoint
      url: this.baseUrl + 'users/add-photo',
      // the token to attach to the upload endpoint bcos interceptor wont work for this
      authToken: 'Bearer ' + this.user.token,
      isHTML5: true,
      // d type of file we want to upload
      allowedFileType: ['image'],
      // if we want to remove the image from dropzone after upload
      removeAfterUpload: true,
      //if we set this to false, it means the user must click an upload button for the upload to start else the upload starts immediately when we drop picture in d dropzone
      autoUpload: false,
      // maximum file to upload (10mb)
      maxFileSize: 10 * 1024 * 1024,
    });
    this.uploader.onAfterAddingFile = (file) => {
      // ds is needed to b false so we wont need to make any adjustment to our endpoint cors config
      file.withCredentials = false;
    };
    //after the file has bin uploaded successfully
    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        // we get d photo from the response
        const photo = JSON.parse(response);
        // we push/add the photo to the array of photos of the member
        this.member.photos.push(photo);
      }
    };
  }
}
