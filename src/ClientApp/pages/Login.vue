<template>
  <div>
    <h1>{{ title }}</h1>
  </div>
</template>
<script lang="ts">
import { Vue, Component } from 'nuxt-property-decorator'

@Component({
  layout: 'gest'
})
export default class Login extends Vue {
  title: String = 'Login'
}
</script>

<style></style>
