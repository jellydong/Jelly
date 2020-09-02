<template>
  <div>
    <h1>这个页面需要权限才能访问</h1>
    <a class="nav-link" @click="renewToken()" href="#">Force Access Token Renewal</a><br/>
    <a class="nav-link" @click="getCount()" href="#">getCount</a><br/>
    <a class="nav-link" @click="getUserVuexState()" href="#">getUserVuexState</a><br/>
  </div>
</template>

<script>
import Mgr from '../services/SecurityService'
import { mapState } from 'vuex'

export default {
  data() {
    return {
      mgr: new Mgr(),
      ...mapState(['count'])
    }
  },
  methods: {
    renewToken() {
      this.mgr.renewToken().then(
        newToken => {
          console.log(newToken)
        },
        err => {
          console.log(err)
        })
    },
    getCount() {
      console.log(this.count)
    },
    getUserVuexState() {
      console.log(this.userInfo)
    }
  }
}
</script>

<style>
</style>
