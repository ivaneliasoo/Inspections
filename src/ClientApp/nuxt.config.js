export default {
  ssr: false,
  head: {
    title: process.env.npm_package_name || '',
    meta: [
      { charset: 'utf-8' },
      { name: 'viewport', content: 'width=device-width, initial-scale=1' },
      {
        hid: 'description',
        name: 'description',
        content: process.env.npm_package_description || '',
      },
    ],
    link: [{ rel: 'icon', type: 'image/x-icon', href: '/favicon.ico' }],
  },
  /*
   ** Customize the progress-bar color
   */
  loading: { color: '#fff' },
  /*
   ** Global CSS
   */
  css: ['@/assets/main.css'],
  /*
   ** Plugins to load before mounting the App
   */
  plugins: ['~/plugins/vee-validate', '~/plugins/vue-signature-pad'],
  /*
   ** Nuxt.js dev-modules
   */
  buildModules: [
    '@nuxt/typescript-build',
    '@nuxtjs/stylelint-module',
    '@nuxtjs/composition-api/module',
    '@nuxtjs/vuetify',
    '@nuxt/image',
    '@nuxtjs/auth-next',
    '@nuxt/postcss8',
    '@nuxtjs/pwa',
    '@nuxtjs/device',
    '@braid/vue-formulate/nuxt',
    'vue2-editor/nuxt',
    ['@pinia/nuxt', { disableVuex: false }],
  ],
  /*
   ** Nuxt.js modules
   */
  modules: [
    // Doc: https://axios.nuxtjs.org/usage
    '@nuxtjs/axios',
    '@vueuse/nuxt',
  ],
  pwa: {
    icon: {
      /* icon options */
    },
  },
  components: [
    '~/components',
    '~/components/JobScheduler',
    '~/components/CostSheets',
  ],
  /*
   ** Axios module configuration
   ** See https://axios.nuxtjs.org/options
   */
  // axios: {
  // },
  publicRuntimeConfig: {
    axios: {
      baseURL: `${process.env.BASE_URL}/api` || 'http://localhost:5000/api',
      browserBaseURL:
        `${process.env.BASE_URL}/api` || 'http://localhost:5000/api',
    },
  },
  auth: {
    localStorage: false,
    redirect: {
      login: '/Login',
      logout: '/Login',
      home: '/',
    },
    cookie: {
      prefix: 'auth.',
      options: {
        sameSite: 'lax',
      },
    },
    token: {
      prefix: '_token.',
      global: true,
    },
    strategies: {
      local: {
        endpoints: {
          login: { url: '/auth/token', method: 'post' },
          logout: false,
          user: { url: '/Users/active', method: 'get' },
        },
        user: {
          property: false,
          autofecth: true,
        },
        tokenRequired: true,
        tokenType: 'bearer',
      },
    },
    plugins: ['~/plugins/api-client'],
  },
  router: {
    middleware: ['auth'],
  },
  /*
   ** vuetify module configuration
   ** https://github.com/nuxt-community/vuetify-module
   */
  vuetify: {
    // customVariables: ['~/assets/variables.scss'],
    optionsPath: './vuetify.options.js',
  },
  /*
   ** Build configuration
   */
  build: {
    postcss: {
      plugins: {
        tailwindcss: {},
        autoprefixer: {},
      },
    },
    transpile: [
      'vee-validate/dist/rules',
      'vuex-module-decorators',
      '@nuxtjs/auth',
      'q',
    ],
    terser: false,
    babel: {
      plugins: [
        ['@babel/plugin-proposal-class-properties', { loose: true }],
        ['@babel/plugin-proposal-private-methods', { loose: true }],
        ['@babel/plugin-proposal-private-property-in-object', { loose: true }],
      ],
    },
  },
}
