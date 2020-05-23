<script>
    import {Line} from 'vue-chartjs'
    import moment from 'moment'
    export default {
        data() {
            return {
                chartData: {
                    labels: [],
                    datasets: [{
                        label: 'Date',
                        backgroundColor: '#C8D4FF',
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
                                callback: function (value) {
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
        computed: {
            user() {
                return this.$store.getters.currentUser
            }
        },
        extends: Line,
        mounted() {
            var self = this
            this.$http.get(this.user.org + '/coverage')
                .then((response) => {
                    var data = response.data
                    for(var i=0; i<data.length; i++) {
                        var dateLabel = moment(data[i].dateTime).format("M-D")
                        self.chartData.labels.push(dateLabel)
                        self.chartData.datasets[0].data.push(data[i].coverage * 100)
                    }
                    // self.chartOptions.scales.yAxes[0].ticks.max = Math.max(self.chartData.datasets[0].data)
                    self.renderChart(this.chartData, this.chartOptions)
                })

        }
    }
</script>

<style>
</style>