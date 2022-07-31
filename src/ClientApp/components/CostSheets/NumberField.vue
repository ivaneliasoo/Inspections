<template>
  <div>
    <label v-if="notNull() && format==='currency'" class="text-caption">$</label>
    <input
      ref="textbox"
      v-model="dataValue"
      type="text"
      class="text-caption text-bold table-input"
      :style="style"
      :data-col="col"
      @change="$emit('change', $event.target.value);"
      @focusout="onExit"
      @keydown="keyDown"
    >
    <label v-if="value" class="text-caption">{{ suffixStr }}</label>
  </div>
</template>

<script>
import { EventBus, Key } from '../../utils/costsheets/event_bus.js'

// Restricts input for the given textbox to the given inputFilter function.
// https://stackoverflow.com/questions/469357/html-text-input-allow-only-numeric-input
function setInputFilter (textbox, inputFilter) {
  ['input', 'keydown', 'keyup', 'mousedown', 'mouseup', 'select', 'contextmenu', 'drop'].forEach(function (event) {
    textbox.addEventListener(event, function () {
      if (inputFilter(this.value)) {
        this.oldValue = this.value
        this.oldSelectionStart = this.selectionStart
        this.oldSelectionEnd = this.selectionEnd
      } else if (this.hasOwnProperty('oldValue')) {
        this.value = this.oldValue
        this.setSelectionRange(this.oldSelectionStart, this.oldSelectionEnd)
      } else {
        this.value = ''
      }
    })
  })
}

export default {
  // components: { EventBus },
  props: {
    value: {
      required: true,
    },
    format: String,
    readOnly: Boolean,
    decimals: Number,
    suffix: String,
    bold: Boolean,
    col: Number
  },
  data: () => {
    return {
      editing: false
    }
  },
  computed: {
    suffixStr () {
      if (this.format === 'percent') {
        return '%'
      }
      return this.suffix ? this.suffix : ''
    },
    style () {
      let width
      // let alignment = "center";
      if (this.format === 'currency') {
        width = '80%'
      } else if (this.format === 'percent') {
        width = '85%'
      } else if (this.suffix) {
        width = '85%'
      } else {
        width = '100%'
      }
      return `display: table-cell; text-align: ${this.align()}; width: ${width};`
    },
    dataValue: {
      get () {
        const value = this.value
        if (this.editing) {
          return value
        }
        const dec = this.decimals ? this.decimals : 0
        if (isNaN(value)) {
          return ''
        }
        // return this.format === "currency" ? parseFloat(this.value).toFixed(2) : parseFloat(this.value).toFixed(dec);
        return this.format === 'currency'
          ? value.toLocaleString('en', { minimumFractionDigits: 2, maximumFractionDigits: 2 })
          : value.toLocaleString('en', { minimumFractionDigits: dec, maximumFractionDigits: dec })
      },
      set (v) {
        this.editing = true
        const field = this.$refs.textbox
        const pos = field.selectionStart

        const value = ('' + v).replace(/,/gi, '')
        // const value = this.format === "currency" ? parseFloat(str).toFixed(2) : parseFloat(str).toFixed(dec);
        this.$emit('input', value)

        field.focus()
        field.setSelectionRange(pos, pos)
      }
    }
  },
  mounted () {
    const textBox = this.$refs.textbox
    if (this.bold) {
      textBox.classList.add('boldtext')
    }
    textBox.readOnly = this.readOnly
    setInputFilter(textBox, function (value) {
      // Allow digits ',' and '.' only, using a RegExp
      return /^\d*\,?\d*\.?\d*$/.test(value)
    })
  },
  updated () {
    if (this.editing) {
      const tx = this.$refs.textbox
      tx.focus()
    }
  },
  methods: {
    keyDown (event) {
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
    onClick () {
      this.editing = true
    },
    onExit () {
      if (this.$refs.textbox !== document.activeElement) {
        this.editing = false
      }
    },
    align () {
      return this.$numberAlignment ? this.$numberAlignment : 'center'
    },
    notNull () {
      const value = this.value
      const isNull = value === undefined || value === null || value === ''
      return !isNull
    }
  }
}
</script>

<style scoped>
input {
  width: 100%;
}

.boldtext {
  font-weight: bold;
}
</style>
