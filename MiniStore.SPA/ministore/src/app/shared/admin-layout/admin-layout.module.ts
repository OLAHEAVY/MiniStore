import { NgModule } from "@angular/core";
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AdminLayoutComponent } from './admin-layout.component';
import { AdminHeaderComponent } from './admin-header/admin-header.component';

@NgModule({
    imports: [CommonModule,
    RouterModule.forChild([
        {
            path: '',
            component: AdminLayoutComponent,
            canActivate: [],
            children:[
                
            ]
        }
    ])],
    declarations: [AdminLayoutComponent, AdminHeaderComponent],
    exports:[],
    providers: []
})

export class AdminLayout{}