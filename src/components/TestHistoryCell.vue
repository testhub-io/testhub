<script>
  export default {
    props: { 
      testResult: { type: Object },
      testRun: { type: Object }, 
      matches: { type: Array },
      id: { type: String },
      isFirstRow: { type: Boolean }
    },
    data() {
      return {
        result: {}
      }
    },
    methods: {
      testResultStatus() {
        if(!this.result) return ''
        if(this.result.status === null) return 'result-cell missing'

        return this.result.status === 1 ? 
          "result-cell good" : "result-cell bad"
      },

      getRunUrl() {
        if(!this.result.testRunName) return
        const project = this.$route.params.project
        const runId = this.result.testRunName.toString().trim()

        const options = {
          name: 'test-run', 
          params: { org: this.$route.params.org, project: project, run: runId }
        }

        return this.$router.resolve(options).href
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
      const date = this.getDateTime()
      const runName = this.result.testRunName ? this.result.testRunName : ''

      const displayTooltip = this.isFirstRow ? 
        (
          createElement('b-tooltip', {
            attrs: {
              trigger: 'hover',
              target: self.id
            }
          }, [
            createElement('span', {
              attrs: { style: 'white-space: pre-wrap;' }
            }, `Test Run: ${runName}\nDate: ${date ? date : '' }`)
          ]) 
        ) : null

      const element = createElement('div', {
        attrs: { class: 'dashboard-block__results-column' }
      }, [
        createElement('div', {
          class: self.testResultStatus(),
          attrs: {
            id: self.id
          }
        }, [
          createElement('a', {
            attrs: {
              href: self.getRunUrl()
            }
          }),
        ]),
        displayTooltip
      ])

      return element
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
    min-width: 16px;
  } 

  .dashboard-block__results-column {
    padding: 20% 2px;
  }
 
 </style>
