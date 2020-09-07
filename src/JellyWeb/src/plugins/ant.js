import Vue from 'vue'

import {
  message,
  Modal
} from 'ant-design-vue'

Vue.use(Modal)

Vue.prototype.$message = message
Vue.prototype.$info = Modal.info
Vue.prototype.$success = Modal.success
Vue.prototype.$error = Modal.error
Vue.prototype.$warning = Modal.warning
Vue.prototype.$confirm = Modal.confirm
Vue.prototype.$destroyAll = Modal.destroyAll
