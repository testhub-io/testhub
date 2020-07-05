<template>
  <b-container id="file-view">
    <b-container fluid :style="{ padding: '20px' }">
      <b-link @click="goBack">Back to file tree</b-link>
    </b-container>
    <b-container fluid v-if="loading === false">
      <h4 class="text-left">{{ node.name }}</h4>
      <table>
        <tbody>
          <tr
            v-for="line in codeLines.slice(1)"
            :key="line.number"
            :class="`line ${getColorCode(line.number)}`"
          >
            <td class="line-num">{{ line.number }}</td>
            <div style="font-size:14px; height:10px; vertical-align:middle;">
              <td class="line-code"><pre><code>{{ line.line }}</code></pre></td>
            </div>
            <td class="line-code" v-html="line.hits"></td>
          </tr>
        </tbody>
      </table>
    </b-container>
    <b-container v-else>
      <b-jumbotron header="Loading...">
        <b-button variant="primary" @click="goBack">Go Back</b-button>
      </b-jumbotron>
    </b-container>
  </b-container>
</template>

<script>

export default {
  name: 'FileView',

  props: {},

  components: {},

  data: () => ({
    loading: true,
    codeLines: [],
    coveredLines: [],
    uncoveredLines: []
  }),

  computed: {
    node () { return this.$store.getters.codeFile },
    fileUrl () { return `${this.$store.getters.baseRunUrl}code/${this.node.name}` }
  },

  methods: {
    goBack() { 
      this.$store.dispatch('setCodeFile', null)
      this.$emit('changeView', 'FileTree') 
    },
    
    getSource() {
      return this.$http.get(this.fileUrl)
        .then(response => {
          return response.body.split('\n')
        })
    },

    async mapLineData() {
      const { linesData } = this.node.data

      const uncovered = linesData
        .filter(line => (parseInt(line.hits) === 0))

      const covered = linesData
        .filter(line => (parseInt(line.hits) > 0))

      this.uncoveredLines = uncovered
      this.coveredLines = covered

      const code = await this.getSource()
      if(code) this.loading = false
      code.unshift('')

      this.codeLines = code.map((line, index) => {
        const hit = covered.find(ln => parseInt(ln.number) === index)
        const notHit = uncovered.find(ln => parseInt(ln.number) === index)

        let html;
        if (hit) {
          html = `<span>${hit.hits}<sup>x</sup></span>`
        } else if(notHit) {
          html = '<span>0</span>'
        }

        return {
          number: index,
          line,
          hits: html
        }
      })
    },

    getColorCode(line) {
      const covered = this.coveredLines.find(ln => parseInt(ln.number) === line)
      const uncovered = this.uncoveredLines.find(ln => parseInt(ln.number) === line)

      if (covered){
        if(covered.branch === "True") {
          const percentage = parseInt(covered['condition-coverage'].split('%')[0])
          if(percentage < 50) {
            return 'uncovered'
          } else if(percentage >= 50 && percentage < 90) {
            return 'average'
          }
        } else return 'covered'
      }
      if (uncovered) return 'uncovered'

      return 'empty'
    }
  },

  async mounted() {
    await this.mapLineData()
  }
}
</script>

<style>
#file-view{
 margin-left: -10px
}
.line-num {
  border-right: solid 1px rgba(0,0,0,0.1);
  text-align: center;
}

.line-code {
}

td { font-size: 12px }

tr {
  height: 10px;
}
pre{
  height: 100%;
  font-size: 12px;
  /*padding-top: 10px;*/
  margin-bottom: 0 !important;
  display: block;
}
.empty {}
.covered {
  background-color: rgba(95, 151, 68, 0.2);
}
.uncovered {
  background-color: rgba(185, 73, 71, 0.2);
}
.average {
  background-color: rgba(255, 165, 0, 0.2);
}
</style>
