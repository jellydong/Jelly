import axios from 'axios'
import { message } from 'ant-design-vue'

// 导入 NProgress 包对应的JS和CSS
import NProgress from 'nprogress'
import 'nprogress/nprogress.css'

import Mgr from '../services/SecurityService'
const mgr = new Mgr()

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
  async config => {
    // 在请求发送之前需要处理的代码
    await mgr.getAcessToken().then(accessToken => {
      // 让每个请求携带令牌
      config.headers.Authorization = `Bearer ${accessToken}`
    })
    // 在 request 拦截器中，展示进度条 NProgress.start()
    NProgress.start()
    // 在最后必须 return config
    return config
  },
  error => {
    // 请求错误
    NProgress.done()
    console.log(error) // for debug
    message.error({ content: error.message, duration: 2 })
    return Promise.reject(error) // 错误提示
  }
)

// 响应拦截器
service.interceptors.response.use(
  response => {
    // 在 response 拦截器中，隐藏进度条 NProgress.done()
    NProgress.done()
    const res = response.data
    // 这里可以根据后台统一返回的进行判断处理 比如 返回的success如果是失败，直接这里提示就可以了
    return res
  },
  error => {
    NProgress.done()
    console.log('err' + error) // for debug
    switch (error.response.status) {
      case 500:
        message.error({ content: error.response.data.message, duration: 2 })
        break
      case 401:
        message.error({ content: '未授权', duration: 2 })
        break
      case 403:
        message.error({ content: '无权限访问', duration: 2 })
        break
      case 404:
        message.error({ content: '请求地址不存在', duration: 2 })
        break
      case 400:
        var errorMsg = error.response.data.error_description || error.response.data.message || error.response.data[0].description
        if (errorMsg) {
          message.error({ content: errorMsg, duration: 2 })
        }
        break
      default:
        message.error({ content: '未知异常联系管理员', duration: 2 })
        break
    }
    return Promise.reject(error)
  }
)

export default service
