<template>
  <div>
    <div class="breadcrumbs-block">
      <ul>
        <li><a href="#">{{ this.$route.params.org }}</a></li>
        <li><a href="#">{{ this.$route.params.project }}</a></li>
        <li><span>#{{ this.$route.params.run }}</span></li>
      </ul>
    </div>

    <div class="panel mb-3">
      <div class="testrun-block__numbers-block">
        <div class="testrun-block__number-item">
          <div style="color: #24A44C;" class="testrun-block__num-elem">{{ passed }}</div>
          <div class="testrun-block__num-label">Passed</div>
        </div>

        <div class="testrun-block__number-item">
          <div style="color: #E63F34;" class="testrun-block__num-elem">{{ failed }}</div>
          <div class="testrun-block__num-label">Failed</div>
        </div>

        <div class="testrun-block__number-item">
          <div style="color: #F98809;" class="testrun-block__num-elem">{{ skipped }}</div>
          <div class="testrun-block__num-label">Skipped</div>
        </div>

        <div class="testrun-block__number-item">
          <div class="testrun-block__num-elem">{{ total }}</div>
          <div class="testrun-block__num-label">Total tests</div>
        </div>

        <div class="testrun-block__number-item">
          <div class="testrun-block__num-elem">{{ executionTime }}</div>
          <div class="testrun-block__num-label">Execution time</div>
        </div>

        <div class="testrun-block__number-item ml-auto">
          <div style="color: #24A44C;" class="testrun-block__num-elem">{{ coverage }}</div>
          <div class="testrun-block__num-label">Total coverage</div>
        </div>
      </div>
    </div>
    
    <b-tabs class="tab-content">
      <b-tab class="tab-pane active" title="Test Results" id="test_results_tab">
        <div class="filter-block">
          <div class="row align-items-center">
            <div class="col-12 col-md-4 col-lg-3">
              <div class="filter-block__search-block">
                <input type="text" class="form-control" placeholder="Search by name">
                <button type="submit" title="Search" class="search-button"><i class="icon-search"></i></button>
              </div>
            </div>

            <div class="col-12 col-md-8 col-lg-auto">
              <div class="filter-block__status-filter">
                <div class="label">Filter by status</div>

                <div class="filter-block__status-row">
                  <div class="filter-block__status-item passed">
                    <input type="checkbox" id="filter_status_passed">
                    <label for="filter_status_passed">Passed</label>
                  </div>

                  <div class="filter-block__status-item failed">
                    <input type="checkbox" id="filter_status_failed">
                    <label for="filter_status_failed">Failed</label>
                  </div>

                  <div class="filter-block__status-item skipped">
                    <input type="checkbox" id="filter_status_skipped">
                    <label for="filter_status_skipped">Skipped</label>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div class="testrun-block__table-wrapper">
          <div class="testrun-block__table-title">E2eNode Suite</div>

          <div class="testrun-block__table" v-if="testRuns.tests !== undefined">

            <div v-for="(test, index) in testRuns.tests" :key="index" class="testrun-block__table-item">
              <div class="testrun-block__table-row">
                <div class="testrun-block__table-main-td" :style="parseInt(test.status) === 1 ? '' : 'color: #E63F34;'">{{ test.name }}
                </div>

                <div class="testrun-block__table-date-td">
                  <div class="testrun-block__date-time">
                    <i class="icon-clock"></i>
                    <span>{{ parseFloat(test.time).toFixed(2) }}</span>
                  </div>
                </div>

                <div class="testrun-block__table-results-td">
                  <div class="dashboard-block__results-cells">
                    <div v-for="(result, resultIndex) in test.recentResults" :key="resultIndex"
                         :class="getTestResultStatus(result)"></div>
                  </div>
                </div>
              </div>

              <div class="testrun-block__error-block" v-if="test.systemOut !== null && parseInt(test.status) !== 1">
                <p>{{ test.systemOut }}</p>
              </div>

              <div class="testrun-block__error-block" v-if="test.systemOut !== null && parseInt(test.status) === 1">
                <p>{{ test.systemOut }}</p>
              </div>

            </div>

          </div>
        </div>
      </b-tab>

      <b-tab class="tab-pane" title="Coverage" id="coverage_tab" >
        <CoverageTab v-if="baseRunUrl" :baseRunUrl="baseRunUrl"></CoverageTab>
      </b-tab>
    </b-tabs>
  </div>
</template>

<script>
  import CoverageTab from '../components/CoverageTab'

  export default {
    data: function () {
        return {
            testRuns: [],
            baseRunUrl: ''
        }
    },
    components: { CoverageTab },
    computed: {
      passed() {
          if (this.testRuns.tests === undefined) {
              return 0
          }
          return this.testRuns.tests.reduce((sum, { status }) => {
              if (parseInt(status) === 1) {
                  sum++
              }
              return sum
          }, 0)
      },
      failed() {
          if (this.testRuns.tests === undefined) {
              return 0
          }
          return this.testRuns.tests.reduce((sum, { status }) => {
              if (parseInt(status) === 0) {
                  sum++
              }
              return sum
          }, 0)
      },
      skipped() {
          if (this.testRuns.tests === undefined) {
              return 0
          }
          return this.testRuns.tests.reduce((sum, { status }) => {
              if (parseInt(status) === 2) {
                  sum++
              }
              return sum
          }, 0)
      },
      total() {
          if (this.testRuns.tests === undefined) {
              return 0
          }
          return this.testRuns.tests.length
      },
      executionTime() {
          if (this.testRuns.tests === undefined) {
              return 0
          }
          return this.testRuns.tests.reduce((sum, {time}) => {
              const execTime = (sum + parseFloat(time))
              return parseFloat(execTime).toFixed(2)
          }, 0)
      },
      coverage() {
          return 0
      }
    },
    methods: {
        load() {
            var self = this
            this.$http.get(`${this.baseRunUrl}tests/`)
                .then((response) => {
                    self.testRuns = response.data
                })
        },
        getTestResultStatus(result) {
            return result.status === 1 ? "result-cell good" : "result-cell bad"
        },
    },
    mounted() {
        this.baseRunUrl = `${this.$route.params.org}/projects/${this.$route.params.project}/runs/${this.$route.params.run}/`
        this.$store.dispatch('setBaseRunUrl', this.baseRunUrl)
        this.$store.dispatch('setProjectName', this.$route.params.project)
        this.load()
    }
  }
</script>
