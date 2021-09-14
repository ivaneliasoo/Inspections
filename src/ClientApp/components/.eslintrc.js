module.exports = {
    root: true,
    env: {
        browser: true,
        node: true
    },
    extends: [
        '@nuxtjs/eslint-config-typescript',
        'plugin:next/recommended'
    ],
    rules: {
        'semi': [2, 'never'],
        'vue/valid-v-slot': ['error', {
            allowModifiers: true
        }]
    }
}