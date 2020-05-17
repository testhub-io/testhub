<script>
import { Line } from 'vue-chartjs'

export default {
  data() {
    return {
      chartData: {
        labels: ['1.01', '5.01', '10.01', '15.01', '20.01', '25.01', '31.01'],
        datasets: [
        {
          backgroundColor: '#fbac54',
          borderColor: '#F98809',
          data: [
            0,
            0,
            3,
            4,
            0,
            1,
            2
          ],
          fill: true,
          pointBackgroundColor: '#F98809',
          pointBorderColor: '#fff',
          pointBorderWidth: 2,
          pointRadius: 5,
          pointHoverRadius: 5
        },
        {
          backgroundColor: '#FFA39D',
          borderColor: '#E63F34',
          data: [
            26,
            30,
            35,
            39,
            42,
            55,
            50
          ],
          fill: true,
          pointBackgroundColor: '#E63F34',
          pointBorderColor: '#fff',
          pointBorderWidth: 2,
          pointRadius: 5,
          pointHoverRadius: 5
        },
        {
          backgroundColor: '#B2DBBF',
          borderColor: '#24A44C',
          data: [
            100,
            122,
            134,
            140,
            160,
            160,
            180
          ],
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
  mounted () {
    this.renderChart(this.chartData, this.chartOptions)
  }
}
</script>

<style>
</style>