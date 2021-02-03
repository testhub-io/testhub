<template>
  <b-navbar
    toggleable="lg"
    type="dark"
    :variant="colorVariant"
    :fixed="fixed"
    :class="{ 'mb-4': $route.path !== '/' }"
  >
    <div class="container">
      <b-navbar-brand to="/">
        <img src="@/assets/img/main_logo.png" height="32" alt="" />
      </b-navbar-brand>

      <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>

      <b-collapse id="nav-collapse" is-nav>
        <b-navbar-nav class="m-auto">
          <!-- <b-nav-item
            :class="{ 'is-active': $route.path === '/' }"
            to="/"
            class="px-3"
            >Home</b-nav-item
          > -->
          <!-- <b-nav-item-dropdown
            :class="{ 'is-active': $route.path !== '/' }"
            class="px-3"
            no-caret
            @toggle="getDegree"
          >
            <template #button-content>
              Docs
              <transition name="flip" mode="out-in">
                <b-icon
                  class="dropdown-icon"
                  icon="chevron-down"
                  font-scale="0.9"
                  aria-hidden="true"
                  :rotate="degree"
                  :key="degree"
                ></b-icon>
              </transition>
            </template>
            <b-dropdown-item to="/Dashboard">dashboard</b-dropdown-item>
            <b-dropdown-item to="/changelog">Changelog</b-dropdown-item>
          </b-nav-item-dropdown> -->
        </b-navbar-nav>

        <!-- Right aligned nav items -->
        <b-navbar-nav>
          <b-nav-item right>
            <!-- Using 'button-content' slot -->
            <b-button variant="primary">Try Now</b-button>
          </b-nav-item>
        </b-navbar-nav>
      </b-collapse>
    </div>
  </b-navbar>
</template>

<script>
export default {
  data() {
    return {
      isLight: false,
      colorVariant: "transparent",
      degree: 0,
      fixed: "top",
    };
  },
  watch: {
    $route(to) {
      if (to.fullPath === "/") {
        this.fixed = "top";
        this.colorVariant = "transparent";
      } else {
        this.fixed = "";
        this.colorVariant = "white";
      }
    },
  },
  created() {
    window.addEventListener("scroll", this.handleScroll);
    if (this.$route.fullPath === "/") {
      this.colorVariant = "transparent";
      this.fixed = "top";
    } else {
      this.colorVariant = "white";
      this.fixed = "";
    }
  },
  destroyed() {
    window.removeEventListener("scroll", this.handleScroll);
  },
  methods: {
    handleScroll() {
      // Any code to be executed when the window is scrolled
      if (window.scrollY < 50 && this.$route.fullPath === "/") {
        this.isLight = false;
        this.colorVariant = "transparent";
      } else {
        this.isLight = true;
        this.colorVariant = "white";
      }
    },
    getDegree() {
      if (this.degree === 0) {
        this.degree = 180;
      } else {
        this.degree = 0;
      }
    },
  },
};
</script>

<style lang="scss">
.bg-transparent {
  transition: all 0.5s ease;
}
.bg-white {
  transition: all 0.5s ease;
  box-shadow: 0 0 3px rgba(60, 72, 88, 0.15) !important;
}
.navbar-dark .navbar-nav .nav-link {
  color: #3c4858 !important;
  font-size: 13px;
  font-weight: 700;
  letter-spacing: 1px;
  line-height: 24px;
  text-transform: uppercase;
  transition: all 0.5s;
  font-family: "Nunito", sans-serif;
  padding-left: 15px;
  padding-right: 15px;
  transition: all 0.3s linear;
  .dropdown-icon {
    margin-bottom: 0.125rem;
  }
  &:hover {
    color: #3c4858 !important;
  }
}
.navbar-dark .navbar-nav .is-active a {
  color: #2f55d4 !important;
  &:hover {
    color: #2f55d4 !important;
  }
}
.dropdown-menu {
  border: none;
  box-shadow: 0 0 3px rgba(60, 72, 88, 0.15);
}
.dropdown-menu li a {
  color: #3c4858 !important;
}
.flip-leave-active {
  transform: rotate(180deg);
  transition: all 0.5s;
}
/* .flip-enter, .flip-leave-to{
  transform: rotate(0deg);
  transition: all .5s;
} */
</style>
