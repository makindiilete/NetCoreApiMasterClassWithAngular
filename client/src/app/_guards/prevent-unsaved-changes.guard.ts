import { Injectable } from '@angular/core';
import {
  CanActivate,
  CanDeactivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { Observable } from 'rxjs';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';

@Injectable({
  providedIn: 'root',
})
export class PreventUnsavedChangesGuard
  implements CanActivate, CanDeactivate<unknown> {
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    return true;
  }
  // canDeactivate takes a component as argument and with dt we have access to all variables, methods and all members of the component
  canDeactivate(component: MemberEditComponent): boolean {
    // since we now av access to the MemberEditComponent, we can check if the form is dirty i.e. d user made some changes and then wanna leave the page..
    if (component.editForm.dirty) {
      // we show an alert, if the user choose yes,
      return confirm(
        'Are you sure you want to continue? Any unsaved changes will be lost'
      );
    }
    // we return true which means we can navigate away from the component else we remain on the page
    return true;
  }
}
