import Mgr from '../services/SecurityService'
const mgr = new Mgr()

export function getProfile() {
  var user = mgr.getProfile().then(res => {
    return res
  })
  return user
}
