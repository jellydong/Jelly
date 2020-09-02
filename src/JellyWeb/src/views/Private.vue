<template>
  <div>
    <h1>这个页面需要权限才能访问</h1>
    <a class="nav-link" @click="renewToken()" href="#">Force Access Token Renewal</a><br/>
    <a class="nav-link" @click="getUser()" href="#">getUser</a>
  </div>
</template>

<script>
import Mgr from '../services/SecurityService'
import { mapActions } from 'vuex'
export default {
  data() {
    return {
      mgr: new Mgr(),
      userinfo: {}
    }
  },
  async mounted() {
    this.userinfo = await this.getProfileAsync()
  },
  methods: {
    ...mapActions(['getProfileAsync']),
    renewToken() {
      this.mgr.renewToken().then(
        newToken => {
          console.log(newToken)
        },
        err => {
          console.log(err)
        })
    },
    getUser() {
      console.log(this.userinfo.name)
    }
  }
}
</script>

<style>
</style>
