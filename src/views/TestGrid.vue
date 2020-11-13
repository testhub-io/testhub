<template>
  <b-tabs>
    <b-tab title="Overview" @click.prevent="gotoProjectRuns"></b-tab>
    <b-tab title="TestGrid" active>
      <TestListGrouped
        :tests="groupedTests"
        :displayTime="false"
      ></TestListGrouped>
    </b-tab>
  </b-tabs>
</template>
<script>
  import TestListGrouped from '../components/TestListGrouped'

  export default {
    components: { TestListGrouped },
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
             
      },
      async fetchTests() {       
        const url = `${this.$route.params.org}/projects/${this.$route.params.project}/tests`
        
        const testsData = await this.$http.get(url)                 
console.log(testsData.data)

        const { data, tests } = testsData.data
        
        this.testGroups = data
        this.testList = tests

        //this.mapTestsToGroups()

        return []
      }
    },
    mounted() {
      console.log('Test Grid')
      this.tests = this.fetchTests()
    }
  }
</script>
