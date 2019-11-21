import { Component, OnInit } from '@angular/core';
import { Testrun } from 'src/app/interfaces/testrun';
import { ApiService } from 'src/app/services/api.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-testrun',
  templateUrl: './testrun.component.html',
  styleUrls: ['./testrun.component.scss']
})
export class TestrunComponent implements OnInit {

  public jsonParsed: Testrun;

  constructor(private apiService: ApiService, private router: Router) { }

  ngOnInit() {
    this.apiService.getData(this.router.url).subscribe((data: Testrun) => {
      this.jsonParsed = data;
    });
  }
}
