import colors from 'vuetify/es5/util/colors'
export default {
  treeShake: true,
  theme: {
    dark: false,
    themes: {
      dark: {
        primary: colors.indigo.darken2,
        accent: colors.grey.darken3,
        secondary: colors.amber.darken3,
        info: colors.teal.lighten1,
        warning: colors.amber.base,
        error: colors.deepOrange.accent4,
        success: colors.green.accent3
      }
    }
  }
}
