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
                            <div class="dashboard-block__num">{{ Math.floor(this.org.summary.avgTestsCount) }}</div>
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
                        <div class="circle-block" id="failing_projects_pie"
                             :data-value="this.projectsInGreenPercentile"></div>
                        <div class="center-caption">
                            <div class="num">{{ this.projectsInGreenPercentile }}%</div>
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
                    <tr v-for="(project,index) in projects" :key="index" @click.prevent="gotoRuns(project)">
                        <td>
                            <div class="mobile-label">Project Name</div>
                            <div class="val"><b>{{ project.name }}</b></div>
                        </td>

                        <td>
                            <div class="mobile-label">Last results</div>
                            <div class="val">
                                <div class="dashboard-block__results-cells">
                                    <div v-for="(testResult, projectIndex) in project.latestResults.testResults" :key="projectIndex" :class="getTestResultStatus(testResult)"></div>
                                </div>
                            </div>
                        </td>

                        <td>
                            <div class="mobile-label">Tests</div>
                            <div class="val">{{ project.testsCount }}</div>
                        </td>

                        <td>
                            <div class="mobile-label">Coverage</div>
                            <div class="val">{{ project.coverage }}%</div>
                        </td>

                        <td>
                            <div class="mobile-label">Frequency</div>
                            <div class="val">Every {{ project.testRunFrequency }}</div>
                        </td>

                        <td>
                            <div class="mobile-label">Tests qty growth</div>
                            <div class="val">
                                <div class="dashboard-block__percent-num plus">
                                    <span>{{ parseInt(project.testQuantityGrowth) >= 0 ? "+" : "-" }} {{ project.testQuantityGrowth }}%</span>
                                    <i class="icon-arrow"></i>
                                </div>
                            </div>
                        </td>

                        <td>
                            <div class="mobile-label">Coverage growth</div>
                            <div class="val">
                                <div class="dashboard-block__percent-num plus">
                                    <span>{{ parseInt(project.coverageGrowth) >= 0 ? "+" : "-" }} {{ project.coverageGrowth }}%</span>
                                    <i class="icon-arrow"></i>
                                </div>
                            </div>
                        </td>
                    </tr>

                    </tbody>
                </table>
            </div>

            <Pagination
                    :pagination="projectsPagination"
                    :callback="loadProjectsPage"
            />

        </div>

    </div>
</template>

<script>
    import CoverageGrowthChart from '../components/CoverageGrowthChart'
    import Pagination from '../components/Pagination'

    export default {
        name: "Dashboard",
        components: {
            CoverageGrowthChart,
            Pagination
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
                },
                projects: [],
                projectsPagination: {
                    "itemsCount": 1,
                    "pageSize": 10,
                    "currentPage": 1,
                    "totalPages": 1,
                    "links": {
                        "next": null
                    }
                }
            }
        },
        methods: {
            getOrg() {
                var self = this
                this.$http.get(this.userOrg)
                    .then((response) => {
                        self.org = response.data
                        this.bootFailingProjectsChart()
                    })
            },
            getOrgProjects(page = 1) {
                var self = this
                this.$http.get(this.userOrg + "/projects/?page=" + page)
                    .then((response) => {
                        self.projects = response.data.data
                        self.projectsPagination = response.data.meta.pagination
                    })
            },
            getTestResultStatus(result) {
                return result === 1 ? "result-cell good" : "result-cell bad"
            },
            loadProjectsPage(page) {
                this.getOrgProjects(page)
                this.$scrollTo(".dashboard-block__table-wrapper", 500, {})
            },
            gotoRuns(project) {
                this.$router.push({ name: 'project-test-runs', params: { org: this.userOrg, project: project.name } })
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
            this.getOrgProjects()
        },
        computed: {
            user() {
                return this.$store.getters.currentUser
            },
            userOrg() {
                return this.$route.params.org ? this.$route.params.org : this.user.org
            },
            projectsInGreenPercentile() {
                var total = this.org.summary.projectsInGreen + this.org.summary.projectsInRed
                // console.log("TOTAL", total)
                // console.log("RED", this.org.summary.projectsInRed)
                // console.log("GREEN", this.org.summary.projectsInGreen)
                if (total === 0 || this.org.summary.projectsInGreen === 0) {
                    return 0
                }
                var percentile = this.org.summary.projectsInGreen / total
                // console.log("percentile", percentile)
                return Math.floor(percentile * 100)
            }
        }
    }
</script>

<style lang="scss" >
    .table {
        tr {
            cursor: pointer;
        }
    }
</style>
