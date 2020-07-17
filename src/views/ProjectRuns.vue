<template>
  <div>
    <div class="breadcrumbs-block">
      <ul>
        <li><a 
          @click.prevent="$router.push({ name: 'org-home', params: { org: $route.params.org }})" 
          >{{ $route.params.org }}</a></li>
        <li><span>{{ $route.params.project }}</span></li>
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
            <ProjectTestResultChart/>
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
            <ProjectCoverageChart/>
          </div>
        </div>
      </div>
    </div>

    <div class="dashboard-block__table-wrapper">
      <div class="filter-block">
        <div class="row align-items-center">
          <div class="col-12 col-md-auto col-lg-3">
            <div class="filter-block__search-block">
              <input type="text" class="form-control" placeholder="Search by name" v-model="searchString">
              <button type="submit" title="Search" class="search-button"><i class="icon-search"></i></button>
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
          <tr v-for="(test, index) in filteredTestResults" :key="index">
            <td>
              <div class="mobile-label">Test run</div>
              <div class="val test-name" @click.prevent="gotoRun(test)"><b>#{{ test.name }}</b></div>
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
              <div class="val">{{ Math.round((test.coverage + Number.EPSILON) * 100) / 100 }}</div>
            </td>

            <td>
              <div class="mobile-label">Time</div>
              <div class="val">{{ Math.round((test.time + Number.EPSILON) * 100) / 100  }}</div>
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
        data: function () {
            return {
                searchString: null,
                filteredTestResults: [],
                testResults: [],
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
        watch: {
          searchString() {
            if (this.searchString) {
              this.filteredTestResults = this.testResults.filter(test => test.name.includes(this.searchString))
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
            userOrg() {
                return this.$route.params.org ? this.$route.params.org : this.user.org
            },
        },
        methods: {
            loadTestRuns(page = 1) {
                var self = this
                this.$http.get(this.$route.params.org + "/projects/" + this.$route.params.project + "/runs?page=" + page)
                    .then((response) => {
                        self.testResults = self.filteredTestResults = response.data.data
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
            gotoRun(test) {
                // console.log(test)
                const project = this.$route.params.project
                const runId = test.name.toString().trim()
                // console.log(runId)
                this.$router.push({name: 'test-run', params: {org: this.userOrg, project: project, run: runId}})
            },
            getTestResult(result) {
                switch (result.toString()) {
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
                switch (result.toString()) {
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

<style lang="scss">
  .test-name{
    cursor: pointer;
  }
</style>
