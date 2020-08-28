import Vue from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import Mgr from './services/SecurityService'
const mgr = new Mgr()

Vue.config.productionTip = false

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount('#app')

// 挂载路由导航守卫
router.beforeEach((to, from, next) => {
  // to 将要访问的路径
  // from 代表从哪个路径跳转而来
  // next 是一个函数，表示放行
  //     next()  放行    next('/login')  强制跳转

  const requiresAuth = to.matched.some(record => record.meta.requiresAuth)
  console.log(requiresAuth)
  debugger
  if (to.path.indexOf('id_token') !== -1) {
    return next()
  }
  if (!requiresAuth) {
    mgr.getUser().then(
      user => {
        if (user === null) {
          mgr.getSignedIn().then(
            signIn => {
              console.log(signIn)
            })
        }
      },
      err => {
        console.log(err)
      }
    )
  } else {
    next()
  }
})
