import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { OrganizationComponent } from './pages/organization/organization.component';
import { HomeComponent } from './pages/home/home.component';
import { LoginComponent } from './pages/login/login.component';
import { ProjectComponent } from './pages/project/project.component';
import { TestrunComponent } from './pages/testrun/testrun.component';

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
    path: ':term',
    component: OrganizationComponent,
  },
  {    
    path: ':term/:term2',
    component: ProjectComponent,
  },
  {    
    path: ':term/:term2/:term3',
    component:  TestrunComponent,
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
