<template>
  <div></div>
</template>

<script>
import Mgr from '../../services/SecurityService'

export default {
  data() {
    return {
      user: new Mgr({ response_mode: 'query' })
    }
  },
  mounted() {
    console.log('renewing tokens')

    this.user.signinSilentCallback()
  }
}
</script>

<style>
</style>
