import Vue from 'vue'
import Vuex from 'vuex'

import userStore from './stores/userStore'
import pageMetaStore from './stores/pageMetaStore'
import frontendMenuStore from './stores/frontendMenuStore'
import coverageFilesStore from './stores/coverageFilesStore.js'

Vue.use(Vuex)
const debug = process.env.NODE_ENV === 'production'

export default new Vuex.Store({
  modules: {
    userStore,
    pageMetaStore,
    frontendMenuStore,
    coverageFilesStore
  },
  strict: debug
})
