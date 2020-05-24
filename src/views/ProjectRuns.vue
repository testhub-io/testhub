<template>
  <div>
    <div class="breadcrumbs-block">
      <ul>
        <li><a @click.prevent="$router.push({ name: 'dashboard' })" href="#">{{ this.user.org }}</a></li>
        <li><span>{{ this.$route.params.project }}</span></li>
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
            <tr v-for="(test, index) in testResults" :key="index">
              <td>
                <div class="mobile-label">Test run</div>
                <div class="val"><b>#{{ test.name }}</b></div>
              </td>

              <td>
                <div class="mobile-label">Branch</div>
                <div class="val">{{ test.branch }}</div>
              </td>

              <td>
                <div class="mobile-label">Timestamp</div>
                <div class="val">{{ getTestRunTime(test.timeStemp) }}</div>
              </td>

              <td>
                <div class="mobile-label">Test run result</div>
                <div class="val">
                  <div class="dashboard-block__legend-label-item">
                    <div class="item-color" :style="getTestResultStyle(test.result)"></div>
                    <div class="item-caption">{{ getTestResult(test.result) }}</div>
                  </div>
                </div>
              </td>

              <td>
                <div class="mobile-label">Tests qty</div>
                <div class="val">{{ test.stats.totalCount }}</div>
              </td>

              <td>
                <div class="mobile-label">Coverage</div>
                <div class="val">{{ test.coverage }}</div>
              </td>

              <td>
                <div class="mobile-label">Time</div>
                <div class="val">{{ parseFloat(test.time).toFixed(2) }}</div>
              </td>
            </tr>

          </tbody>
        </table>
      </div>

      <Pagination
              :pagination="testResultsPagination"
              :callback="loadTestResultsPage"
      />

    </div>

  </div>
</template>

<script>
import ProjectCoverageChart from '../components/ProjectCoverageChart'
import ProjectTestResultChart from '../components/ProjectTestResultChart'
import Pagination from '../components/Pagination'
import moment from "moment";
export default {
  data: function() {
    return {
      testResults: [
      ],
      testResultsPagination: {
        "itemsCount": 1,
        "pageSize": 10,
        "currentPage": 1,
        "totalPages": 1,
        "links": {
          "next": null
        }
      }
    }
  },
  components: {
    ProjectCoverageChart,
    ProjectTestResultChart,
    Pagination
  },
  computed: {
    user() {
      return this.$store.getters.currentUser
    },
  },
  methods: {
    loadTestRuns(page = 1) {
      var self = this
      this.$http.get(this.user.org + "/projects/" + this.$route.params.project + "/runs?page=" + page)
              .then((response) => {
                self.testResults = response.data.data
                self.testResultsPagination = response.data.meta.pagination
              })
    },
    loadTestResultsPage(page) {
      this.loadTestRuns(page)
      this.$scrollTo(".dashboard-block__chart-element", 500, {})
    },
    getTestRunTime(timestamp) {
      return moment(timestamp).format("HH:mm, D MMM YYYY")
    },
    getTestResult(result) {
      switch(result.toString()) {
        case "-1":
          result = "Skipped"
          break;
        case "1":
          result = "Passed"
          break;
        case "0":
          result = "Failed"
          break;
        default:
          result = "Unknown"
      }
      return result
    },
    getTestResultStyle(result) {
      var style = ""
      switch(result.toString()) {
        case "-1":
          style = "background-color: #F98809;"
          break;
        case "1":
          style = "background-color: #24A44C;"
          break;
        case "0":
          style = "background-color: #E63F34;"
          break;
        default:
          style = "background-color: #E63F34;"
      }
      return style
    }
  },
  mounted() {
    this.loadTestRuns()
  }
}
</script>
