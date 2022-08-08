<template>
  <div style="display: block">
    <nuxt />
  </div>
</template>

<style scoped>
header,
footer,
.content {
  display: flex;
}

body {
  margin: 0;
  color: #000;
  background-color: #fff;
}

header {
  flex-flow: row wrap;
  align-items: center;
  justify-content: space-between;
  line-height: 1;
}

header div.content {
  flex-flow: column;
}

header h1.title {
  margin-bottom: 2px;
  font-weight: 600;
  font-size: 1em;
}

header h2.subtitle {
  margin: 0;
  font-weight: normal;
  font-size: 0.75em;
  text-decoration: underline;
  text-align: right;
}
</style>
