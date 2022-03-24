<template>
  <v-list dense>
    <v-list-item class="px-2">
      <v-list-item-avatar>
        <v-icon large color="indigo">mdi-account-circle</v-icon>
      </v-list-item-avatar>
      <v-list-item-content>
        <v-list-item-title class="text-uppercase">{{$auth.user.userName}}</v-list-item-title>
        <v-list-item-subtitle class="text-uppercase">{{ $auth.user.name }} {{ $auth.user.lastName }} {{ $auth.user.isAdmin ? '(admin)':'' }}</v-list-item-subtitle>
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
      v-for="(menu, index) in menuDefinition.filter(m=>m.parentName==='')"
      :key="`${menu.name}${index}`"
      link
      :to="menu.route.includes('http') ? '' : menu.route"
      @click="menu.route.includes('http') ? redirect(menu.route):''"
      :disabled="!menu.route || menu.disabled"
    >
      <v-list-item-icon>
        <v-icon v-if="menu.icon" v-text="menu.icon"></v-icon>
        <span class="font-weight-black text-uppercase" v-else>{{ initials(menu) }}</span>
      </v-list-item-icon>
      <v-list-item-content>
        <v-list-item-title v-text="menu.name" />
      </v-list-item-content>
    </v-list-item>
  </v-list>
</template>

<script lang="ts">
import { Vue, Component } from "nuxt-property-decorator";

@Component
export default class Menu extends Vue {
  menuDefinition: any[] = [
    {
      name: "LEW Licensing",
      parentName: "",
      route: "/lewlicensing",
      icon: "",
    },
    {
      name: "Reporting",
      parentName: "",
      route: "/",
      icon: "mdi-file",
    },
    {
      name: 'Energy Report',
      parentName: '',
      route: '/energyreport',
      icon: 'mdi-graph',
      disabled: this.$auth.user && this.$auth.user.userName == 'reports' ? true : false,
    },
    {
      name: 'Job Planner',
      parentName: '',
      route: '/jobplanner',
      icon: 'mdi-calendar',
      disabled: this.$auth.user && this.$auth.user.userName == 'reports' ? true : false,
    },
    {
      name: 'Cost Sheets',
      parentName: '',
      route: '/costsheets',
      icon: 'mdi-table-large',
      disabled: this.$auth.user && this.$auth.user.userName == 'reports' ? true : false,
    },
    {
      name: "Risk Assessment",
      parentName: "",
      route: "",
      icon: "",
    },
    {
      name: "Method of Statement",
      parentName: "",
      route: "",
      icon: "",
    },
    {
      name: "Manpower Scheduling",
      parentName: "",
      route: "",
      icon: "",
      disabled: this.$auth.user && this.$auth.user.userName == 'reports' ? true : false,
    },
    {
      name: "Master Setup",
      parentName: "",
      route: "/master",
      icon: "mdi-cog",
    },
  ];

  get defaultPage() {
    return "/";
  }

  initials(menu: any) {
    const words = menu.name.split(' ')
    return words[0][0] + words[1][0]
  }

  redirect(route: string) {
    window.open(route, '_blank')
  }
}
</script>

<style>
</style>
