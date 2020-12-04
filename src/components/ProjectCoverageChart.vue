<script>
import { Line } from 'vue-chartjs'

export default {
  data() {
    
    return {
      coverage: {},
      chartData: {
        labels: [],
        datasets: [{
          label: 'Date',
          backgroundColor: '#C8D4FF',
          borderColor: '#0038FF',
          data: [],
          fill: true,
          pointBackgroundColor: '#0038FF',
          pointBorderColor: '#fff',
          pointBorderWidth: 1,
          pointRadius: 3,
          pointHoverRadius: 5
        }]
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
            distribution: 'series', 
            type: 'time',
            time: {
              parser: 'MM/DD/YYYY HH:mm',
              tooltipFormat: 'll HH:mm',
              unit: 'day',
              unitStepSize: 1,
              displayFormats: {
                'day': 'MM-DD'
              }
            }
          }],
          yAxes: [{
            ticks: {
              // Include a percent sign in the ticks
              callback: function(value) {
                return value + '%';
              },
              source: 'data',
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
            },
          }]
        }
      }
    }
  },
  extends: Line,
  methods: {
      
    getCoverageChartData() {
      return this.$http.get(`${this.$route.params.org}/projects/${this.$route.params.project}/coverage`)
        .then(response => {
          this.coverageChartData = response.data
          this.chartData.labels = response.data.items.map(item => new Date(item.dateTime))
          this.chartData.datasets[0].data = response.data.items.map(item => item.coverage)
        })
    },
  },
  mounted () {
    this.getCoverageChartData().then(() => {
      this.renderChart(this.chartData, this.chartOptions)
    })
  }
}
</script>

<style>
</style>
