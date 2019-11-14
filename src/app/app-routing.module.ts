import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { OrganizationComponent } from './pages/organization/organization.component';
import { HomeComponent } from './pages/home/home.component';
import { LoginComponent } from './pages/login/login.component';
import { ProjectComponent } from './pages/project/project.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    pathMatch: 'full',
    data: {
        breadcrumb: ''
    }
  },
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    // redirects all unknovn root URIs to OrganizationComponent
    path: ':term',
    children: [
      {
        path: '',
        component: OrganizationComponent,
      },
      {
        path: '**',
        component: ProjectComponent,
      },
    ]
  },
  {
    path: '**',
    redirectTo: ''
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
