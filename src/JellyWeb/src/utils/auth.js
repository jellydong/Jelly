import Mgr from '../services/SecurityService'
const mgr = new Mgr()

export async function getProfile() {
  return await mgr.getProfile()
}
