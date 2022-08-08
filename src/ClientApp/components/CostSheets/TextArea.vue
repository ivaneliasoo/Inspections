<template>
  <div>
    <div
      v-if="viewMode"
      class="text-caption text-left table-input text-box"
      @click="onClick"
    >
      {{ dataValue }}
      <input
        ref="navbox"
        type="text"
        size="1"
        readonly
        :data-col="col"
        @keydown="keyDown"
      />
    </div>
    <textarea
      v-else
      ref="textbox"
      v-model="dataValue"
      class="text-caption table-input"
      style="width: 200px; padding: 0"
      @focusout="onExit"
    />
  </div>
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
    viewMode() {
      return !this.editing && this.dataValue && this.dataValue.length > 0
    },
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
  updated() {
    if (this.editing) {
      const tx = this.$refs.textbox
      tx.focus()
    }
  },
  methods: {
    keyDown(event) {
      if (event.keyCode === Key.up) {
        EventBus.$emit('move', this.$refs.navbox, Key.up)
      } else if (event.keyCode === Key.down) {
        EventBus.$emit('move', this.$refs.navbox, Key.down)
      } else if (event.keyCode === Key.left) {
        EventBus.$emit('move', this.$refs.navbox, Key.left)
      } else if (event.keyCode === Key.right) {
        EventBus.$emit('move', this.$refs.navbox, Key.right)
      } else if (event.keyCode === Key.enter) {
        EventBus.$emit('move', this.$refs.navbox, Key.enter)
      } else {
        this.editing = true
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

<style scoped>
.text-box {
  width: 200px;
  white-space: normal;
  line-height: 15px;
  padding-top: 2px;
  padding-bottom: 2px;
}

.text-box:focus-within {
  border: 2px solid black;
  border-radius: 5px;
}

.focus-handler {
  border-bottom: 2px solid black;
}

input {
  border: 0;
  outline: 0;
}

input:focus {
  outline: none !important;
  border-left: 1px solid black;
  animation-name: blinking;
  animation-duration: 1s;
  animation-iteration-count: 100;
}
@keyframes blinking {
  50% {
    border-color: white;
  }
}
</style>
