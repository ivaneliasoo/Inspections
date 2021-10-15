<template>
  <div style="display: block">
    <header>
      <PrintingLogo />
      <div class="content">
        <h1 class="title">Inspection Report</h1>
        <h2 class="subtitle">CSE EI(R1) FORM</h2>
      </div>
    </header>
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
  font-size: .75em;
  text-decoration: underline;
  text-align: right;
}
</style>
