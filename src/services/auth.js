var Cookies = require('cookies-js')
import store from './../store.js'
Cookies.defaults = {
  path: '/'
}

const authService = {
  isAuthenticated: function () {
    return Cookies.get('token')
  },

  getToken: function () {
    if (this.isMasqueraded()) {
      return this.getMasqueradedToken()
    }
    return Cookies.get('token')
  },

  login: function (response, remember) {
    // remember for 90 days if wanted
    var options = (remember === true) ? { expires: 60 * 60 * 24 * 90 } : { expires: 60 * 60 * 24 * 90 }
    Cookies.set('token', response.body.token, options)
    store.dispatch('setUserObject', response.body)
  },

  logout: function () {
    Cookies.expire('token')
    Cookies.expire('masqueraded')
    window.localStorage.clear()
    store.dispatch('clearAuthUser')
  },

  masquerade: function (profile) {
    Cookies.set('masqueraded', profile.token)
    window.location = '/'
  },

  isMasqueraded: function () {
    return Cookies.get('masqueraded')
  },

  getMasqueradedToken: function () {
    return Cookies.get('masqueraded')
  },

  unmask: function () {
    Cookies.expire('masqueraded')
    window.location = '/'
  }
}
export default authService
