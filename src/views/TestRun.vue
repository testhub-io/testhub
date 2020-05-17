<template>
  <div>
    <div class="breadcrumbs-block">
      <ul>
        <li><a href="#">Wibbits</a></li>
        <li><a href="#">Capsule</a></li>
        <li><span>#2045634</span></li>
      </ul>
    </div>

    <div class="panel mb-3">
      <div class="testrun-block__numbers-block">
        <div class="testrun-block__number-item">
          <div style="color: #24A44C;" class="testrun-block__num-elem">127</div>
          <div class="testrun-block__num-label">Passed</div>
        </div>

        <div class="testrun-block__number-item">
          <div style="color: #E63F34;" class="testrun-block__num-elem">12</div>
          <div class="testrun-block__num-label">Failed</div>
        </div>

        <div class="testrun-block__number-item">
          <div style="color: #F98809;" class="testrun-block__num-elem">0</div>
          <div class="testrun-block__num-label">Skipped</div>
        </div>

        <div class="testrun-block__number-item">
          <div class="testrun-block__num-elem">139</div>
          <div class="testrun-block__num-label">Total tests</div>
        </div>

        <div class="testrun-block__number-item">
          <div class="testrun-block__num-elem">0.27</div>
          <div class="testrun-block__num-label">Execution time</div>
        </div>

        <div class="testrun-block__number-item ml-auto">
          <div style="color: #24A44C;" class="testrun-block__num-elem">95%</div>
          <div class="testrun-block__num-label">Total coverage</div>
        </div>
      </div>
    </div>

    <div class="nav-tabs__wrapper">
      <ul class="nav nav-tabs">
        <li class="nav-item">
          <a class="nav-link active" data-toggle="tab" data-target="#test_results_tab" href="javascript:;">Test results</a>
        </li>
        <li class="nav-item">
          <a class="nav-link" data-toggle="tab" data-target="#coverage_tab" href="javascript:;">Coverage</a>
        </li>
      </ul>
    </div>

    <div class="tab-content">
      <div class="tab-pane active" id="test_results_tab">
        <div class="filter-block">
          <div class="row align-items-center">
            <div class="col-12 col-md-4 col-lg-3">
              <div class="filter-block__search-block">
                <input type="text" class="form-control" placeholder="Search by name">
                <button type="submit" title="Search" class="search-button"><i class="icon-search"></i></button>
              </div>
            </div>

            <div class="col-12 col-md-8 col-lg-auto">
              <div class="filter-block__status-filter">
                <div class="label">Filter by status</div>

                <div class="filter-block__status-row">
                  <div class="filter-block__status-item passed">
                    <input type="checkbox" id="filter_status_passed">
                    <label for="filter_status_passed">Passed</label>
                  </div>

                  <div class="filter-block__status-item failed">
                    <input type="checkbox" id="filter_status_failed">
                    <label for="filter_status_failed">Failed</label>
                  </div>

                  <div class="filter-block__status-item skipped">
                    <input type="checkbox" id="filter_status_skipped">
                    <label for="filter_status_skipped">Skipped</label>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div class="testrun-block__table-wrapper">
          <div class="testrun-block__table-title">E2eNode Suite</div>

          <div class="testrun-block__table">
            <div class="testrun-block__table-item">
              <div class="testrun-block__table-row">
                <div class="testrun-block__table-main-td">[k8s.io] InitContainer [NodeConformance] should invoke init </div>

                <div class="testrun-block__table-date-td">
                  <div class="testrun-block__date-time">
                    <i class="icon-clock"></i>
                    <span>7.14</span>
                  </div>
                </div>

                <div class="testrun-block__table-results-td">
                  <div class="dashboard-block__results-cells">
                    <div class="result-cell good"></div>
                    <div class="result-cell bad"></div>
                    <div class="result-cell bad"></div>
                    <div class="result-cell good"></div>
                    <div class="result-cell good"></div>
                  </div>
                </div>
              </div>
            </div>

            <div class="testrun-block__table-item">
              <div class="testrun-block__table-row">
                <div class="testrun-block__table-main-td">Feature:DynamicKubeletConfig</div>

                <div class="testrun-block__table-date-td">
                  <div class="testrun-block__date-time">
                    <i class="icon-clock"></i>
                    <span>7.14</span>
                  </div>
                </div>

                <div class="testrun-block__table-results-td">
                  <div class="dashboard-block__results-cells">
                    <div class="result-cell good"></div>
                    <div class="result-cell bad"></div>
                    <div class="result-cell bad"></div>
                    <div class="result-cell good"></div>
                    <div class="result-cell good"></div>
                  </div>
                </div>
              </div>
            </div>

            <div class="testrun-block__table-item">
              <div class="testrun-block__table-row">
                <div class="testrun-block__table-main-td" style="color: #E63F34;">[sig-storage] EmptyDir volumes[LinuxOnly] [NodeConformance] [Conformance]</div>

                <div class="testrun-block__table-date-td">
                  <div class="testrun-block__date-time">
                    <i class="icon-clock"></i>
                    <span>1.10</span>
                  </div>
                </div>

                <div class="testrun-block__table-results-td">
                  <div class="dashboard-block__results-cells">
                    <div class="result-cell bad"></div>
                    <div class="result-cell bad"></div>
                    <div class="result-cell bad"></div>
                    <div class="result-cell bad"></div>
                    <div class="result-cell bad"></div>
                  </div>
                </div>
              </div>

              <div class="testrun-block__error-block">
                <p>/go/src/k8s.io/kubernetes/_output/local/go/src/k8s.io/kubernetes/test/e2e/common/security_context.go:141\nImage \"gcr.io/kubernetes-e2e-test-images/nonroot:1.0\" is not in the white list, consider adding it to CommonImageWhiteList in test/e2e/common/util.go or NodeImageWhiteList in test/e2e_node/image_list.go\nExpected\n </p>
              </div>
            </div>

            <div class="testrun-block__table-item">
              <div class="testrun-block__table-row">
                <div class="testrun-block__table-main-td">[k8s.io] InitContainer [NodeConformance] should invoke init </div>

                <div class="testrun-block__table-date-td">
                  <div class="testrun-block__date-time">
                    <i class="icon-clock"></i>
                    <span>7.14</span>
                  </div>
                </div>

                <div class="testrun-block__table-results-td">
                  <div class="dashboard-block__results-cells">
                    <div class="result-cell good"></div>
                    <div class="result-cell bad"></div>
                    <div class="result-cell bad"></div>
                    <div class="result-cell good"></div>
                    <div class="result-cell good"></div>
                  </div>
                </div>
              </div>
            </div>

            <div class="testrun-block__table-item">
              <div class="testrun-block__table-row">
                <div class="testrun-block__table-main-td">Feature:DynamicKubeletConfig</div>

                <div class="testrun-block__table-date-td">
                  <div class="testrun-block__date-time">
                    <i class="icon-clock"></i>
                    <span>7.14</span>
                  </div>
                </div>

                <div class="testrun-block__table-results-td">
                  <div class="dashboard-block__results-cells">
                    <div class="result-cell good"></div>
                    <div class="result-cell bad"></div>
                    <div class="result-cell bad"></div>
                    <div class="result-cell good"></div>
                    <div class="result-cell good"></div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div class="tab-pane" id="coverage_tab">
        <h3>Coverage Tab Content</h3>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  data: function() {
    return {
    }
  },
  components: {
  },
  methods: {

  },
  mounted() {

  }
}
</script>
