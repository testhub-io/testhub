const state = {
  title: '',
  description: '',
  page_title: ''
}

const mutations = {
  SET_META (state, meta) {
    state.title = meta.title || ''
    state.description = meta.description || ''
    state.page_title = meta.page_title || ''
    state.url = meta.url || ''
    state.type = meta.type || ''
    state.image = meta.image || ''
    // Set title
    document.title = state.title
    document.description = state.description
  }
}

const actions = {
  setMeta ({ commit }, meta) {
    commit('SET_META', meta)
  }
}

export default {
  state, actions, mutations
}
