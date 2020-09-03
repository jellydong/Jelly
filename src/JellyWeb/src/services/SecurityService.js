import Oidc from 'oidc-client'
import store from '../store/index'
import { Message } from 'element-ui'

var mgr = new Oidc.UserManager({
  userStore: new Oidc.WebStorageStateStore(),
  authority: 'http://localhost:8000',
  client_id: 'jellyweb',
  redirect_uri: window.location.origin + '/CallBack',
  response_type: 'id_token token',
  scope: ' openid profile scope1 address email phone offline_access',
  post_logout_redirect_uri: window.location.origin + '/',
  silent_redirect_uri: window.location.origin + '/SilentCallback',
  // 自动刷新Token使用自动刷新Token需要accessTokenExpiringNotificationTime和automaticSilentRenew 一起设置，当AccssToken要过期前:accessTokenExpiringNotificationTime设置的时间，会去请求IdentityServer4 connect/token接口，刷新Token
  accessTokenExpiringNotificationTime: 10,
  automaticSilentRenew: true,
  filterProtocolClaims: true,
  includeIdTokenInSilentRenew: true,
  loadUserInfo: true
})

// Oidc.Log.logger = console
// Oidc.Log.level = Oidc.Log.INFO

// 在建立(或重新建立)用户会话时引发。实际只有重新刷新的时候才会触发？
mgr.events.addUserLoaded(function (user) {
  store.dispatch('SetUserInfo', user)
  console.log('New User Loaded：', arguments)
  console.log('Acess_token: ', user.access_token)
})

// 在访问令牌到期之前引发。
mgr.events.addAccessTokenExpiring(function () {
  console.log('AccessToken Expiring：', arguments)
})

// 在访问令牌过期后引发。
mgr.events.addAccessTokenExpired(function () {
  console.log('AccessToken Expired：', arguments)
  Message({
    message: 'Session expired. Going out!',
    type: 'error',
    duration: 5 * 1000
  })
  store.dispatch('SetUserInfo', null)
  mgr.signoutRedirect().then(function (resp) {
    console.log('signed out', resp)
  }).catch(function (err) {
    console.log(err)
  })
})

// 静默登录失败时
mgr.events.addSilentRenewError(function () {
  console.error('Silent Renew Error：', arguments)
})

// 用户登录状态发生变化时
mgr.events.addUserSignedOut(function () {
  console.log('用户已登出')
  Message({
    message: '用户已登出',
    type: 'error',
    duration: 5 * 1000
  })
  store.dispatch('SetUserInfo', null)
  console.log('UserSignedOut：', arguments)
  mgr.signoutRedirect().then(function (resp) {
    console.log('signed out', resp)
  }).catch(function (err) {
    console.log(err)
  })
})

export default class SecurityService {
  // Renew the token manually
  renewToken() {
    const self = this
    return new Promise((resolve, reject) => {
      debugger
      mgr.signinSilent().then(function (user) {
        debugger
        if (user == null) {
          self.signIn(null)
        } else {
          return resolve(user)
        }
      }).catch(function (err) {
        console.log(err)
        return reject(err)
      })
    })
  }

  // Get the user who is logged in
  getUser() {
    const self = this
    return new Promise((resolve, reject) => {
      mgr.getUser().then(function (user) {
        if (user == null) {
          self.signIn()
          return resolve(null)
        } else {
          return resolve(user)
        }
      }).catch(function (err) {
        console.log(err)
        return reject(err)
      })
    })
  }

  signinRedirectCallback() {
    return new Promise((resolve, reject) => {
      mgr.signinRedirectCallback().then(function (res) {
        return resolve(res)
      }
      ).catch(function (err) {
        return reject(err)
      })
    })
  }

  signinSilentCallback() {
    return new Promise((resolve, reject) => {
      mgr.signinSilentCallback().then(function (res) {
        return resolve(res)
      }
      ).catch(function (err) {
        return reject(err)
      })
    })
  }

  // Check if there is any user logged in
  getSignedIn(returnPath) {
    const self = this
    return new Promise((resolve, reject) => {
      mgr.getUser().then(function (user) {
        if (user == null) {
          self.signIn(returnPath)
          return resolve(false)
        } else {
          store.dispatch('SetUserInfo', user)
          return resolve(true)
        }
      }).catch(function (err) {
        console.log(err)
        return reject(err)
      })
    })
  }

  // Redirect of the current window to the authorization endpoint.
  signIn(returnPath) {
    console.log('signIn')
    returnPath ? mgr.signinRedirect({ state: returnPath })
      : mgr.signinRedirect()
  }

  // Redirect of the current window to the end session endpoint
  signOut() {
    mgr.signoutRedirect().then(function (resp) {
      console.log('signed out', resp)
    }).catch(function (err) {
      console.log(err)
    })
  }

  // Get the profile of the user logged in
  getProfile() {
    const self = this
    return new Promise((resolve, reject) => {
      mgr.getUser().then(function (user) {
        if (user == null) {
          self.signIn()
          return resolve(null)
        } else {
          return resolve(user.profile)
        }
      }).catch(function (err) {
        console.log(err)
        return reject(err)
      })
    })
  }

  // Get the token id
  getIdToken() {
    const self = this
    return new Promise((resolve, reject) => {
      mgr.getUser().then(function (user) {
        if (user == null) {
          self.signIn()
          return resolve(null)
        } else {
          return resolve(user.id_token)
        }
      }).catch(function (err) {
        console.log(err)
        return reject(err)
      })
    })
  }

  // Get the session state
  getSessionState() {
    const self = this
    return new Promise((resolve, reject) => {
      mgr.getUser().then(function (user) {
        if (user == null) {
          self.signIn()
          return resolve(null)
        } else {
          return resolve(user.session_state)
        }
      }).catch(function (err) {
        console.log(err)
        return reject(err)
      })
    })
  }

  // Get the access token of the logged in user
  getAcessToken() {
    const self = this
    return new Promise((resolve, reject) => {
      mgr.getUser().then(function (user) {
        if (user == null) {
          self.signIn()
          return resolve(null)
        } else {
          return resolve(user.access_token)
        }
      }).catch(function (err) {
        console.log(err)
        return reject(err)
      })
    })
  }

  // Takes the scopes of the logged in user
  getScopes() {
    const self = this
    return new Promise((resolve, reject) => {
      mgr.getUser().then(function (user) {
        if (user == null) {
          self.signIn()
          return resolve(null)
        } else {
          return resolve(user.scopes)
        }
      }).catch(function (err) {
        console.log(err)
        return reject(err)
      })
    })
  }

  // Get the user roles logged in
  getRole() {
    const self = this
    return new Promise((resolve, reject) => {
      mgr.getUser().then(function (user) {
        if (user == null) {
          self.signIn()
          return resolve(null)
        } else {
          return resolve(user.profile.role)
        }
      }).catch(function (err) {
        console.log(err)
        return reject(err)
      })
    })
  }
}
