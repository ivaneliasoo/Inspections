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
  excludes:[
    "**/node_modules/**"
  ],
  // add your custom rules here
  rules: {
  }
}
