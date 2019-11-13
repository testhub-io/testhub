import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.scss']
})
export class ProjectComponent implements OnInit {

  // This var should receive parsed project JSON data from back-end API
  public jsonParsed = JSON.parse('{"Project":"TestDataUpload-Regular","TestRunsCount":20,"TestRuns":[{"Name":"VS Test result","TestRunName":"3","Time":204.15,"Timestamp":"2019-08-02T20:20:14","uri":"Test-org/TestDataUpload-Regular/3","Count":{"Passed":906,"Failed":0,"Skipped":1}},{"Name":"VS Test result","TestRunName":"4","Time":204.15,"Timestamp":"2019-08-02T20:20:14","uri":"Test-org/TestDataUpload-Regular/4","Count":{"Passed":1204,"Failed":0,"Skipped":2}},{"Name":"VS Test result","TestRunName":"5","Time":204.15,"Timestamp":"2019-08-02T20:20:14","uri":"Test-org/TestDataUpload-Regular/5","Count":{"Passed":1506,"Failed":0,"Skipped":2}},{"Name":"VS Test result","TestRunName":"6","Time":204.15,"Timestamp":"2019-08-02T20:20:14","uri":"Test-org/TestDataUpload-Regular/6","Count":{"Passed":1906,"Failed":1,"Skipped":2}},{"Name":"VS Test result","TestRunName":"7","Time":204.15,"Timestamp":"2019-08-02T20:20:14","uri":"Test-org/TestDataUpload-Regular/7","Count":{"Passed":2110,"Failed":1,"Skipped":3}},{"Name":"VS Test result","TestRunName":"8","Time":204.15,"Timestamp":"2019-08-02T20:20:14","uri":"Test-org/TestDataUpload-Regular/8","Count":{"Passed":2400,"Failed":1,"Skipped":3}},{"Name":"VS Test result","TestRunName":"9","Time":204.15,"Timestamp":"2019-08-02T20:20:14","uri":"Test-org/TestDataUpload-Regular/9","Count":{"Passed":2700,"Failed":3,"Skipped":5}},{"Name":"VS Test result","TestRunName":"10","Time":204.15,"Timestamp":"2019-08-02T20:20:14","uri":"Test-org/TestDataUpload-Regular/10","Count":{"Passed":3000,"Failed":6,"Skipped":10}},{"Name":"VS Test result","TestRunName":"11","Time":204.15,"Timestamp":"2019-08-02T20:20:14","uri":"Test-org/TestDataUpload-Regular/11","Count":{"Passed":1906,"Failed":0,"Skipped":1}},{"Name":"VS Test result","TestRunName":"12","Time":204.15,"Timestamp":"2019-08-02T20:20:14","uri":"Test-org/TestDataUpload-Regular/12","Count":{"Passed":1906,"Failed":0,"Skipped":1}},{"Name":"VS Test result","TestRunName":"13","Time":204.15,"Timestamp":"2019-08-02T20:20:14","uri":"Test-org/TestDataUpload-Regular/13","Count":{"Passed":1906,"Failed":0,"Skipped":1}},{"Name":"VS Test result","TestRunName":"14","Time":204.15,"Timestamp":"2019-08-02T20:20:14","uri":"Test-org/TestDataUpload-Regular/14","Count":{"Passed":1906,"Failed":0,"Skipped":1}},{"Name":"VS Test result","TestRunName":"15","Time":204.15,"Timestamp":"2019-08-02T20:20:14","uri":"Test-org/TestDataUpload-Regular/15","Count":{"Passed":1906,"Failed":0,"Skipped":1}},{"Name":"VS Test result","TestRunName":"16","Time":204.15,"Timestamp":"2019-08-02T20:20:14","uri":"Test-org/TestDataUpload-Regular/16","Count":{"Passed":1906,"Failed":0,"Skipped":1}},{"Name":"VS Test result","TestRunName":"17","Time":204.15,"Timestamp":"2019-08-02T20:20:14","uri":"Test-org/TestDataUpload-Regular/17","Count":{"Passed":1906,"Failed":0,"Skipped":1}},{"Name":"VS Test result","TestRunName":"18","Time":204.15,"Timestamp":"2019-08-02T20:20:14","uri":"Test-org/TestDataUpload-Regular/18","Count":{"Passed":1906,"Failed":0,"Skipped":1}},{"Name":"VS Test result","TestRunName":"19","Time":204.15,"Timestamp":"2019-08-02T20:20:14","uri":"Test-org/TestDataUpload-Regular/19","Count":{"Passed":1906,"Failed":0,"Skipped":1}},{"Name":"VS Test result","TestRunName":"20","Time":204.15,"Timestamp":"2019-08-02T20:20:14","uri":"Test-org/TestDataUpload-Regular/20","Count":{"Passed":1906,"Failed":0,"Skipped":1}},{"Name":"VS Test result","TestRunName":"21","Time":204.15,"Timestamp":"2019-08-02T20:20:14","uri":"Test-org/TestDataUpload-Regular/21","Count":{"Passed":1906,"Failed":80,"Skipped":100}},{"Name":"VS Test result","TestRunName":"22","Time":204.15,"Timestamp":"2019-08-02T20:20:14","uri":"Test-org/TestDataUpload-Regular/22","Count":{"Passed":1906,"Failed":20,"Skipped":1}}]}');

  constructor() { }

  public barChartOptions = {
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

    // Loading chart data sets into barChartData array
    this.jsonParsed.TestRuns.forEach(element => {
      this.barChartData[0].data.push(element.Count.Skipped);
      this.barChartData[1].data.push(element.Count.Failed);
      this.barChartData[2].data.push(element.Count.Passed);
      this.barChartLabels.push(element.Timestamp.split('T')[0]);
    });
  }


}
