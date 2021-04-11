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
    'semi': [2, 'never'],
    'vue/comment-directive': ['error', {
      'reportUnusedDisableDirectives': false
    }]
  }
}
