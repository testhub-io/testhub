<template>
  <div  class="container">
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

        <div class="testrun-block__table-wrapper mt-3" v-if="testClasses.length">
           
          <div v-for="(group, groupIndex) in filteredClasses" :key="group.className" class="groupedClass">
          
            <div class="testrun-block__table-title" v-if="group.test.length">{{ group.className }}</div>

            <div class="testrun-block__table" v-if="group.test.length">

              <div v-for="(test, testIndex) in group.test" :key="testIndex" class="testrun-block__table-item">
                <div class="testrun-block__table-row">
                  <div class="testrun-block__table-main-td"> {{ test.name }} </div>
                  
                  <div class="testrun-block__table-results-td">
                    <div class="dashboard-block__results-cells">
                      <TestHistoryCell 
                        v-for="run in testRuns"
                        :matches="run.testCases.filter(cs => cs.id === test.id)"
                        :ref="run.testRun"
                        :testRun="run"
                        @mouseover.native="highlightColumn(run.testRun)"
                        @mouseleave.native="removeHighlight(run.testRun)"
                        :id="test.id + run.testRun"
                        :isFirstRow="groupIndex === 0 && testIndex === 0"
                        :key="test.id + run.testRun">
                      </TestHistoryCell>
                    </div>
                  </div>

                </div>
              </div>
            </div>
          </div>
        </div>

        <Loader v-else></Loader>
      </b-tab>
    </b-tabs>
  </div>
</template>
<script>
  import TestHistoryCell from '../components/TestHistoryCell'
  import Loader from '../components/Loader.vue'

  export default {
    components: { TestHistoryCell, Loader },
    data() {
      return {
        searchString: null,
        testClasses: [],
        filteredClasses: [],
        testRuns: []
      }
    },
    computed: { },
    watch: {
      searchString() {     
        if (this.searchString) {
          const filter = test => test.name.includes(this.searchString)
          const tests = [...this.testClasses]

          const groups = tests.filter(group => group.test.some(filter))

          this.filteredClasses = groups.map(grp => {
              const test = grp.test.filter(filter)
              return { ...grp, test }
            })
        } else {              
          this.filteredClasses = this.testClasses
        }
      }
    },
    methods: {
      highlightColumn(runId) {
        this.$refs[runId].forEach(cell => { cell.$el.classList.add('highlighted') })
      },

      removeHighlight(runId) {
        this.$refs[runId].forEach(cell => { cell.$el.classList.remove('highlighted') })
      },

      gotoProjectRuns() {
        const project = this.$route.params.project
        const org = this.$route.params.org

        this.$router.push({
          name: 'project-test-runs', 
          params: { org, project }
        })
      },
      
      fetchTests() {       
        const url = `${this.$route.params.org}/projects/${this.$route.params.project}/tests?runsLimit=42`
        
        this.$http.get(url) 
          .then(response => {
            const { data, tests } = response.data
        
            this.testRuns = Object.freeze(data)
            this.testClasses = this.filteredClasses = Object.freeze(tests)
          })                
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
  
  .highlighted {
    background-color: rgba(0,56,255, 0.2) !important;
  }

  .testrun-block__table-row {
    padding-top: 5px !important;
    padding-bottom: 5px !important;
  }

</style>
