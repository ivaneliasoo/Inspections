<template>
  <h1>Users Administration</h1>
</template>
<script lang="ts">
import { Vue, Component, mixins } from 'nuxt-property-decorator'
import InnerPageMixin from '@/mixins/innerpage'

@Component
export default class UserAdmin extends mixins(InnerPageMixin) {
  
}
</script>