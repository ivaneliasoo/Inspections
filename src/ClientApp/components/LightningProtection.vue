<template>
  <div style="width: 900px">
    This form may take you 5-8 minutes to complete.
    <div class="vbox border-ltr">
      <div class="row spaced">
        THE BUILDING CONTROL ACT (CAP 29)
      </div>
      <div class="row spaced">
        CERTIFICATE OF DESIGN &amp; SUPERVISION OF LIGHTNING PROTECTION SYSTEM
      </div>
    </div>
    <div class="hbox border-ltr">
      <div class="vbox col50 border-r">
        <div class="row">
          Commissioner of Building Control
        </div>
        <div class="row">
          Building &amp; Construction Authority
        </div>
        <div class="row">
          52 Jurong Gateway Road #11-01
        </div>
        <div class="row">
          Singapore 608550
        </div>
        <div class="row spc-bottom">
          Website: <a href="http://www.bca.gov.sg/" class="text_">http://www.bca.gov.sg/</a>
        </div>
      </div>
      <div class="vbox col50 spc-left">
        <div class="row">
          class="text_1">INSTRUCTIONS:
        </div>
        <div class="row">
          1&nbsp;One copy is to be submitted
        </div>
        <div class="row">
          2<span class="tab">&nbsp;</span>Please tick in the appropriate box.
        </div>
        <div class="row padding-bottom">
          3<span class="tab">&nbsp;</span>This form is to be filled in BLACK INK only.
        </div>
      </div>
    </div>

    <div class="vbox border-ltr border-b">
      <div class="row spaced">
        <label>
          This Certificate relates to the lightning protection system installed in the building(s) approved under: -
        </label>
      </div>

      <div class="row spaced">
        <label class="col1">
          Project Ref. No.:
        </label>
        <input type="text">
      </div>

      <div class="row spaced">
        <label class="col1">
          Address:
        </label>
        <input type="text">
      </div>

      <div class="row spaced">
        <label class="col1">
          TS/MK &amp; Lot/Plot:
        </label>
        <input type="text">
      </div>

      <div class="row spaced spc-bottom">
        <label class="col1">
          Remarks:
        </label>
        <input type="text">
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'LightningProtection'
}
</script>

<style scoped>
.table {
  width: 100%;
  border: 1px solid black;
  border-collapse: collapse;
}

td {
  border: 1px solid black;
}

input{
    background-color: transparent;
    border: none;
    border-bottom: 1px solid black;
    color: #555;
    width: 80%
}

.bordered {
  border: 1px solid black;
}

.border-ltr {
  border-left: 1px solid black;
  border-top: 1px solid black;
  border-right: 1px solid black;
}

.border-r {
  border-right: 1px solid black;
}

.border-b {
  border-bottom: 1px solid black;
}

.vbox {
  display: flex;
  justify-content: flex-start;
  flex-direction: column;
  padding-left: 20px;
}

.hbox {
  display: flex;
  justify-content: flex-start;
}

.col50 {
  width: 50%;
}

.row {
  display: flex;
  justify-content: flex-start;
}

.spc-left {
  margin-left: 10px;
}

.spaced {
  padding: 5px;
}

.spc-bottom {
  padding-bottom: 12px;
}

.col {
  width: 100%
}

.col1 {
  width: 150px;
  text-align: left;
}

</style>
