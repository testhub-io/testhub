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
      <b-tab title="Overview" @click.prevent="gotoProjectRuns"></b-tab>
      <b-tab title="TestGrid" active>

      </b-tab>
    </b-tabs>
  </div>
</template>
<script>
  //import TestHistoryCell from '../components/TestHistoryCell'

  export default {
    //components: { TestHistoryCell },
    data() {
      return {
        testGroups: [],
        testList: [],
        groupedTests: []
      }
    },
    methods: {
      gotoProjectRuns() {
        const project = this.$route.params.project
        const org = this.$route.params.org

        this.$router.push({
          name: 'project-test-runs', 
          params: { org, project }
        })
      },
      mapTestsToGroups() {
        /*
        const recursiveFilter = () => {

        };*/

        this.groupedTests = this.testGroups.map(group => {
          //const testRuns = {};
          group.test.forEach(testName => {
            console.group('Run', testName)
            
          })

          
        });     
      },
      async fetchTests() {       
        const url = `${this.$route.params.org}/projects/${this.$route.params.project}/tests`
        
        const testsData = await this.$http.get(url)                 
        console.log(testsData.data)

        const { data, tests } = testsData.data
        
        this.testGroups = data
        this.testList = tests

        //this.mapTestsToGroups()
      }
    },
    mounted() {
      this.fetchTests()
    }
  }
</script>
