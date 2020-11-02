import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule } from 'ngx-toastr';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    BsDropdownModule.forRoot(),
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right', // toasts will appear on the bottom right
    }),
  ],
  exports: [BsDropdownModule, ToastrModule], // we exporting ds so it can be available everywhere in the app
})
export class SharedModule {}
