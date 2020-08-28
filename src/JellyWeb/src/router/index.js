import Vue from 'vue'
import VueRouter from 'vue-router'
import Home from '../views/Home.vue'
import Private from '../views/Private.vue'
import Unauthorized from '../views/Unauthorized.vue'
import CallBack from '../components/oidc/CallBack.vue'
import SilentCallback from '../components/oidc/SilentCallback.vue'

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home
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
    component: CallBack
  },
  {
    path: '/SilentCallback',
    name: 'SilentCallback',
    component: SilentCallback
  }
]

const router = new VueRouter({
  routes
})

// 挂载路由导航守卫
router.beforeEach((to, from, next) => {
  // to 将要访问的路径
  // from 代表从哪个路径跳转而来
  // next 是一个函数，表示放行
  //     next()  放行    next('/login')  强制跳转

  if (to.path !== '/' || to.path !== '/home') {
    return next()
  }
  // 获取token
  const tokenStr = window.sessionStorage.getItem('token')
  if (!tokenStr) {
    return next('/unauthorized')
  }
  next()
})

export default router
