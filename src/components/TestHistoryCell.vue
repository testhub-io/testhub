<template>
  <div 
    v-b-tooltip.hover.html="tooltipHTML"
    :class="testResultStatus">
    <a @click.prevent="gotoRun()"></a>
  </div>
</template>
<script>
  export default {
    props: { testResult: { type: Object, required: true } },
    computed: {
      testResultStatus() {
        return this.testResult.status === 1 ? 
          "result-cell good" : "result-cell bad"
      },
      tooltipHTML() {
        const date = this.getDateTime();
        console.log("date", date);
        const dateString = date ? `Date: ${date}` : '';
        const tooltipData = {
          title: `<span style="white-space: nowrap;">Test Run: ${this.testResult.testRunName} <br /> ${dateString}</span>`
        };
        return tooltipData;
      }
    },
    methods: {
      gotoRun() {
        console.log("hit")
        const project = this.$route.params.project
        const runId = this.testResult.testRunName.toString().trim();

        this.$router.push({
          name: 'test-run', 
          params: { org: this.$route.params.org, project: project, run: runId }
        })
      },
      //getTooltipHTML() {},
      getDateTime() {
        let { timestamp } = this.testResult; 

        timestamp = new Date(timestamp);

        const date = timestamp.getDate().toString().padStart(2, "0");
        const month = timestamp.getMonth().toString().padStart(2, "0"); 
        const year = timestamp.getFullYear().toString().padStart(4, "0");
        const time = `${timestamp.getHours()}:${timestamp.getMinutes()}`;

        const dateString = `${year}-${month}-${date} ${time}`;

        return month !== "00" ? dateString : null;
      },
    },
    mounted() {
     // console.log(this.$route.params)
    } 
  }
</script>
<style scoped>

  div > a {
    cursor: pointer;
    display: block;
    height: 100%;
    width: 100%;
  }
</style>
