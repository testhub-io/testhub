<template>
  <div>
    <div class="breadcrumbs-block">
      <ul>
          <li><a 
            @click.prevent="$router.push({ name: 'org-home', params: { org: $route.params.org }})">
            {{ $route.params.org }}</a></li>
          <li><a @click.prevent="gotoRuns">{{ this.$route.params.project }}</a></li>
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
      
      <b-tabs>
        <b-tab title="Test Results" active>
          <div class="filter-block">
            <div class="row align-items-center">

              <div class="col-12 col-md-8 col-lg-auto">
                <div class="filter-block__status-filter">
                  <div class="label">Filter by status</div>

                  <div class="filter-block__status-row">
                    <div class="filter-block__status-item passed">
                      <input type="checkbox" id="filter_status_passed" value="1" v-model="selectedFilters">
                      <label for="filter_status_passed">Passed</label>
                    </div>

                    <div class="filter-block__status-item failed">
                      <input type="checkbox" id="filter_status_failed" value="0" v-model="selectedFilters">
                      <label for="filter_status_failed">Failed</label>
                    </div>

                    <div class="filter-block__status-item skipped">
                      <input type="checkbox" id="filter_status_skipped" value="2" v-model="selectedFilters">
                      <label for="filter_status_skipped">Skipped</label>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <div class="testrun-block__table-wrapper">
            <div v-for="group in Object.keys(groupedTests)" :key="group" class="groupedClass">
            
              <div class="testrun-block__table-title" v-if="groupedTests[group].length">{{ group }}</div>

              <div class="testrun-block__table" v-if="groupedTests[group].length">

                <div v-for="(test, index) in groupedTests[group]" :key="index" class="testrun-block__table-item">
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
                        <div v-for="(result, resultIndex) in test.recentResults"
                          v-b-tooltip.hover.html="getTooltipHTML(result)"
                          :key="resultIndex"
                          :class="getTestResultStatus(result)"
                          :id="`${result.testRunName}-${resultIndex}`"
                          :v-if="test.recentResults">
                          <a :href="gotoRun(result)"></a>
                        </div>
                        <span v-if="test.recentResults === null">New test</span>
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
          </div>
        </b-tab>

        <b-tab title="Coverage">
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
              testRuns: {},
              selectedFilters: [],
              groupedTests: {},
              baseRunUrl: ''
          }
      },
      watch: {
        selectedFilters() {
          if (this.selectedFilters.length === 0) {
            this.groupedTests = this.groupTestsByClass(this.testRuns.tests)
          } else {
            const filteredTests = [...this.testRuns.tests].filter(test => {
              return this.selectedFilters.some(key => test.status === parseInt(key))
            });
            this.groupedTests = this.groupTestsByClass(filteredTests);
        }
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
        gotoRuns() {
          this.$router.push({
            name: 'project-test-runs', 
            params: {
              org: this.$route.params.org, 
              project: this.$route.params.project
            }
          })
        },
        load() {
            var self = this
            this.$http.get(`${this.baseRunUrl}tests/`)
                .then((response) => {
                    self.testRuns = response.data
                    self.groupedTests = self.groupTestsByClass(response.data.tests);
                })
        },
        groupTestsByClass(tests) {
          return tests.reduce((accumulator, currentTest) => {
            const group = currentTest.className;
    
            accumulator[group] = accumulator[group] || [];
    
            accumulator[group].push(currentTest);
    
            return accumulator; 
          }, {}); 
        },
        getTestResultStatus(result) {
            return result.status === 1 ? "result-cell good" : "result-cell bad"
        },
        gotoRun(result) {
          const project = this.$route.params.project
          const runId = result.testRunName.toString().trim();
          
          return this.$router.resolve({
            name: 'test-run', 
            params: { org: this.$route.params.org, project: project, run: runId }
          }).href
        },
        getTooltipHTML(test) {
          const date = this.getDateTime(test);
          const dateString = date ? `Date: ${date}` : '';
          const tooltipData = {
            title: `<span style="white-space: nowrap;">Test Run: ${test.testRunName} <br /> ${dateString}</span>`
          };
          return tooltipData;
        },
        getDateTime(result) {
          let { timestamp } = result; 
          timestamp = new Date(timestamp);
          const date = timestamp.getDate().toString().padStart(2, "0");
          const month = timestamp.getMonth().toString().padStart(2, "0"); 
          const year = timestamp.getFullYear().toString().padStart(4, "0");
          const time = `${timestamp.getHours()}:${timestamp.getMinutes()}`;
          const dateString = `${year}-${month}-${date} ${time}`;
          return month !== "00" ? dateString : null;
        }
    },
    mounted() {
        this.baseRunUrl = `${this.$route.params.org}/projects/${this.$route.params.project}/runs/${this.$route.params.run}/`
        this.$store.dispatch('setBaseRunUrl', this.baseRunUrl)
        this.$store.dispatch('setProjectName', this.$route.params.project)
        this.load()
    }
  }
</script>

<style lang="scss" scoped>
  a {
    cursor: pointer;
  }
  .dashboard-block__results-cells > div > a {
    display: block;
    height: 100%;
    width: 100%;
  }
  .groupedClass {
    margin-top: 20px !important;
  }
</style>
