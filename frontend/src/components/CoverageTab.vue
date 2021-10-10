<template>
  <b-container fluid id="project-view">
    
    <b-alert show variant="danger">
        <h4 class="alert-heading mb-0">Coming Soon</h4>
        <hr/>
        <p>
          This functionality is not ready yet. We are working hard on making it available soon. Below is an example of how it will look like.
        </p> 
        </b-alert>
    

    <b-card
      class="mb-12"
      border-variant="light"
      v-if="projectSummary && activeView === 'FileTree'"
    >    
      <b-card-text >
        <b-row>
          <b-col>
          <b-row>
            <b-col sm="12" class="metric">
              <h4 :class="coverageColor">{{ coverage }}%</h4>
            </b-col>
            <b-col sm="12">
              <span class="metric-label">Coverage</span>
            </b-col>
          </b-row>
          </b-col>
          
          <b-col>
          <b-row>
            <b-col sm="12" class="metric">
              <h4 :class="branchCoverageColor">{{ branchCoverage }}%</h4>
            </b-col>
            <b-col sm="12">
              <span class="metric-label">Branch Coverage</span>
            </b-col>
          </b-row>
          </b-col>
          
          <b-col>
          <b-row>
            <b-col sm="12" class="metric">
              <h4 class="metric-neutral">{{ projectSummary['lines-valid'] }}</h4>
            </b-col>
            <b-col sm="12">
              <span class="metric-label">Lines</span>
            </b-col>
          </b-row>
          </b-col>

          <b-col>
          <b-row>
            <b-col sm="12" class="metric">
              <h4 class="metric-neutral">{{ projectSummary['lines-covered'] }}</h4>
            </b-col>
            <b-col sm="12">
              <span class="metric-label">Lines Covered</span>
            </b-col>
          </b-row>
          </b-col>

          <b-col>
          <b-row>
            <b-col sm="12" class="metric">
              <h4 class="metric-neutral">{{ projectSummary['branches-valid'] }}</h4>
            </b-col>
            <b-col sm="12">
              <span class="metric-label">Branches</span>
            </b-col>
          </b-row>
          </b-col>

          <b-col>
          <b-row>
            <b-col sm="12" class="metric">
              <h4 class="metric-neutral">{{ projectSummary['branches-covered'] }}</h4>
            </b-col>
            <b-col sm="12">
              <span class="metric-label">Branches Covered</span>
            </b-col>
          </b-row>
          </b-col>
        </b-row>
      </b-card-text>
    </b-card>
    <FileTree
      v-if="filesData && activeView === 'FileTree'"
      :filesData="filesData"
    ></FileTree>
    <FileView
      v-if="activeView === 'FileView'"
      :baseRunUrl="baseRunUrl"
    ></FileView>

  </b-container>
</template>

<script>
import FileTree from './FileTree'
import FileView from './FileView'

export default {
  name: 'CoverageTab',
  components: { FileTree, FileView },
  props: {
    baseRunUrl: { type: String, required: true }
  },

  data: () => ({
    projectName: '',
    coverageData: null,
    projectSummary: null,
    filesData: null,
  }),

  computed: {
    activeView() {
      return this.$store.getters.codeFile !== null ? 'FileView' : 'FileTree'
    },
    coverage() {
      return Math.round(
        (parseFloat(this.projectSummary['line-rate']) * 100))
    },

    coverageColor() { return this.coverageClass(this.coverage) },

    branchCoverage() {
      return Math.round(
        (parseFloat(this.projectSummary['branch-rate']) * 100))
    },

    branchCoverageColor() { return this.coverageClass(this.branchCoverage) }
  },

  methods: {
    coverageClass(coverage) {
      if(coverage >= 0 && coverage < 75) {
        return 'low'
      } else if (coverage > 75 && coverage < 85) {
        return 'medium'
      } else {
        return 'high'
      }
    },
    fetchXML(url) {
      this.$http.get(url)
        .then(response => { 
          this.getCoverageData(response.body)
        })
    },

    getProperties(data) {
      const createDict = element => Array.from(element.attributes)
        // attributes are returned as a 'NamedNodeMap' then we convert that to a dictionary
        .map(attribute => ({ [attribute.name]: attribute.value }))
        // Then we combine the key:value pairs into a single dictionary representing a file
        .reduce((accumulator, currentObj) => {
          Object.keys(currentObj)
            .forEach(key => { accumulator[key] = currentObj[key]; });
          return accumulator;
        }, {});

      let props;

      if(Array.isArray(data)) {
        props = data.map( elem => createDict(elem) )
      } else {
        props = createDict(data)
      }

      return props
    },
    

    getCoverageData(xml) {
      const parser = new DOMParser();
      const xmlDoc = parser.parseFromString(xml, "text/xml");

      const packages = xmlDoc.getElementsByTagName("package")
      this.projectName = xmlDoc.getElementsByTagName("source")[0].innerHTML

      const summary = xmlDoc.getElementsByTagName("coverage")
      this.projectSummary = this.getProperties(summary[0])


      let files = []

      Array.from(packages).forEach(pkg => {
        const packageData = { ...this.getProperties(pkg), isPackage: true }
        const packageClasses = pkg.children[0].children

        const classes = Array.from(packageClasses).map(cls => {
          const classProps = this.getProperties(cls)
          const lines = Array.from(cls.children[1].children)
          const linesData = lines.map(line => this.getProperties(line))

          classProps.linesData = linesData
          return classProps
        })

        files = [...files, packageData, ...classes]
      })

      this.filesData = files
    },
  },

  mounted() {
    const coverageUrl = `${this.baseRunUrl}coverage`
    this.fetchXML(coverageUrl)
  }
}
</script>
<style>
#project-view{
  margin-top: 10px
}

.metric {
  height: 30px;
}

.metric-label {
  font-size: 13px;
  color: #000;
}

.low{
  color: rgb(230, 63, 52);
}
.medium{
  color: rgb(249, 136, 9)
}
.high{
  color: rgb(36, 164, 76);
}
</style>
