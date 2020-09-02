<template>
  <div>
    <h1>这个页面需要权限才能访问</h1>
    <a class="nav-link" @click="renewToken()" href="#">Force Access Token Renewal</a><br/>
    <a class="nav-link" @click="getUser()" href="#">getUser</a>
  </div>
</template>

<script>
import Mgr from '../services/SecurityService'
export default {
  data() {
    return {
      mgr: new Mgr()
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
    getUser() {
      console.log(this.$store.state.user)
    }
  }
}
</script>

<style>
</style>
