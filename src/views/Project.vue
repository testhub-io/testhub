<template>
  <div>
    <div class="breadcrumbs-block">
      <ul>
        <li><a href="#">Wibbits</a></li>
        <li><span>Capsule</span></li>
      </ul>
    </div>

    <div class="row dashboard-block__topbar-row">
      <div class="col-12 col-xl-6">
        <div class="panel mb-3">
          <div class="form-row no-gutters justify-content-between align-items-center mb-3">
            <div class="h5 mb-0">Tests results</div>

            <div class="dashboard-block__num smaller-version"></div>
          </div>

          <div class="dashboard-block__chart-element">
            <ProjectTestResultChart />
          </div>

          <div class="dashboard-block__legend-labels dashboard-block__tests-results-legend">
            <div class="dashboard-block__legend-label-item">
              <div class="item-color" style="background-color: #24A44C;"></div>
              <div class="item-caption">Passed</div>
            </div>

            <div class="dashboard-block__legend-label-item">
              <div class="item-color" style="background-color: #E63F34;"></div>
              <div class="item-caption">Failed</div>
            </div>

            <div class="dashboard-block__legend-label-item">
              <div class="item-color" style="background-color: #F98809;"></div>
              <div class="item-caption">Skipped</div>
            </div>
          </div>
        </div>
      </div>

      <div class="col-12 col-xl-6">
        <div class="panel mb-3">
          <div class="form-row no-gutters justify-content-between align-items-center mb-3">
            <div class="h5 mb-0">Coverage growth</div>

            <div class="dashboard-block__num smaller-version">82%</div>
          </div>

          <div class="dashboard-block__chart-element">
            <ProjectCoverageChart />
          </div>
        </div>
      </div>
    </div>

    <div class="dashboard-block__table-wrapper">
      <div class="filter-block">
        <div class="row align-items-center">
          <div class="col-12 col-md-auto col-lg-3">
            <div class="filter-block__search-block">
              <input type="text" class="form-control" placeholder="Search by name">
              <button type="submit" title="Search" class="search-button"><i class="icon-search"></i></button>
            </div>
          </div>

          <div class="col-auto">
            <div class="filter-block__filter-list-block">
              <a href="javascript:;" class="filter-block__filter-list-toggle">
                <i class="icon-filter"></i>
                <span>Filter list</span>
              </a>
            </div>
          </div>

          <div class="col-auto ml-auto">
            <div class="filter-block__show-quant">
              <label>Rows per page:</label>

              <select class="form-control">
                <option value="5" selected>5</option>
                <option value="10">10</option>
                <option value="15">15</option>
                <option value="25">25</option>
                <option value="50">50</option>
                <option value="100">100</option>
              </select>
            </div>
          </div>
        </div>
      </div>

      <div class="dashboard-block__table">
        <table class="table">
          <thead>
            <tr>
              <th>Test run</th>
              <th>Branch</th>
              <th>Timestamp</th>
              <th>Test run result</th>
              <th>Tests qty</th>
              <th>Coverage</th>
              <th>Time</th>
            </tr>
          </thead>

          <tbody>
            <tr v-for="test in testResults" :key="test.id">
              <td>
                <div class="mobile-label">Test run</div>
                <div class="val"><b>#{{ test.build_number }}</b></div>
              </td>

              <td>
                <div class="mobile-label">Branch</div>
                <div class="val">{{ test.branch }}</div>
              </td>

              <td>
                <div class="mobile-label">Timestamp</div>
                <div class="val">{{ test.created_at }}</div>
              </td>

              <td>
                <div class="mobile-label">Test run result</div>
                <div class="val">
                  <div class="dashboard-block__legend-label-item">
                    <div class="item-color" :style="getTestResultStyle(test.result)"></div>
                    <div class="item-caption">{{ test.result }}</div>
                  </div>
                </div>
              </td>

              <td>
                <div class="mobile-label">Tests qty</div>
                <div class="val">{{ test.qty }}</div>
              </td>

              <td>
                <div class="mobile-label">Coverage</div>
                <div class="val">{{ test.coverage }}</div>
              </td>

              <td>
                <div class="mobile-label">Time</div>
                <div class="val">{{ test.duration }}</div>
              </td>
            </tr>

          </tbody>
        </table>
      </div>

      <div class="pagination-block">
        <ul>
          <li class="arrow"><a href="#"><i class="icon-union"></i></a></li>
          <li class="arrow"><a href="#"><i class="icon-union-single"></i></a></li>

          <li class="active"><a href="#">1</a></li>
          <li><a href="#">2</a></li>
          <li><a href="#">3</a></li>
          <li><a href="#">4</a></li>

          <li class="arrow next"><a href="#"><i class="icon-union-single"></i></a></li>
          <li class="arrow next"><a href="#"><i class="icon-union"></i></a></li>
        </ul>
      </div>
    </div>

  </div>
</template>

<script>
import ProjectCoverageChart from '../components/ProjectCoverageChart'
import ProjectTestResultChart from '../components/ProjectTestResultChart'
export default {
  data: function() {
    return {
      testResults: [
        {
          id: 6,
          branch: "master",
          build_number: "2045634",
          created_at: "1:58, 2 May 2020",
          result: "Skipped",
          qty: "159",
          coverage: "82%",
          duration: "0.2"
        },
        {
          id: 5,
          branch: "feature/test-result",
          build_number: "2355634",
          created_at: "12:08, 2 May 2020",
          result: "Success",
          qty: "159",
          coverage: "80%",
          duration: "0.2"
        },
        {
          id: 4,
          branch: "develop",
          build_number: "2145674",
          created_at: "11:48, 2 May 2020",
          result: "Fail",
          qty: "159",
          coverage: "78%",
          duration: "0.3"
        },
        {
          id: 3,
          branch: "feature/fix-code",
          build_number: "2248634",
          created_at: "10:45, 2 May 2020",
          result: "Fail",
          qty: "159",
          coverage: "76%",
          duration: "0.1"
        },
        {
          id: 2,
          branch: "feature/fix-tests",
          build_number: "2645434",
          created_at: "9:15, 2 May 2020",
          result: "Fail",
          qty: "159",
          coverage: "72%",
          duration: "0.4"
        },
      ]
    }
  },
  components: {
    ProjectCoverageChart,
    ProjectTestResultChart
  },
  methods: {
    getTestResultStyle(result) {
      var style = ""
      switch(result) {
        case "Skipped":
          style = "background-color: #F98809;"
          break;
        case "Success":
          style = "background-color: #24A44C;"
          break;
        case "Fail":
          style = "background-color: #E63F34;"
          break;
        default:
          style = "background-color: #E63F34;"
      }
      return style
    }
  },
  mounted() {

  }
}
</script>
