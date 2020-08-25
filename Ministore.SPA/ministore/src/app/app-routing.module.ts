import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';


const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: '', loadChildren: () => import('./shared/ordinary-layout/ordinary-layout.module').then(x => x.OrdinaryLayoutModule) },
  {path: '', loadChildren: () => import('./customer/customer.module').then(x => x.CustomerModule)},
  { path: '', loadChildren: () => import('./shared/admin-layout/admin-layout.module').then(x => x.AdminLayoutModule)}
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
