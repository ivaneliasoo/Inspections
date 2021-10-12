<template>
  <v-app id="inspire">
    <v-navigation-drawer
      v-model="drawer"
      expand-on-hover
      :mini-variant="!$device.isMobile"
      clipped
      app
      overflow
    >
      <Menu v-if="$auth.user" />
    </v-navigation-drawer>

    <v-app-bar
      app
      color="indigo"
      clipped-left
      dense
      dark
    >
      <v-app-bar-nav-icon @click.stop="drawer = !drawer" />
      <v-btn v-if="canGoBack" icon @click.stop="$router.go(-1)">
        <v-icon>mdi-arrow-left</v-icon>
      </v-btn>
      <v-toolbar-title>Reporting</v-toolbar-title>
      <v-spacer />
      <img src="/client/Logo.jpeg" height="30px" width="110px">
      <v-btn icon @click="logout">
        <v-icon>mdi-logout</v-icon>
      </v-btn>
    </v-app-bar>
    <v-fab-transition>
      <v-btn
        v-show="showScrollUpFab"
        color="primary"
        fab
        fixed
        dark
        bottom
        right
        class="v-btn--example"
        @click="$vuetify.goTo(0)"
      >
        <v-icon>mdi-chevron-up</v-icon>
      </v-btn>
    </v-fab-transition>
    <v-main>
      <v-container
        fluid
      >
        <v-row
          align="center"
          justify="center"
        >
          <v-col class="text-center">
            <nuxt v-scroll="onScroll" />
          </v-col>
        </v-row>
      </v-container>
    </v-main>
    <v-footer
      v-show="false"
      color="indigo"
      app
    >
      <span class="white--text">&copy; 2020</span>
    </v-footer>
  </v-app>
</template>
<script lang="ts">
import { Vue, Component } from 'nuxt-property-decorator'
import { RootState } from '@/store'

@Component
export default class Default extends Vue {
  drawer: boolean | null = null
  mini: boolean = false
  showScrollUpFab: boolean = false
  fab: boolean = false

  onScroll (e: any) {
    if (typeof window === 'undefined') { return }
    const top = window.pageYOffset || e.target.scrollTop || 0
    this.showScrollUpFab = top > 20
  }

  async logout () {
    await this.$auth.logout()
  }

  get canGoBack () {
    return (this.$store.state as RootState).canGoBack
  }
}
</script>
<style scoped>
.v-btn--example {
    bottom: 0;
    margin: 0 0 16px 16px;
  }

</style>
