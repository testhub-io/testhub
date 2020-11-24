
<script>
  export default {
    props: { 
      testResult: { type: Object },
      testRun: { type: Object }, 
      matches: { type: Array }
    },
    data() {
      return {
        result: null
      }
    },
    computed: {
      testResultStatus() {
        if(!this.result) return ''
        if(this.result.status === null) return 'result-cell missing'

        return this.result.status === 1 ? 
          "result-cell good" : "result-cell bad"
      },
      tooltipHTML() {
        if(!this.result) return ''
        const date = this.getDateTime();
        const dateString = date ? `Date: ${date}` : 'Date: N/A';
        const tooltipData = {
          title: `<span style="white-space: nowrap;">Test Run: ${this.result.testRunName} <br /> ${dateString}</span>`
        };
        return tooltipData;
      }, 
    },
    methods: {
      gotoRun(event) {
        const project = this.$route.params.project
        const runId = this.result.testRunName.toString().trim()

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
        if(!this.result.timestamp) return

        let { timestamp } = this.result; 
 
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
      if(this.testRun) {
        const [match] = this.matches

        this.result =  { 
          status: match ? match.status : null,
          testRunName: this.testRun.testRun
        }
      } else { this.result = this.testResult }
    },
    render(createElement) {
      const self = this
      return createElement('div', {
          class: self.testResultStatus,
        }, [
          createElement('a', {
            attrs: {}
          })
        ])
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
