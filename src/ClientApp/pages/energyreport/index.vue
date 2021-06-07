<template>
  <v-app>
    <v-app-bar
      app
      color="primary"
      dark
    >
    </v-app-bar>
    <v-main>
      <EnergyReport/>
    </v-main>
  </v-app>
</template>

<script>
import EnergyReport from './components/EnergyReport';

export default {
  name: 'App',

  components: {
    EnergyReport,
  },

  data: () => ({
    //
  }),
};
</script>
