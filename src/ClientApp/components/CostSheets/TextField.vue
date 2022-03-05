<template>
  <div>
    <div v-if="viewMode"
      class="text-caption text-left table-input text-box"
      style="width: 200px; padding-top: 0px; padding-bottom: 0px;"
      @click="onClick">
      {{dataValue}}
    </div>
    <textarea v-else
      ref="textbox"
      class="text-caption table-input"
      style="width: 200px; padding: 0px;"
      v-model="dataValue"
      @focusout="onExit">
    </textarea>
  </div>
</template>

<style scoped>
.text-box {
  line-height: 15px;
  padding-top: 2px;
  padding-bottom: 2px;
}
</style>

<script>
export default {
  props: {
    value: {
      required: true,
    }
  },
  computed: {
    viewMode() {
      return !this.editing && this.dataValue && this.dataValue.length>0;
    },
    dataValue: {
      get() {
        return this.value;
      },
      set(v) {
        this.editing = true;
        this.$emit('input', v);
      }
    }
  },
  updated() {
    if (this.editing) {
      const tx = this.$refs.textbox;
      tx.focus(); 
      tx.setAttribute("style", "width: 200px; height:" + (tx.scrollHeight) + "px;");
    }
  },
  data: () => {
    return {
      editing: false
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