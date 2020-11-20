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
    <b-tabs>
      <!-- Overview Tab -->
      <b-tab title="Overview" @click.prevent="gotoProjectRuns"></b-tab>


      <!-- Test Grid Tab -->
      <b-tab title="Test Grid" active>
        <div class="filter-block__search-block mt-3">
          <input type="text" class="form-control" placeholder="Search by name" v-model="searchString">
          <button type="submit" title="Search" class="search-button"><i class="icon-search"></i></button>
        </div>

        <div class="testrun-block__table-wrapper mt-3" v-if="filteredTestResults.length">
          
          
          <div v-for="group in filteredTestResults" :key="group.className" class="groupedClass">
          
            <div class="testrun-block__table-title" v-if="Object.keys(group.testCases).length">{{ group.className }}</div>

            <div class="testrun-block__table" v-if="Object.keys(group.testCases).length">

              <div v-for="(test, index) in Object.keys(group.testCases)" :key="index" class="testrun-block__table-item">
                <div class="testrun-block__table-row">
                  <div class="testrun-block__table-main-td"> {{ test }} </div>
                  
                  <div class="testrun-block__table-results-td">
                    <div class="dashboard-block__results-cells" v-if="group.testCases[test].length">
                      <TestHistoryCell 
                        v-for="(result, resultIndex) in group.testCases[test].slice(0, 40)"
                        :testResult="result"
                        :key="resultIndex">
                      </TestHistoryCell>
                    </div>
                  </div>

                </div>
              </div>
            </div>
          </div>
        </div>
      </b-tab>
    </b-tabs>
  </div>
</template>
<script>
  import TestHistoryCell from '../components/TestHistoryCell'

  export default {
    components: { TestHistoryCell },
    data() {
      return {
        searchString: null,
        testClasses: [],
        testRuns: [],
        mappedTests: [],
        filteredTestResults: []
      }
    },
    computed: { },
    watch: {
      searchString() {  
        if (this.searchString) {
          const filter = test => test.includes(this.searchString)

          const groups = this.mappedTests.filter(group =>  
            Object.keys(group.testCases).some(filter))

          const filtered = groups.map(grp => {
            const matches = Object.keys(grp.testCases).filter(filter)

            const filteredCases = {}
            matches.forEach(key => { filteredCases[key] = grp.testCases[key] })

            grp.testCases = filteredCases
            return grp
          })
          this.filteredTestResults = filtered
        } else {              
          this.filteredTestResults = this.mapTestsToClasses(this.testClasses, this.testRuns)
        }
      }
    },
    methods: {
      mapTestsToClasses(testClasses, testRuns) {
        const grouped = testClasses.map(group => { 
          const result = { className: group.className, testCases: {} };

          group.test.forEach(testName => { result.testCases[testName] = [] });

          return result;
        });

        testRuns.forEach(run => {
          run.testCases && run.testCases.forEach(testCase => {
            const [group] = grouped.filter(group => 
              Object.keys(group.testCases).includes(testCase.name));

            group.testCases[testCase.name].push(
              { ...testCase, uri: run.uri, testRunName: run.testRun }
            )
          })
        });

        return grouped;
      },

      gotoProjectRuns() {
        const project = this.$route.params.project
        const org = this.$route.params.org

        this.$router.push({
          name: 'project-test-runs', 
          params: { org, project }
        })
      },
      
      async fetchTests() {       
        const url = `${this.$route.params.org}/projects/${this.$route.params.project}/tests`
        
        const testsData = await this.$http.get(url)                 

        const { data, tests } = testsData.data
        
        this.testRuns = data
        this.testClasses = tests

        this.mappedTests = this.filteredTestResults = this.mapTestsToClasses(tests, data)
      }
    },
    mounted() {
      this.fetchTests()
    }
  }
</script>
<style scoped>
  .groupedClass {
    margin-top: 20px !important;
  }

  .testrun-block__table-main-td {
    width: 400px;
  }
 
  .testrun-block__table-row {
    pointer-events: none
  }

  .testrun-block__table-results-td {
    pointer-events: auto
  }

  .testrun-block__table-row:hover {
    background-color: rgba(0,56,255, 0.1);
  } 
</style>
