<template>
  <input
    ref="textbox"
    v-model="dataValue"
    type="text"
    class="text-caption table-input"
    :data-col="col"
    @click="onClick"
    @keydown="keyDown"
    @focusout="onExit"
  />
</template>

<script>
import { EventBus, Key } from '../../utils/costsheets/event_bus.js'

export default {
  props: {
    value: {
      required: true,
    },
    col: Number,
  },
  data: () => {
    return {
      editing: false,
    }
  },
  computed: {
    dataValue: {
      get() {
        return this.value
      },
      set(v) {
        this.editing = true
        this.$emit('input', v)
      },
    },
  },
  methods: {
    keyDown(event) {
      if (event.keyCode === Key.up) {
        EventBus.$emit('move', this.$refs.textbox, Key.up)
      } else if (event.keyCode === Key.down) {
        EventBus.$emit('move', this.$refs.textbox, Key.down)
      } else if (event.keyCode === Key.enter) {
        EventBus.$emit('move', this.$refs.textbox, Key.enter)
      } else if (!this.editing) {
        if (event.keyCode === Key.left) {
          EventBus.$emit('move', this.$refs.textbox, Key.left)
        } else if (event.keyCode === Key.right) {
          EventBus.$emit('move', this.$refs.textbox, Key.right)
        } else {
          this.editing = true
        }
      }
    },
    onClick() {
      this.editing = true
    },
    onExit() {
      if (this.$refs.textbox !== document.activeElement) {
        this.editing = false
      }
    },
  },
}
</script>
