import Vue from 'vue'
Vue.config.debug = true
import { BootstrapVue, BootstrapVueIcons } from 'bootstrap-vue'
import moment from 'moment'
import VueBus from 'vue-bus'
import store from './store'
import VueRouter from 'vue-router'
import VueResource from 'vue-resource'
import VueI18n from 'vue-i18n'
import { routes } from './routes'
import authService from './services/auth'
import App from './App.vue'

import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'
import './assets/css/main.css'

window.moment = require('moment-timezone')
window.authService = authService
window.$ = window.jQuery = require('jquery')
require('./assets/js/main.min.js')
require('./assets/js/plugins.min.js')

import './registerServiceWorker'

Vue.use(BootstrapVue)
Vue.use(BootstrapVueIcons)

Vue.use(moment)
Vue.use(VueBus)
Vue.use(VueI18n)
Vue.use(VueResource)
Vue.use(VueRouter)

Vue.http.options.root = 'https://test-hub-api.azurewebsites.net/API/'
let lang = window.Sitedata !== undefined ? window.SiteData.lang : 'en'
const i18n = new VueI18n({
  locale: lang, // set locale
})

const router = new VueRouter({
  mode: 'history',
  base: '/',
  routes,
  scrollBehavior (to, from, savedPosition) {
    if (savedPosition) {
      return savedPosition
    } else {
      return { x: 0, y: 0 }
    }
  }
})

router.beforeEach((to, from, next) => {
  if (to.matched.some(record => record.meta.requiresAuth)) {
    // this route requires auth, check if logged in
    // if not, redirect to login page.
    if (!authService.isAuthenticated()) {
      next({
        name: 'login'
      })
    } else {
      next()
    }
  } else {
    next()
  }
})


/**
 * Interceptors
 */
var requests = []
Vue.http.interceptors.push((request, next) => {
  request.headers.set('Accept', 'application/json')
  request.headers.set('Authorization', 'Bearer ' + authService.getToken())
  requests.push(request)
  // continue to next interceptor
  next((response) => {
    if (!response.ok) {
      switch (response.status) {
        case 401:
          authService.logout()
          store.commit('SET_SELECTED', 'dashboard')
          router.push({ name: 'login' })
          break

        case 422:
          // Distraction free error for validation. Should be handled by the component
          break

        case 429:
          //
          break

        case 402:
          // Do Nothing on this end
          // Payment required!
          break

        case 403:
          break

        case 404:
          // Something was not found, let the component
          // that did the request handle that...
          break

        case 500:
          // Something was not found, let the component
          // that did the request handle that...
          break

        default:
          // Something went wrong...
          break
      }
    }
  })
})

const app = new Vue({
  store,
  router,
  i18n,
  render: h => h(App)
}).$mount('#app')

Vue.prototype.$locale = {
  change (lang, data) {
    app.$i18n.locale = lang
    app.$i18n.setLocaleMessage(lang, data)
  },
  current () {
    return app.$i18n.locale
  }
}

Vue.prototype.$window = window
