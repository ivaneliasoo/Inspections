require('dotenv').config()

export default {
  mode: 'spa',
  /*
  ** Headers of the page
  */
  head: {
    title: process.env.npm_package_name || '',
    meta: [
      { charset: 'utf-8' },
      { name: 'viewport', content: 'width=device-width, initial-scale=1' },
      { hid: 'description', name: 'description', content: process.env.npm_package_description || '' }
    ],
    link: [
      { rel: 'icon', type: 'image/x-icon', href: '/favicon.ico' }
    ]
  },
  /*
  ** Customize the progress-bar color
  */
  loading: { color: '#fff' },
  /*
  ** Global CSS
  */
  css: [
  ],
  /*
  ** Plugins to load before mounting the App
  */
  plugins: [
    '~/plugins/vee-validate'
  ],
  /*
  ** Nuxt.js dev-modules
  */
  buildModules: [
    '@nuxt/typescript-build',
    '@nuxtjs/vuetify',
    // Doc: https://github.com/nuxt-community/dotenv-module
    '@nuxtjs/dotenv'
  ],
  /*
  ** Nuxt.js modules
  */
  modules: [
    // Doc: https://axios.nuxtjs.org/usage
    '@nuxtjs/axios',
    '@nuxtjs/pwa',
    '@nuxtjs/auth',
  ],
  /*
  ** Axios module configuration
  ** See https://axios.nuxtjs.org/options
  */
  axios: {
      baseURL: 'https://localhost:44388',
      browserBaseURL: 'https://localhost:44388'
  },
  router: {
    middleware: ['auth']
  },
  auth: {
    redirect: {
      login: '/Login',
      logout: '/Login',
      home: false
    },
    cookie: true,
    strategies: {
      local: {
        endpoints: {
          login: { url: '/auth/token', method: 'post', propertyName: false },
          user: { url: '/Users/active', method: 'get', propertyName: false }
        },
        tokenRequired: true,
        tokenType: 'bearer',
        autoFetchUser: true
      }
    }
  },
  /*
     ** vuetify module configuration
     ** https://github.com/nuxt-community/vuetify-module
     */
  vuetify: {
    // customVariables: ['~/assets/variables.scss'],
    optionsPath: './vuetify.options.js'
  },
  /*
  ** Build configuration
  */
  build: {
    /*
    ** You can extend webpack config here
    */
    extend (config, ctx) {
    },
    // Excepcion para vee-validate (no quitar)
    transpile: ['vee-validate/dist/rules', 'vuex-module-decorators', '@nuxtjs/auth'],
  }
}
