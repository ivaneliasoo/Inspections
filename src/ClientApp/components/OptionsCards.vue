<template>
  <v-row>
    <v-col
      v-for="(option, index) in options"
      :key="index"
      cols="12"
      lg="4"
      md="4"
    >
      <v-card
        min-height="250"
        min-width="200"
        class="pt-10"
        :ripple="option.innerActions ? false : true"
        @click="option.innerActions ? 0 : onCardClick(option)"
      >
        <v-btn
          v-if="!option.innerActions || !option.icon"
          fab
          :class="option.color"
          dark
        >
          <v-icon>
            {{ option.icon }}
          </v-icon>
        </v-btn>
        <v-card-title class="text-center">
          <v-row>
            <v-col class="text-center">
              {{ option.text }}
            </v-col>
          </v-row>
        </v-card-title>
        <v-card-subtitle>
          <v-row>
            <v-col class="text-center">
              {{ option.helpText }}
            </v-col>
          </v-row>
        </v-card-subtitle>
        <v-card-actions>
          <v-row>
            <v-col
              v-for="(action, actionIndex) in option.innerActions"
              :key="actionIndex"
              cols="6"
            >
              <v-btn
                :color="action.color"
                tile
                :block="$vuetify.breakpoint.smAndDown"
                :disabled="
                  !$auth.user.lastEditedReport && action.text === 'Edit Last'
                "
                @click.stop="action.action()"
              >
                <v-icon>
                  {{ action.icon }}
                </v-icon>
                {{ action.text }}
              </v-btn>
            </v-col>
          </v-row>
        </v-card-actions>
      </v-card>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { useRouter, defineComponent } from '@nuxtjs/composition-api'
import { CardOption } from '~/types'

export default defineComponent({
  props: {
    options: {
      type: Array as () => CardOption[],
      default: () => [],
      required: true,
    },
  },
  setup() {
    const router = useRouter()
    const onCardClick = (option: CardOption) => {
      const path =
        typeof option.path === 'function' ? option.path() : option.path
      if (path) {
        router.push(path)
      } else if (option.action) {
        option.action()
      }
    }

    return {
      onCardClick,
    }
  },
})
</script>
