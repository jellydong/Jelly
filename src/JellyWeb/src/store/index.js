import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

export default new Vuex.Store({
  state: {
    count: 100,
    user: {
      name: '张三'
    },
    accessToken: ''
  },
  mutations: {
    setUser(state, user) {
      debugger
      this.state.user = user
      this.state.accessToken = user.access_token
    }
  },
  actions: {
  },
  modules: {
  }
})
