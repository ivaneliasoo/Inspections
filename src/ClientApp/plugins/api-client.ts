import { Plugin } from '@nuxt/types'
import { ReportsApi } from '@/services/api'
import { Configuration } from '../services/api/configuration'

const apiClient: Plugin = ({ $auth, $config }, inject) => {
  if (!$auth?.user) { return }
  const config: Configuration = {
    accessToken: `${$auth.getToken('local').replace('bearer ', '')}`,
    basePath: $config.axios.baseURL.replace('/api', ''),
    isJsonMime: () => false
  }
  const reportsApi = new ReportsApi(config)
  inject('reportsApi', reportsApi)
}

export default apiClient
