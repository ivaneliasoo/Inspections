export default {
  ssr: false,
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
    '@/assets/main.css'
  ],
  /*
  ** Plugins to load before mounting the App
  */
  plugins: [
    '~/plugins/vee-validate',
    '~/plugins/vue-signature-pad'
  ],
  /*
  ** Nuxt.js dev-modules
  */
  buildModules: [
    '@nuxt/typescript-build',
    '@nuxtjs/vuetify',
    '@nuxtjs/tailwindcss',
    '@nuxt/image'
  ],
  /*
  ** Nuxt.js modules
  */
  modules: [
    // Doc: https://axios.nuxtjs.org/usage
    '@nuxtjs/axios',
    '@nuxtjs/pwa',
    '@nuxtjs/auth',
    '@nuxtjs/device',
    ['nuxt-compress',
      {
        gzip: {
          cache: true
        },
        brotli: {
          threshold: 10240
        }
      }
    ]
  ],
  pwa: {
    icon: {
      /* icon options */
    }
  },
  components: true,
  /*
  ** Axios module configuration
  ** See https://axios.nuxtjs.org/options
  */
  // axios: {
  // },
  publicRuntimeConfig: {
    axios: {
      baseURL: process.env.BASE_URL || 'http://localhost:5000/api',
      browserBaseURL: process.env.BASE_URL || 'http://localhost:5000/api'
    }
  },
  auth: {
    redirect: {
      login: '/Login',
      logout: '/Login',
      home: '/Reports'
    },
    cookie: {
      options: {
        sameSite: 'lax'
      }
    },
    strategies: {
      local: {
        endpoints: {
          login: { url: '/auth/token', method: 'post', propertyName: false },
          logout: false,
          user: { url: '/Users/active', method: 'get', propertyName: false }
        },
        tokenRequired: true,
        tokenType: 'bearer',
        autoFetchUser: true
      }
    },
    plugins: ['~/plugins/api-client']
  },
  router: {
    middleware: ['auth']
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
    transpile: ['vee-validate/dist/rules', 'vuex-module-decorators', '@nuxtjs/auth', 'q'],
    terser: false
  }
}
