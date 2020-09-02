<template>
  <div></div>
</template>

<script>
import Oidc from 'oidc-client'

export default {
  mounted() {
    console.log('renewing tokens')

    new Oidc.UserManager().signinSilentCallback()
  }
}
</script>

<style>
</style>
