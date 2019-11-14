import { Component} from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { IBreadCrumb } from '../interfaces/ibredcrumb';
import { filter} from 'rxjs/operators';

@Component({
  selector: 'app-breadcrumb',
  templateUrl: './breadcrumb.component.html',
  styleUrls: ['./breadcrumb.component.scss']
})

export class BreadcrumbComponent{
  public breadcrumbs: IBreadCrumb[] = [];
  constructor(private router: Router) {
    this.router.events
    .pipe(filter(event => event instanceof NavigationEnd))
    .subscribe(() => {
      this.breadcrumbs = [];
      let ar = window.location.pathname.split('/');
      ar.shift();
      let linkBuilder = '';
      ar.forEach((element) => {
        linkBuilder += '/' + element;
        this.breadcrumbs.push({label: element, url: linkBuilder});
      });
    });
  }

}
