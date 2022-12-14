<template>
  <v-list dense>
    <v-list-item class="px-2">
      <v-list-item-avatar>
        <v-icon large color="indigo"> mdi-account-circle </v-icon>
      </v-list-item-avatar>
      <v-list-item-content>
        <v-list-item-title class="text-uppercase">
          {{ $auth.user.userName }}
        </v-list-item-title>
        <v-list-item-subtitle class="text-uppercase">
          {{ $auth.user.name }} {{ $auth.user.lastName }}
          {{ $auth.user.isAdmin ? '(admin)' : '' }}
        </v-list-item-subtitle>
      </v-list-item-content>
    </v-list-item>
    <v-list-item :to="defaultPage" link>
      <v-list-item-icon>
        <v-icon>mdi-view-dashboard</v-icon>
      </v-list-item-icon>
      <v-list-item-title>Home</v-list-item-title>
    </v-list-item>
    <v-divider />
    <v-list-item
      v-for="(menu, index) in menuDefinition.filter(
        (m) => m.parentName === '' && !m.visible
      )"
      :key="`${menu.name}${index}`"
      link
      :to="menu.route.includes('http') ? '' : menu.route"
      :visible="!menu.route || menu.visible"
      @click="menu.route.includes('http') ? redirect(menu.route) : ''"
    >
      <v-list-item-icon>
        <v-icon v-if="menu.icon">
          {{ menu.icon }}
        </v-icon>
        <span v-else class="font-weight-black text-uppercase">{{
          initials(menu)
        }}</span>
      </v-list-item-icon>
      <v-list-item-content>
        <v-list-item-title>
          {{ menu.name }}
        </v-list-item-title>
      </v-list-item-content>
    </v-list-item>
  </v-list>
</template>

<script lang="ts">
import { useContext, defineComponent } from '@nuxtjs/composition-api'

export default defineComponent({
  setup() {
    const { $auth } = useContext()
    const menuDefinition: any[] = [
      {
        name: 'LEW Licensing',
        parentName: '',
        route: '/lewlicensing',
        icon: ''
      },
      {
        name: 'Reporting',
        parentName: '',
        route: '/',
        icon: 'mdi-file'
      },
      {
        name: 'Power Analyzer Report',
        parentName: '',
        route: '/energyreport',
        icon: 'mdi-graph',
        visible: !!(
          $auth.user &&
          ($auth.user.userName === 'reports' ||
            $auth.user.userName === 'leehanpin')
        )
      },
      {
        name: 'Job Planner',
        parentName: '',
        route: '/jobplanner',
        icon: 'mdi-calendar',
        visible: !!(
          $auth.user &&
          ($auth.user.userName === 'reports' ||
            $auth.user.userName === 'leehanpin')
        )
      },
      {
        name: 'Cost Sheets',
        parentName: '',
        route: '/costsheets',
        icon: 'mdi-table-large',
        visible: !!(
          $auth.user &&
          ($auth.user.userName === 'reports' ||
            $auth.user.userName === 'leehanpin')
        )
      },
      {
        name: 'Risk Assessment',
        parentName: '',
        route: '',
        icon: '',
        visible: !!(
          $auth.user &&
          ($auth.user.userName === 'reports' ||
            $auth.user.userName === 'leehanpin')
        )
      },
      {
        name: 'Method of Statement',
        parentName: '',
        route: '',
        icon: '',
        visible: !!(
          $auth.user &&
          ($auth.user.userName === 'reports' ||
            $auth.user.userName === 'leehanpin')
        )
      },
      {
        name: 'Manpower Scheduling',
        parentName: '',
        route: '',
        icon: '',
        visible: !!(
          $auth.user &&
          ($auth.user.userName === 'reports' ||
            $auth.user.userName === 'leehanpin')
        )
      },
      {
        name: 'Master Setup',
        parentName: '',
        route: '/master',
        icon: 'mdi-cog'
      }
    ]

    const defaultPage = '/'

    const initials = (menu: any) => {
      const words = menu.name.split(' ')
      return words[0][0] + words[1][0]
    }

    const redirect = (route: string) => {
      window.open(route, '_blank')
    }

    return {
      menuDefinition,
      defaultPage,
      initials,
      redirect
    }
  }
})
</script>
