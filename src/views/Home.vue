<template>
    <div>
        <h6>{{ user.org }}</h6>

        <div class="row dashboard-block__topbar-row">
            <div class="col-12 col-md-6 col-xl-3 order-1 order-xl-1">
                <div class="panel mb-3">
                    <div class="dashboard-block__numbers-block">
                        <div class="dashboard-block__number-item">
                            <div class="dashboard-block__num">{{ this.org.summary.projectsCount }}</div>
                            <div class="dashboard-block__number-caption">Current projects</div>
                        </div>

                        <div class="dashboard-block__number-item">
                            <div class="dashboard-block__num">{{ this.org.summary.avgTestsCount }}</div>
                            <div class="dashboard-block__number-caption">Average tests count</div>
                        </div>

                        <div class="dashboard-block__number-item">
                            <div class="dashboard-block__num">{{ this.org.summary.avgCoverage }}%</div>
                            <div class="dashboard-block__number-caption">Average coverage</div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-12 col-xl-6 order-3 order-xl-2">
                <div class="panel mb-3">
                    <div class="form-row no-gutters justify-content-between align-items-center mb-3">
                        <div class="h5 mb-0">Coverage growth</div>

                        <div class="dashboard-block__num smaller-version">{{ this.org.summary.avgCoverage }}%</div>
                    </div>

                    <div class="dashboard-block__chart-element">
                        <CoverageGrowthChart/>
                    </div>
                </div>
            </div>

            <div class="col-12 col-md-6 col-xl-3 order-2 order-xl-3">
                <div class="panel mb-3">
                    <div class="h5 mb-3">Failing projects</div>

                    <div class="dashboard-block__progress-pie">
                        <div class="circle-block" id="failing_projects_pie" :data-value="this.projectsInGreenPercentile"></div>
                        <div class="center-caption">
                            <div class="num">{{ this.projectsInGreenPercentile * 100 }}%</div>
                            <div class="caption">Success rate</div>
                        </div>
                    </div>

                    <div class="dashboard-block__legend-labels">
                        <div class="dashboard-block__legend-label-item">
                            <div class="item-color" style="background-color: #24A44C;"></div>
                            <div class="item-caption">Success</div>
                        </div>

                        <div class="dashboard-block__legend-label-item">
                            <div class="item-color" style="background-color: #E63F34;"></div>
                            <div class="item-caption">Failed tests</div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="dashboard-block__table-wrapper">
            <div class="filter-block">
                <div class="row align-items-center">
                    <div class="col-12 col-md-auto col-lg-3">
                        <div class="filter-block__search-block">
                            <input type="text" class="form-control" placeholder="Search by name">
                            <button type="submit" title="Search" class="search-button"><i class="icon-search"></i>
                            </button>
                        </div>
                    </div>

                    <div class="col-auto">
                        <div class="filter-block__filter-list-block">
                            <a href="javascript:;" class="filter-block__filter-list-toggle">
                                <i class="icon-filter"></i>
                                <span>Filter list</span>
                            </a>
                        </div>
                    </div>

                    <div class="col-auto ml-auto">
                        <div class="filter-block__show-quant">
                            <label>Rows per page:</label>

                            <select class="form-control">
                                <option value="5" selected>5</option>
                                <option value="10">10</option>
                                <option value="15">15</option>
                                <option value="25">25</option>
                                <option value="50">50</option>
                                <option value="100">100</option>
                            </select>
                        </div>
                    </div>
                </div>
            </div>

            <div class="dashboard-block__table">
                <table class="table">
                    <thead>
                    <tr>
                        <th>Project Name</th>
                        <th>Last results</th>
                        <th>Tests</th>
                        <th>Coverage</th>
                        <th>Frequency</th>
                        <th>Tests qty growth</th>
                        <th>Coverage growth</th>
                    </tr>
                    </thead>

                    <tbody>
                    <tr>
                        <td>
                            <div class="mobile-label">Project Name</div>
                            <div class="val"><b>Capsule</b></div>
                        </td>

                        <td>
                            <div class="mobile-label">Last results</div>
                            <div class="val">
                                <div class="dashboard-block__results-cells">
                                    <div class="result-cell good"></div>
                                    <div class="result-cell bad"></div>
                                    <div class="result-cell bad"></div>
                                    <div class="result-cell good"></div>
                                    <div class="result-cell good"></div>
                                </div>
                            </div>
                        </td>

                        <td>
                            <div class="mobile-label">Tests</div>
                            <div class="val">52</div>
                        </td>

                        <td>
                            <div class="mobile-label">Coverage</div>
                            <div class="val">70%</div>
                        </td>

                        <td>
                            <div class="mobile-label">Frequency</div>
                            <div class="val">Every 1h</div>
                        </td>

                        <td>
                            <div class="mobile-label">Tests qty growth</div>
                            <div class="val">
                                <div class="dashboard-block__percent-num plus">
                                    <span>+18%</span>
                                    <i class="icon-arrow"></i>
                                </div>
                            </div>
                        </td>

                        <td>
                            <div class="mobile-label">Coverage growth</div>
                            <div class="val">
                                <div class="dashboard-block__percent-num plus">
                                    <span>+26%</span>
                                    <i class="icon-arrow"></i>
                                </div>
                            </div>
                        </td>
                    </tr>

                    </tbody>
                </table>
            </div>

            <div class="pagination-block">
                <ul>
                    <li class="arrow"><a href="#"><i class="icon-union"></i></a></li>
                    <li class="arrow"><a href="#"><i class="icon-union-single"></i></a></li>

                    <li class="active"><a href="#">1</a></li>
                    <li><a href="#">2</a></li>
                    <li><a href="#">3</a></li>
                    <li><a href="#">4</a></li>
                    <li><a href="#">5</a></li>
                    <li><a href="#">6</a></li>
                    <li><a href="#">7</a></li>

                    <li class="arrow next"><a href="#"><i class="icon-union-single"></i></a></li>
                    <li class="arrow next"><a href="#"><i class="icon-union"></i></a></li>
                </ul>
            </div>
        </div>

    </div>
</template>

<script>
    import CoverageGrowthChart from '../components/CoverageGrowthChart'
    export default {
        name: "Dashboard",
        components: {
            CoverageGrowthChart
        },
        data() {
            return {
                org: {
                    "name": "",
                    "projects": "",
                    "coverage": "",
                    "summary": {
                        "projectsCount": 0,
                        "avgTestsCount": 0,
                        "avgCoverage": 0,
                        "projectsInGreen": 0,
                        "projectsInRed": 0
                    },
                    "uri": ""
                }
            }
        },
        methods: {
            getOrg() {
                var self = this
                this.$http.get(this.user.org)
                    .then((response) => {
                        self.org = response.data
                    })
            },
            bootFailingProjectsChart() {
                this.$nextTick(() => {
                    $('#failing_projects_pie').circleProgress({
                        size: 160,
                        startAngle: -Math.PI / 2,
                        lineCap: 'round',
                        thickness: 20,
                        emptyFill: '#E63F34',
                        fill: '#24A44C'
                    });
                })
            }
        },
        mounted() {
            this.getOrg()
            this.bootFailingProjectsChart()
        },
        computed: {
            user() {
                return this.$store.getters.currentUser
            },
            projectsInGreenPercentile() {
                var total = this.org.summary.projectsInGreen + this.org.summary.projectsInRed
                if(total === 0 || this.org.summary.projectsInGreen === 0) {
                    return 0
                }
                return (this.org.summary.projectsInGreen / total) * 100
            }
        }
    }
</script>
