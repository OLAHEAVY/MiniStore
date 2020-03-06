import { NgModule } from "@angular/core";
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { NgxLoadingModule } from 'ngx-loading';

@NgModule({
    
    imports: [CommonModule, 
              NgxLoadingModule,
              RouterModule.forChild([
                  {path: '', loadChildren:() => import('./authentication/authentication.module').then(v => v.AuthenticationModule)}
              ])]
    

})

export class OrdinaryLayoutModule{}