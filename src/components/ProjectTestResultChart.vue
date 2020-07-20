<script>
import { Line } from 'vue-chartjs'

export default {
  data() {
    return {
      testResultsChartData: {},
      chartData: {
        labels: [],
        datasets: [
        // Skipped
        {
          backgroundColor: '#fbac54',
          borderColor: '#F98809',
          data: [],
          fill: true,
          pointBackgroundColor: '#F98809',
          pointBorderColor: '#fff',
          pointBorderWidth: 2,
          pointRadius: 5,
          pointHoverRadius: 5
        },
        // Failed
        {
          backgroundColor: '#FFA39D',
          borderColor: '#E63F34',
          data: [],
          fill: true,
          pointBackgroundColor: '#E63F34',
          pointBorderColor: '#fff',
          pointBorderWidth: 2,
          pointRadius: 5,
          pointHoverRadius: 5
        },
        // Passed
        {
          backgroundColor: '#B2DBBF',
          borderColor: '#24A44C',
          data: [],
          fill: true,
          pointBackgroundColor: '#24A44C',
          pointBorderColor: '#fff',
          pointBorderWidth: 2,
          pointRadius: 5,
          pointHoverRadius: 5
        }
        ]
      },
      chartOptions: {
        responsive: true,
        aspectRatio: 1,
        legend: {
          display: false
        },
        maintainAspectRatio: false,
        layout: {
          padding: {
            left: 0,
            right: 10,
            top: 0,
            bottom: 0
          }
        },
        title: {
          display: false
        },
        tooltips: {
          enabled: false
        },
        hover: {
          display: false
        },
        scales: {
          xAxes: [{
            display: true,
            scaleLabel: {
              display: false,
              fontColor: 'rgba(0,0,0,0.7)',
            },
            gridLines: {
              display: false
            },
            type: 'time',
            time: {
              parser: 'MM/DD/YYYY',
              tooltipFormat: 'll HH:mm',
              unit: 'day',
              unitStepSize: 1,
              displayFormats: {
                'day': 'MM/DD/YYYY'
              }
            }
          }],
          yAxes: [{
            ticks: {
              stepSize: 50
                    },

            display: true,
            scaleLabel: {
              display: false,
              fontColor: 'rgba(0,0,0,0.7)',
            },
            gridLines: {
              color: '#d7e1ea',
              borderDash: [5, 15],
              borderCapStyle: 'round',
              drawBorder: false,
              zeroLineColor: "#d7e1ea"
            }
          }]
        }
      }
    }
  },
  extends: Line,  
  methods: {    
    getTestResultsChartData() {
      return this.$http.get(`${this.$route.params.org}/projects/${this.$route.params.project}/testresults`)
        .then(response => {
          this.testResultsChartData = response.data
          this.chartData.labels = response.data.data.map(item => new Date(item.dateTime))
          
          this.chartData.datasets[2].data = response.data.data.map(item => item.passed)

          this.chartData.datasets[1].data = response.data.data.map(item => item.failed)

          this.chartData.datasets[0].data = response.data.data.map(item => item.skipped)
        })
    },
  },
  mounted () {
    this.getTestResultsChartData().then(() => {
      this.renderChart(this.chartData, this.chartOptions)
    })
  }
}

</script>

<style>
</style>
