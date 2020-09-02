import Vue from 'vue'
import Vuex from 'vuex'
Vue.use(Vuex)

export default new Vuex.Store({
  state: {
    count: 100,
    userInfo: null
  },
  mutations: {
    SET_USERINFO(state, userInfo) {
      state.userInfo = userInfo
    }
  },
  actions: {
    SetUserInfo({ commit }, userInfo) {
      commit('SET_USERINFO', userInfo)
    }
  },
  modules: {
  }
})
