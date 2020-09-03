<template>
  <div>
    <h1>这个页面需要权限才能访问</h1>
    <a class="nav-link" @click="renewToken()" href="#">Force Access Token Renewal</a><br/>
    <a class="nav-link" @click="getCount()" href="#">getCount</a><br/>
    <a class="nav-link" @click="getUserVuexState()" href="#">getUserVuexState</a><br/>
    <a class="nav-link" @click="elementMessage()" href="#">elementMessage</a><br/>
  </div>
</template>

<script>
import Mgr from '../services/SecurityService'
import { mapState } from 'vuex'
export default {
  data() {
    return {
      mgr: new Mgr()
    }
  },
  computed: {
    ...mapState(['count', 'userInfo'])
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
      console.log(this.$store.state.userInfo)
    },
    elementMessage() {
      this.$message.info('info 消息')
    }
  }
}
</script>

<style>
</style>
