import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-organization',
  templateUrl: './organization.component.html',
  styleUrls: ['./organization.component.scss']
})
export class OrganizationComponent implements OnInit {

  // This var should receive parsed organization JSON data from back-end API
  public jsonParsed = JSON.parse('{"Name": "Test-org","uri": "Test-org","Projects": [  {    "Name": "TestDataUpload-HugeReport",   "Status": {"TestRunsCount": 1,"RecentTestRun": "2018-12-04T04:02:21"    },    "uri": "Test-org/TestDataUpload-HugeReport"  },  {    "Name": "TestDataUpload-Regular",    "Status": {"TestRunsCount": 20,"RecentTestRun": "2019-08-02T20:20:14"    },    "uri": "Test-org/TestDataUpload-Regular"  },  {    "Name": "TestDataUpload-SmallJUnit",    "Status": {"TestRunsCount": 2,"RecentTestRun": "0001-01-01T00:00:00"    },    "uri": "Test-org/TestDataUpload-SmallJUnit"  }]    }');

  constructor() { }

  ngOnInit() {

  }
}
