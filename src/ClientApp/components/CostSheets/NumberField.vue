<template>
  <div>
    <label v-if="value && format==='currency'" class="text-caption">$</label>
    <input
      ref="textbox"
      type="text"
      class="text-caption text-bold table-input"
      :style="style"
      v-model="dataValue"
      @change="$emit('change', $event.target.value);"
    />
    <label v-if="value" class="text-caption">{{suffixStr}}</label>
  </div>
</template>

<style scoped>
input {
  width: 100%;
}

.boldtext {
  font-weight: bold;
}
</style>

<script>
// Restricts input for the given textbox to the given inputFilter function.
// https://stackoverflow.com/questions/469357/html-text-input-allow-only-numeric-input
function setInputFilter(textbox, inputFilter) {
  ["input", "keydown", "keyup", "mousedown", "mouseup", "select", "contextmenu", "drop"].forEach(function(event) {
    textbox.addEventListener(event, function() {
      if (inputFilter(this.value)) {
        this.oldValue = this.value;
        this.oldSelectionStart = this.selectionStart;
        this.oldSelectionEnd = this.selectionEnd;
      } else if (this.hasOwnProperty("oldValue")) {
        this.value = this.oldValue;
        this.setSelectionRange(this.oldSelectionStart, this.oldSelectionEnd);
      } else {
        this.value = "";
      }
    });
  });
}
export default {
  props: {
    value: {
      required: true,
    },
    format: String,
    readOnly: Boolean,
    decimals: Number,
    suffix: String,
    bold: Boolean
  },
  computed: {
    suffixStr() {
      if (this.format === 'percent') {
        return "%" 
      }
      return this.suffix ? this.suffix : "";
    },
    style() {
      let width;
      if (this.format === "currency") {
        width = "80%";
      } else if (this.format === "percent") {
        width = "85%";
      } else  if (this.suffix) {
        width = "85%"
      } else {
        width = "100%";
      }
      return `display: table-cell; text-align: right; width: ${width};`
    },
    dataValue: {
      get() {
        if (this.editing) {
          return this.value;
        }
        const dec = this.decimals ? this. decimals : 0;
        if (isNaN(this.value)) {
          return "";
        }
        return this.format === "currency" ? parseFloat(this.value).toFixed(2) : parseFloat(this.value).toFixed(dec); 
      },
      set(v) {
        this.editing = true;
        this.$emit('input', v);
      }
    }
  },
  mounted() {
    const textBox = this.$refs.textbox;
    if (this.bold) {
      textBox.classList.add("boldtext");
    }
    textBox.readOnly = this.readOnly;
    setInputFilter(textBox, function(value) {
      return /^\d*\.?\d*$/.test(value); // Allow digits and '.' only, using a RegExp
    })
  },
  updated() {
    if (this.editing) {
      const tx = this.$refs.textbox;
      tx.focus(); 
    }
  },
  methods: {
    onClick() {
      this.editing=true;
    },
    onExit() {
      if (this.$refs.textbox !== document.activeElement) {
        this.editing=false;
      }
    }
  }
};
</script>