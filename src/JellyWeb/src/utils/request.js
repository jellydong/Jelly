import axios from 'axios'
import { Message } from 'element-ui'
import store from '../store/index'
// 导入 NProgress 包对应的JS和CSS
import NProgress from 'nprogress'
import 'nprogress/nprogress.css'

// 创建一个axios实例
const service = axios.create({
  baseURL: process.env.VUE_APP_BASE_API, // url = base url + request url
  // withCredentials: true, //  //在跨域请求时发送cookie
  timeout: 5000, // 请求超时时间
  headers: {
    'Content-Type': 'application/json;charset=UTF-8'
  }
})

// 请求拦截器
service.interceptors.request.use(
  config => {
    // 在请求发送之前需要处理的代码
    if (store.getters.accessToken) {
      // 让每个请求携带令牌
      config.headers.Authorization = `Bearer ${store.getters.accessToken}`
    }
    // 在 request 拦截器中，展示进度条 NProgress.start()
    NProgress.start()
    // 在最后必须 return config
    return config
  },
  error => {
    // 请求错误
    console.log(error) // for debug
    return Promise.reject(error) // 错误提示
  }
)

// 响应拦截器
service.interceptors.response.use(
  response => {
    // 在 response 拦截器中，隐藏进度条 NProgress.done()
    NProgress.done()

    if (response.status !== 200) {
      const res = response.data
      if (!res.Success) {
        Message({
          message: res.Message || 'Error',
          type: 'error',
          duration: 5 * 1000
        })
      } else {
        return res
      }
    } else {
      switch (response.status) {
        case 500:
          Message.error({ showClose: true, message: response.data.message })
          break
        case 401:
          Message.error({ showClose: true, message: '未授权' })
          break
        case 403:
          Message.error({ showClose: true, message: '无权限访问' })
          break
        case 404:
          Message.error({ showClose: true, message: '请求地址不存在' })
          break
        case 400:
          var errorMsg = response.data.error_description || response.data.message || response.data[0].description
          if (errorMsg) {
            Message.error({ showClose: true, message: errorMsg })
          }
          break
        default:
          Message.error({ showClose: true, message: '未知异常联系管理员' })
          break
      }
    }
  },
  error => {
    console.log('err' + error) // for debug
    Message({
      message: error.message,
      showClose: true,
      type: 'error',
      duration: 5 * 1000
    })
    return Promise.reject(error)
  }
)

export default service
