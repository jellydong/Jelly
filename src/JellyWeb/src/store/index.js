import Vue from 'vue'
import Vuex from 'vuex'
import Mgr from '../services/SecurityService'
Vue.use(Vuex)
const mgr = new Mgr()

export default new Vuex.Store({
  state: {
    count: 100
  },
  mutations: {
  },
  actions: {
    async getProfileAsync() {
      return await mgr.getProfile()
    }
  },
  modules: {
  }
})
