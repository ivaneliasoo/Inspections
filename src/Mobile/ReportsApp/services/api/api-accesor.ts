import axios from 'axios';
import {API_HOST, API_KEY} from '../../config/config';

const api = {
  zaderp: () => {
    return axios.create({
      baseURL: `${API_HOST}/api`,
      headers: {'X-Api-Key': `ApiKey ${API_KEY}`},
    });
  },
};

const apiZaderp = api.zaderp();

export {apiZaderp, API_HOST, API_KEY};

export default api;