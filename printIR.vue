<template>
  <div style="display: block">
    <header>
      <PrintingLogoir />
      <div class="content">
        <!-- <h1 class="title">Inspection Report</h1> -->
        <h4 class="subtitle">INCIDENT REPORT</h4>
        <div class="direccion">
          HSBC Institutional<br />
          Trust Services (Singapore) Limited As Trustee of Sabana Reit<br />
          151 Lorong Chuan #02-03<br />
          New Tech Park<br />
          Singapore 556741<br />
        </div>
      </div>
    </header>
    <div class="wrapper">
      <div class="row">
        
      </div>
    </div>
    <footer>
      <PrintingLogofooterir />
    </footer>
  </div>
</template>



<style>
header .container {
  display: flex;
  align-items: center;
}
.direccion {
  font-weight: bold;
}
.subtitle {
  text-align: right;
  padding-right: 10px;
}

.container {
  padding: 20px;
  justify-content: end;
}
.wrapper {
  display: flex;
  /* align-items: center; */
  flex-direction: column;
  padding: 20px;
  gap: 10px;
}
.row {
  display: flex;
  flex-wrap: wrap;
}
.cell {
  overflow: hidden;
  padding: 20px;
  background: #ddd;
  width: 50vw;
}
.description {
  padding: 20px;
  border: 1px solid black;
}
.note {
  display: flex;
  flex-direction: column;
}
.note span {
  text-decoration: underline;
}
.instructions {
  font-weight: bold;
}
.instructions span {
  text-decoration: underline;
}
.particulars {
  width: 100%;
}
.particulars span {
  font-weight: bold;
  text-decoration: underline;
}
.particular-inputs {
  display: flex;
  padding: 20px 0px;
}
.particular-inputs label {
  width: 150px;
}
.particular-inputs input {
  width: 100%;
}
.special-input {
  width: 50%;
}
input {
  background-color: transparent;
  border: none;
  border-bottom: 1px solid black;
  color: #555;
  box-sizing: border-box;
}
input:focus {
  outline: none;
}
.ordered-cell {
  overflow: hidden;
  padding: 20px;
  /* margin: 2px 1em; */
  background: #ddd;
  /* flex: 1; */
  width: 100%;
  align-items: center;
}
ol {
  counter-reset: section;
  list-style-type: none;
}
/* li::before {
    counter-increment: section;
    content: counters(section,".") ". ";
    } */
.ordered-cell ol {
  padding: 0px;
}
.ordered-cell ol li {
  display: flex;
  /* justify-content: space-between;
        align-items: center; */
}
.ordered-cell ol li span {
  text-decoration: underline;
  font-weight: bold;
}
.list-box {
  display: flex;
  justify-content: space-between;
  align-items: center;
  width: 100%;
}
.my_table {
  width: 100%;
}
</style>
