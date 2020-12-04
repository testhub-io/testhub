<script>
import {Bar} from 'vue-chartjs'
import moment from 'moment'
export default {
  data() {
    return {
      chartData: {
        labels: [],
        datasets: [{
          label: 'Test Count',
          backgroundColor: '#8da4fa',
          borderColor: '#0038FF',
          data: [
          ],
          fill: true,
          pointBackgroundColor: '#0038FF',
          pointBorderColor: '#fff',
          pointBorderWidth: 2,
          pointRadius: 5,
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
          enabled: true,
          title: [""],
          titleFontSize: 0,
          callbacks: {
            label: function (tooltipItem) {
              return " Date: " + tooltipItem.xLabel + " | Tests Count: "  + tooltipItem.yLabel
            },
          }
        },
        hover: {
          display: true
        },
        scales: {
          xAxes: [{
            display: true,
            scaleLabel: {
              display: true,
              fontColor: 'rgba(0,0,0,0.7)',
            },
            gridLines: {
              display: false
            }
          }],
          yAxes: [{
            ticks: {
              // Include a percent sign in the ticks
              callback: function (value) {
                return value;
              },
              min: 0,

              // forces step size to be 5 units
              stepSize: 25 // <----- This prop sets the stepSize
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
  computed: {
    user() {
      return this.$store.getters.currentUser
    }
  },
  extends: Bar,
  mounted() {
    var self = this
    this.$http.get(this.$route.params.org + '/testresults')
        .then((response) => {
          var data = response.data.data
          for(var i=0; i<data.length; i++) {
            var dateLabel = moment(data[i].dateTime).format("M-D")
            self.chartData.labels.push(dateLabel)
            self.chartData.datasets[0].data.push(data[i].testsCount)
          }
          self.renderChart(this.chartData, this.chartOptions)
        })

  },
  created() {

  },
  destroyed() {
  }
}
</script>

<style>
</style>