<template>
  <div 
    v-b-tooltip.hover.html="tooltipHTML"
    :class="testResultStatus">
    <a @click="gotoRun($event)"></a>
  </div>
</template>
<script>
  export default {
    props: { testResult: { type: Object, required: true } },
    computed: {
      testResultStatus() {
        if(!this.testResult.status && this.testResult.status !== 0) return 'result-cell missing'

        return this.testResult.status === 1 ? 
          "result-cell good" : "result-cell bad"
      },
      tooltipHTML() {
        const date = this.getDateTime();
        const dateString = date ? `Date: ${date}` : '';
        const tooltipData = {
          title: `<span style="white-space: nowrap;">Test Run: ${this.testResult.testRunName} <br /> ${dateString}</span>`
        };
        return tooltipData;
      }, 
    },
    methods: {
      gotoRun(event) {
        const project = this.$route.params.project
        const runId = this.testResult.testRunName.toString().trim()

        const options = {
          name: 'test-run', 
          params: { org: this.$route.params.org, project: project, run: runId }
        }

        if(event.ctrlKey) {
          const page = this.$router.resolve(options)
          window.open(page.href, "_blank");
         } else { this.$router.push(options) }
      }, 

      getDateTime() {
        let { timestamp } = this.testResult; 

        if(!this.testResult.timestamp) timestamp = "2020-10-18T08:43:02.186171"

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

  .result-cell {
    min-width: 16px
  }

 </style>
