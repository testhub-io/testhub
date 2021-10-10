const state = {
  selected: window.localStorage.getItem('selectedFrontendMenuItem') || 'home',
  selectedSubmenu: window.localStorage.getItem('selectedFrontendSubMenuItem') || '',
  hidden: false
}

const mutations = {
  SET_SELECTED (state, value) {
    window.localStorage.setItem('selectedFrontendMenuItem', value)
    state.selected = value
    // Reset submenu
    window.localStorage.removeItem('selectedFrontendSubMenuItem')
    state.selectedSubmenu = ''
  },
  SET_SELECTED_SUBMENU (state, value) {
    window.localStorage.setItem('selectedFrontendSubMenuItem', value)
    state.selectedSubmenu = value
  },
  CLEAR_SELECTED (state) {
    window.localStorage.removeItem('selectedFrontendMenuItem')
    state.selected = ''
  },
  CLEAR_SELECTED_SUBMENU (state) {
    window.localStorage.removeItem('selectedFrontendSubMenuItem')
    state.selectedSubmenu = ''
  },
  CLEAR_MENU (state) {
    window.localStorage.setItem('selectedFrontendMenuItem', 'home')
    state.selected = 'home'
  }
}

const actions = {
  clearFrontendMenu: ({ commit }) => {
    commit('CLEAR_MENU')
  },
  toggleSideBar: ({ commit }) => {
    commit('TOGGLE_SIDEBAR')
    return true
  },
  setMenuItem: ({ commit }, value) => {
    commit('SET_SELECTED', value)
    return true
  },
  setFrontendMenuCategory: ({ commit }, category) => {
    commit('SET_SELECTED', 'category-nav-' + category.id)
    return true
  }
}

export default {
  state, mutations, actions
}