<script>
import { Line } from 'vue-chartjs'

export default {
  data() {
    return {
      chartData: {
        labels: ['1.01', '5.01', '10.01', '15.01', '20.01', '25.01', '31.01'],
        datasets: [{
          label: 'Date',
          backgroundColor: '#C8D4FF',
          borderColor: '#0038FF',
          data: [
            15,
            30,
            22,
            35,
            30,
            50,
            65
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
            }
          }],
          yAxes: [{
            ticks: {
              // Include a percent sign in the ticks
              callback: function(value) {
                return value + '%';
              },
              min: 0,
              max: 100,

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
  extends: Line,
  mounted () {
    this.renderChart(this.chartData, this.chartOptions)
  }
}
</script>

<style>
</style>