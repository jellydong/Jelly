import request from '../utils/request'

export function getUserInfo() {
  debugger
  return request({
    url: '/identity',
    method: 'get'
  })
}
