<template>
  <v-app id="inspire">
    <v-navigation-drawer
      v-model="drawer"
      color="indigo"
      expand-on-hover
      mini-variant
      app
      dark
    >
      <v-list dense>
        <v-list-item class="px-2">
          <v-list-item-avatar>
            <v-img src="https://randomuser.me/api/portraits/men/85.jpg" />
          </v-list-item-avatar>
        </v-list-item>
        <v-list-item link>
          <v-list-item-content>
            <v-list-item-title class="title">Demo User</v-list-item-title>
            <v-list-item-subtitle>user@gmail.com</v-list-item-subtitle>
          </v-list-item-content>
        </v-list-item>
        <v-divider />
        <nuxt-link to="/">
          <v-list-item link>
            <v-list-item-action>
              <v-icon>mdi-home</v-icon>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-title>Home</v-list-item-title>
            </v-list-item-content>
          </v-list-item>
        </nuxt-link>
        <nuxt-link to="/reports">
          <v-list-item link>
            <v-list-item-action>
              <v-icon>mdi-file-chart</v-icon>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-title>Reports</v-list-item-title>
            </v-list-item-content>
          </v-list-item>
        </nuxt-link>
        <nuxt-link to="/checklists">
          <v-list-item link>
            <v-list-item-action>
              <v-icon>mdi-format-list-checks</v-icon>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-title>CheckLists</v-list-item-title>
            </v-list-item-content>
          </v-list-item>
        </nuxt-link>
        <nuxt-link to="/signatures">
          <v-list-item link>
            <v-list-item-action>
              <v-icon>mdi-draw</v-icon>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-title>Signatures</v-list-item-title>
            </v-list-item-content>
          </v-list-item>
        </nuxt-link>
        <nuxt-link to="/configurations">
          <v-list-item link>
            <v-list-item-action>
              <v-icon>mdi-cog-outline</v-icon>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-title>Reports Settings</v-list-item-title>
            </v-list-item-content>
          </v-list-item>
        </nuxt-link>
      </v-list>
    </v-navigation-drawer>

    <v-app-bar
      app
      color="indigo"
      dark
    >
      <v-app-bar-nav-icon @click.stop="drawer = !drawer" />
      <v-toolbar-title>Inspections</v-toolbar-title>
      <v-spacer />
      <img src="/Logo.jpeg" height="50px" width="130px">
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
        small
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
            <nuxt v-scroll="onScroll"/>
          </v-col>
        </v-row>
      </v-container>
    </v-main>
    <v-footer
      color="indigo"
      app
    >
      <span class="white--text">&copy; 2020</span>
    </v-footer>
  </v-app>
</template>
<script lang="ts">
import { Vue, Component } from 'nuxt-property-decorator'

@Component
export default class Default extends Vue {
  drawer: boolean | null = null
  mini: boolean = false
  showScrollUpFab: boolean = false

  onScroll(e: any) {
    if (typeof window === 'undefined') return
      const top = window.pageYOffset ||   e.target.scrollTop || 0
      this.showScrollUpFab = top > 20
  }

  async logout() {
    await this.$auth.logout()
  }
}
</script>
<style scoped>
.v-btn--example {
    bottom: 0;
    margin: 0 0 16px 16px;
  }

</style>
