var Cookies = require('cookies-js')
const state = {
  authUser: Cookies.get('token'),
  user: {
    firstName: 'JAV',
    lastName: '20',
    email: 'jav20@gmail.com',
    photo: '/img/user-img.e2fe36bd.png',
    photo_thumb: '/img/user-img.e2fe36bd.png',
    roles: {
      data: []
    },
    org: "test-hub"
  }
}

const mutations = {
  SET_AUTH_USER (state, userObj) {
    state.authUser = userObj
  },
  CLEAR_AUTH_USER (state) {
    state.authUser = false
    state.user = false
  },
  SET_USER (state, user) {
    state.user = user
  }
}

const actions = {
  setUserObject: ({ commit }, userObj) => {
    commit('SET_AUTH_USER', userObj)
    return true
  },
  clearAuthUser: ({ commit }) => {
    commit('CLEAR_AUTH_USER')
    return true
  },
  setUser: ({ commit }, user) => {
    commit('SET_USER', user)
    return true
  }
}

const getters = {
  isAuthenticated: (state) => {
    return state.authUser !== undefined && state.authUser !== false && state.authUser !== null
  },
  currentUser: (state) => {
    return (state.user !== undefined) && (state.user !== false) ? state.user : {}
  },
  currentUserRoles: (state) => {
    return (state.user !== undefined) && (state.user !== false) ? state.user.roles : []
  },
  currentUserPermissions: (state) => {
    return (state.user !== undefined) && (state.user !== false) ? state.user.permissions : []
  }
}

export default {
  state, mutations, actions, getters
}
