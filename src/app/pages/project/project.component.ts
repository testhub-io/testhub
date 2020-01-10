import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { Router } from '@angular/router';
import { Project } from '../../interfaces/project';
import { Testrun } from '../../interfaces/testrun';

@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.scss']
})
export class ProjectComponent implements OnInit {

  public jsonParsed: Project;

  constructor(private apiService: ApiService, private router: Router) { }

  public barChartOptions = {
    tooltips: {
      mode: 'index'
    },
    scaleShowVerticalLines: false,
    responsive: true,
    scales: {
      yAxes: [{
        scaleLabel: {
          display: true,
          // labelString: 'Label of y-Axes'
        },
        stacked: true
      }]
    },

  };

  public chartColors: Array<any> = [
    { // first color - yellow
      backgroundColor: 'rgba(246,183,46,1)',
      borderColor: 'rgba(226,163,36,1)',
      pointBackgroundColor: '#fff',
      pointBorderColor: 'rgba(226,163,36,1)',
      pointHoverBackgroundColor: 'rgba(226,163,36,1)',
      pointHoverBorderColor: '#fff'
    },
    { // second color - red
      backgroundColor: 'rgba(222,100,100,1)',
      borderColor: 'rgba(180,80,80,1)',
      pointBackgroundColor: '#fff',
      pointBorderColor: 'rgba(180,80,80,1)',
      pointHoverBackgroundColor: 'rgba(180,80,80,1)',
      pointHoverBorderColor: '#fff'
    },
    { // third color - blue
      backgroundColor: 'rgba(95,162,231,1)',
      borderColor: 'rgba(65,132,201,1)',
      pointBackgroundColor: '#fff',
      pointBorderColor: 'rgba(65,132,201,1)',
      pointHoverBackgroundColor: 'rgba(65,132,201,1)',
      pointHoverBorderColor: '#fff'
    }
  ];

  public barChartLabels = [];
  public barChartType = 'line';
  public barChartLegend = true;

  public barChartData = [];


  ngOnInit() {
    this.barChartData = [
      {data: [], label: 'Skipped'},
      {data: [], label: 'Failed'},
      {data: [], label: 'Passed'}
    ];

    this.apiService.getData(this.router.url).subscribe((data: Project) => {
      this.jsonParsed = data;

      // Loading chart data sets into barChartData array
      this.jsonParsed.testRuns.forEach((element: Testrun) => {
        this.barChartData[0].data.push(element.count.skipped);
        this.barChartData[1].data.push(element.count.failed);
        this.barChartData[2].data.push(element.count.passed);
        this.barChartLabels.push(element.timestamp.split('T')[0]);
      });
      console.log(this.router.url);
    });

  }


}
