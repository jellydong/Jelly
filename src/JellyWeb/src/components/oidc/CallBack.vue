<template>
  <div></div>
</template>

<script>
import Oidc from 'oidc-client'

new Oidc.UserManager()
  .signinRedirectCallback({ userStore: new Oidc.WebStorageStateStore(), loadUserInfo: true, filterProtocolClaims: true })
  .then(function (res) {
    debugger
    window.location = '/#/Home'
  })
  .catch(function (e) {})
export default {}
</script>

<style>
</style>
