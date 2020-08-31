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
        <v-list-item>
          <v-list-item-content>
            <v-list-item-title class="title">{{$auth.user.userName}}</v-list-item-title>
            <v-list-item-subtitle>{{ $auth.user.name }} {{ $auth.user.lastName }} {{ $auth.user.isAdmin ? '(admin)':'' }}</v-list-item-subtitle>
          </v-list-item-content>
        </v-list-item>
        <v-divider />
        <nuxt-link to="/">
          <v-list-item>
            <v-list-item-action>
              <v-icon>mdi-home</v-icon>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-title>Home</v-list-item-title>
            </v-list-item-content>
          </v-list-item>
        </nuxt-link>
        <nuxt-link to="/">
          <v-list-item>
            <!-- <v-list-item-action>
              <v-icon>mdi-home</v-icon>
            </v-list-item-action> -->
            <v-list-item-content>
              <v-list-item-title>LEW Licensing</v-list-item-title>
            </v-list-item-content>
          </v-list-item>
        </nuxt-link>
         <nuxt-link to="/">
          <v-list-item>
            <v-list-item-action>
              <v-icon small>mdi-file</v-icon>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-title>Reporting</v-list-item-title>
            </v-list-item-content>
          </v-list-item>
        </nuxt-link>
          <v-list-item>
            <v-list-item-content>
              <v-list-item-title>Risk Assessment</v-list-item-title>
            </v-list-item-content>
          </v-list-item>
          <v-list-item>
            <v-list-item-content>
              <v-list-item-title>Method of Statement</v-list-item-title>
            </v-list-item-content>
          </v-list-item>
          <v-list-item>
            <v-list-item-content>
              <v-list-item-title>Manpower Scheduling</v-list-item-title>
            </v-list-item-content>
          </v-list-item>
          <v-list-item v-if="$auth.user.isAdmin">
            <v-list-item-content @click="$router.push('master')">
              <v-list-item-title>Master Setup</v-list-item-title>
            </v-list-item-content>
          </v-list-item>
      </v-list>
    </v-navigation-drawer>

    <v-app-bar
      app
      color="indigo"
      dark
    >
      <v-app-bar-nav-icon @click.stop="drawer = !drawer" />
      <v-btn v-if="canGoBack" icon @click.stop="$router.go(-1)">
          <v-icon>mdi-arrow-left</v-icon>
      </v-btn>
      <v-toolbar-title>Reporting</v-toolbar-title>
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
        class="v-btn--example"
        @click="$vuetify.goTo(0)"
      >
        <v-icon>mdi-chevron-up</v-icon>
      </v-btn>
    </v-fab-transition>
            <!-- <v-speed-dial
              v-model="fab"
              bottom
              right
              direction="top"
              open-on-hover
              transition="slide-y-reverse-transition"
            >
              <template v-slot:activator>
                <v-btn
                  v-model="fab"
                  color="success"
                  dark
                  fixed
                  fab
                >
                  <v-icon v-if="fab">mdi-close</v-icon>
                  <v-icon v-else>mdi-content-save</v-icon>
                </v-btn>
              </template>
             <v-btn
                fab
                dark
                small
                color="indigo"
              >
                <v-icon>mdi-plus</v-icon>
              </v-btn>
              <v-btn
                fab
                dark
                small
                color="red"
              >
                <v-icon>mdi-delete</v-icon>
              </v-btn>
            </v-speed-dial> -->
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

  onScroll(e: any) {
    if (typeof window === 'undefined') return
      const top = window.pageYOffset ||   e.target.scrollTop || 0
      this.showScrollUpFab = top > 20
  }

  async logout() {
    await this.$auth.logout()
  }

  get canGoBack() {
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
