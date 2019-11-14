import { Component, OnInit, ɵCompiler_compileModuleSync__POST_R3__ } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { Router } from '@angular/router';
import { Organisation } from '../../interfaces/organisation';
import { BreadcrumbComponent } from '../../breadcrumb/breadcrumb.component';

@Component({
  selector: 'app-organization',
  templateUrl: './organization.component.html',
  styleUrls: ['./organization.component.scss']
})
export class OrganizationComponent implements OnInit {

  public jsonParsed: Organisation;

  constructor(private apiService: ApiService, private router: Router) { }

  ngOnInit() {
    this.apiService.getData(this.router.url).subscribe((data: Organisation) => {
      this.jsonParsed = data;
    });
  }

}
