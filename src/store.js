import Vue from 'vue'
import Vuex from 'vuex'

import userStore from './stores/userStore'
import pageMetaStore from './stores/pageMetaStore'
import frontendMenuStore from './stores/frontendMenuStore'

Vue.use(Vuex)
const debug = process.env.NODE_ENV === 'production'

export default new Vuex.Store({
  modules: {
    userStore,
    pageMetaStore,
    frontendMenuStore
  },
  strict: debug
})