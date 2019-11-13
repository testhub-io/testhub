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
    data: {
        breadcrumb: ''
    }
  },
  {
    path: 'Test-org',
    data: {
      breadcrumb: 'Test-org'
    },
    children: [
      {
        path: '',
        component: OrganizationComponent,
      },
      {
        path: 'TestDataUpload-HugeReport',
        component: ProjectComponent,
        data: {
          breadcrumb: 'TestDataUpload-HugeReport'
        },
      },
      {
        path: '**',
        redirectTo: ''
      }
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
