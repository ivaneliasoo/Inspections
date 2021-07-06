import env from 'react-native-config';

const config = {
  reportsApi: {
    host: env.API_HOST,
    key: env.API_KEY,
  },
};

const API_HOST = config.reportsApi.host;
const API_KEY = config.reportsApi.key;
const API_CONFIG = {basePath: API_HOST, apiKey: API_KEY};

export {API_HOST, API_KEY, API_CONFIG};

export default config;
