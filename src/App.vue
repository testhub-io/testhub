<template>
  <div>
    <header class="page-header">
      <div class="container">
        <div class="page-header__main-row">
          <a @click.prevent="$router.push({ name: 'dashboard' })" class="page-header__logo"><img src="./assets/img/main_logo.png" alt=""></a>

          <div class="page-header__user-block">
            <a href="javascript:;" class="page-header__user-toggle">
              <span class="img-wrapper"><img :src="user.photo_thumb" alt=""></span>
              <span class="name">{{ user.firstName + " " + user.lastName }}</span>
              <i class="icon-chevron"></i>
            </a>
          </div>
        </div>
      </div>
    </header>

    <div id="page-container">
      <div class="container">
        <router-view></router-view>
      </div>
    </div>

    <footer class="page-footer">
      <div class="container">
        <div class="row">
          <div class="col-12 col-md-4 col-lg-4">
            <div class="page-footer__info-block">
              <div class="page-footer__logo"><img src="./assets/img/main_logo_footer.png" alt=""></div>

              <div class="page-footer__copyright">© 2020 TestHub, Inc.</div>
            </div>
          </div>

          <div class="col-12 col-md-8 col-lg-6">
            <div class="row">
              <div class="col-6 col-md-4">
                <div class="page-footer__nav-col">
                  <div class="page-footer__nav-title">Product</div>

                  <ul class="page-footer__nav-list">
                    <li><a href="#">Pricing</a></li>
                    <li><a href="#">Integrations</a></li>
                  </ul>
                </div>
              </div>

              <div class="col-6 col-md-4">
                <div class="page-footer__nav-col">
                  <div class="page-footer__nav-title">Features</div>

                  <ul class="page-footer__nav-list">
                    <li><a href="#">Closed Demo</a></li>
                    <li><a href="#">Why TestHub</a></li>
                  </ul>
                </div>
              </div>

              <div class="col-6 col-md-4">
                <div class="page-footer__nav-col">
                  <div class="page-footer__nav-title">Resources</div>

                  <ul class="page-footer__nav-list">
                    <li><a href="#">Documentation</a></li>
                    <li><a href="#">Help Center</a></li>
                  </ul>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </footer>

  </div>
</template>

<script>
export default {
  name: 'App',
  components: {
  },
  data: function () {
    return {
      loading: true,
    }
  },
  computed: {
    authenticated () {
      return this.$store.getters.isAuthenticated
    },
    user() {
      return this.$store.getters.currentUser
    },
    meta () {
      return this.$store.state.pageMetaStore
    }
  },
  methods: {
  },
  mounted () {
    document.title = this.meta.title
    document.description = this.meta.description
  },
  created () {
  }
}
</script>

<style>
  
</style>
