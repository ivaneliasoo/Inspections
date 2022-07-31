import { Plugin } from '@nuxt/types'
import { Configuration } from '../services/api/configuration'
import { FormsApi, ReportConfigurationApi, ReportsApi } from '@/services/api'

const apiClient: Plugin = ({ $auth, $config }, inject) => {
  if (!$auth?.user) { return }
  const config: Configuration = {
    // @ts-ignore
    accessToken: `${$auth.strategy.token.get().replace('bearer ', '')}`,
    basePath: $config.axios.baseURL.replace('/api', ''),
    isJsonMime: () => false
  }
  const reportsApi = new ReportsApi(config)
  const formsApi = new FormsApi(config)
  const reportsConfigApi = new ReportConfigurationApi(config)

  inject('reportsApi', reportsApi)
  inject('formsApi', formsApi)
  inject('reportsConfigApi', reportsConfigApi)
}

export default apiClient
