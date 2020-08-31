<template>
  <div></div>
</template>

<script>
import Oidc from 'oidc-client'
console.log('renewing tokens')
new Oidc.UserManager({
  userStore: new Oidc.WebStorageStateStore({
    store: window.localStorage
  })
}).signinSilentCallback()
export default {}
</script>

<style>
</style>
