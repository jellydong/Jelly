import request from '../utils/request'

export function getUserInfo() {
  return request({
    url: '/identitys',
    method: 'get'
  })
}
export function getValues() {
  return request({
    url: '/values',
    method: 'get'
  })
}
