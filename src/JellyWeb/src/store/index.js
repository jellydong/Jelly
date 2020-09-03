import Vue from 'vue'
import Vuex from 'vuex'
Vue.use(Vuex)

export default new Vuex.Store({
  state: {
    count: 100,
    userInfo: null,
    accessToken: ''
  },
  mutations: {
    SET_USERINFO(state, userInfo) {
      if (userInfo != null) {
        state.accessToken = userInfo.access_token
        state.userInfo = userInfo.profile
      } else {
        state.accessToken = ''
        state.userInfo = null
      }
    }
  },
  actions: {
    SetUserInfo({ commit }, userInfo) {
      commit('SET_USERINFO', userInfo)
    }
  },
  modules: {
  },
  getters: {
    accessToken: state => state.accessToken
    // accessToken(state) {
    //   return state.accessToken
    // }
  }
})
