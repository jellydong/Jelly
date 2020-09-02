import Vue from 'vue'
import VueRouter from 'vue-router'

import Home from '../views/Home.vue'
import Private from '../views/Private.vue'
import Unauthorized from '../views/Unauthorized.vue'
import CallBack from '../components/oidc/CallBack.vue'
import SilentCallback from '../components/oidc/SilentCallback.vue'
import Mgr from '../services/SecurityService'
const mgr = new Mgr()

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    redirect: '/home'
  },
  {
    path: '/home',
    name: 'Home',
    component: Home,
    meta: {
      notRequiresAuth: true
    }
  },
  {
    path: '/about',
    name: 'About',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "about" */ '../views/About.vue')
  },
  {
    path: '/private',
    name: 'Private',
    component: Private
  },
  {
    path: '/unauthorized',
    name: 'Unauthorized',
    component: Unauthorized
  },
  {
    path: '/CallBack',
    name: 'CallBack',
    component: CallBack,
    meta: {
      notRequiresAuth: true
    }
  },
  {
    path: '/SilentCallback',
    name: 'SilentCallback',
    component: SilentCallback,
    meta: {
      notRequiresAuth: true
    }
  }
]

const router = new VueRouter({
  mode: 'history',
  routes
})

// 挂载路由导航守卫
router.beforeEach((to, from, next) => {
  // to 将要访问的路径
  // from 代表从哪个路径跳转而来
  // next 是一个函数，表示放行
  //     next()  放行    next('/login')  强制跳转

  const notRequiresAuth = to.matched.some(record => record.meta.notRequiresAuth)
  console.log(notRequiresAuth)
  if (!notRequiresAuth) {
    mgr.getUser().then(
      user => {
        if (user === null) {
          mgr.getSignedIn().then(
            signIn => {
              console.log(signIn)
            })
        } else {
          next()
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
export default router
