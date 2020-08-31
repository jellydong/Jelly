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
    redirect: '/home'
  },
  {
    path: '/home',
    name: 'Home',
    component: Home,
    meta: {
      requiresAuth: false
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
      requiresAuth: false
    }
  },
  {
    path: '/SilentCallback',
    name: 'SilentCallback',
    component: SilentCallback
  }
]

const router = new VueRouter({
  mode: 'history',
  routes
})

export default router
