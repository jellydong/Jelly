<template>
  <div></div>
</template>

<script>
import Oidc from 'oidc-client'

export default {
  data() {
    return {
      mgr: new Oidc.UserManager()
    }
  },
  mounted() {
    console.log('trying to renew the acess_token')
    this.mgr.signinSilentCallback()
  }
}
</script>

<style>

</style>
