/* eslint-disable @typescript-eslint/no-unused-vars */
const express = require('express')
const consola = require('consola')
const { loadNuxt } = require('nuxt-start')
process.env.UNIX_SOCKET = process.env.PORT
const app = express()
async function start() {
  const nuxt = await loadNuxt('start')
  await nuxt.listen(process.env.PORT, process.env.HOST)
}

start()
