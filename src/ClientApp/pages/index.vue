<template>
  <div>
    {{ name }}
  </div>
</template>
<script lang="ts">
import { Vue, Component } from 'nuxt-property-decorator'
@Component({
  layout: 'Default'
})
export default class Login extends Vue {
  name: String = ''
}
</script>
