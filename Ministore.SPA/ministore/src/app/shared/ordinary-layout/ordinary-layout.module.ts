import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { RouterModule } from '@angular/router';



@NgModule({
    imports: [CommonModule,
             RouterModule.forChild([
                 {path: '', loadChildren: () => import('./authentication/authentication.module').then (a => a.AuthenticationModule)}
             ])],
    exports: [],
    providers: [],
    declarations:[]
})
export class OrdinaryLayoutModule { }
