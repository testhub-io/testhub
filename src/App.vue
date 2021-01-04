<template>
  <div id="app">
    <nav-bar />
    <b-container>
      <router-view></router-view>
    </b-container>
    <app-footer />
  </div>
</template>

<script>
import NavBar from "@/components/shared/NavBar.vue";
import AppFooter from "@/components/shared/AppFooter";
export default {
  name: "App",
  components: {
    NavBar,
    AppFooter,
  },
  data: function() {
    return {
      loading: true,
      meta: {
        title: "",
        description: "",
      },
    };
  },
  computed: {
    isLandingPage() {
      return this.$router.currentRoute.name === "landing";
    },
  },
  methods: {},
  mounted() {
    document.title = this.meta.title;
    document.description = this.meta.description;
  },
  created() {},
};
</script>
