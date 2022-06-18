module.exports = {
  root: true,
  env: {
    browser: true,
    node: true
  },
  extends: [
    '@nuxtjs/eslint-config-typescript',
    'plugin:nuxt/recommended'
  ],
  rules: {
    semi: [2, 'never'],
    'vue/valid-v-slot': ['error', {
      allowModifiers: true
    }],
    'comma-dangle': 'off',
    quotes: ['error', 'single'],
  }
}
