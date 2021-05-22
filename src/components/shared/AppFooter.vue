<template>
  <footer class="page-footer">
    <b-container>
      <b-row>
        <b-col cols="12" md="4" lg="4">
          <div
            class="d-flex flex-column align-items-start justify-content-between h-100"
          >
            <div class="page-footer_logo">
              <b-img :src="require('@/assets/img/main_logo_footer.png')" fluid />
            </div>

            <div class="page-footer_copyright">© 2020 TestHub, Inc.</div>
          </div>
        </b-col>

        <b-col cols="12" md="8" lg="6">
          <!-- <b-row>
            <b-col cols="6" md="4">
              <div class="page-footer_nav-col">
                <div class="page-footer_nav-title">Product</div>

                <ul class="page-footer_nav-list">
                  <li><a href="#">Pricing</a></li>
                  <li><a href="#">Integrations</a></li>
                </ul>
              </div>
            </b-col>

            <b-col cols="6" md="4">
              <div class="page-footer_nav-col">
                <div class="page-footer_nav-title">Features</div>

                <ul class="page-footer_nav-list">
                  <li><a href="#">Closed Demo</a></li>
                  <li><a href="#">Why TestHub</a></li>
                </ul>
              </div>
            </b-col>

            <b-col cols="6" md="4">
              <div class="page-footer__nav-col">
                <div class="page-footer_nav-title">Resources</div>

                <ul class="page-footer_nav-list">
                  <li><a href="#">Documentation</a></li>
                  <li><a href="#">Help Center</a></li>
                </ul>
              </div>
            </b-col>
          </b-row> -->
        </b-col>
      </b-row>
    </b-container>
  </footer>
</template>

<script>
export default {};
</script>

<style lang="scss" scoped>
.page-footer {
  width: 100%;
  position: relative;
  padding-top: 45px;
  padding-bottom: 45px;
  margin-top: 45px;
  &_logo {
    width: 157px;
  }
  &_copyright {
    font-size: 16px;
    color: #08090a;
  }
  &_nav-title {
    font-size: 18px;
    font-weight: 600;
    color: #08090a;
    margin-bottom: 32px;
  }
  &_nav-list {
    list-style: none;
    padding: 0;
    margin: 0;
    li {
      a {
        font-size: 16px;
        text-decoration: none !important;
        color: #08090a;
        transition: all 0.2s ease-in-out;
        &:hover {
          color: #0038ff;
        }
      }
      &:not(:last-child) {
        margin-bottom: 15px;
      }
    }
  }
}
</style>
