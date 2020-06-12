export default {
  // async get ({ commit }) {
  //   return await this.$salasService.index().then((res) => {
  //     commit('SET_LISTADO_SALAS', res)
  //   })
  // },
  // async getSala ({ commit }, id) {
  //   return await this.$salasService.show(id)
  // },
  // async getSalas ({ commit }, filter) {
  //   return await this.$salasService.resumen(filter).then((res) => {
  //     commit('salas/SET_ALL_SALAS', res, { root: true })
  //   })
  // },
  // async export ({ commit }, filter) {
  //   return await this.$salasService.export(filter)
  // },
  // async add ({ commit, rootState }, payload) {
  //   const obj = {
  //     id: 0,
  //     codigo: payload.codSala,
  //     titulo: payload.tituloSala,
  //     descripcion: payload.descSala,
  //     usuario: rootState.user
  //   }
  //   return await this.$salasService.create(obj).then((res) => {
  //     payload.idSala = res.idSala
  //     commit('salas/ADD_SALA', payload, { root: true })
  //   })
  // },
  // async update ({ commit, rootState }, payload) {
  //   const obj = {
  //     codigo: payload.codSala,
  //     titulo: payload.tituloSala,
  //     descripcion: payload.descSala,
  //     usuario: rootState.user
  //   }
  //   return await this.$salasService.update(payload.idSala, obj).then((res) => {
  //     commit('salas/PUT_SALA', payload, { root: true })
  //   })
  // },
  // async delete ({ commit }, id) {
  //   return await this.$salasService.delete(id).then((res) => {
  //     commit('salas/REMOVE_SALA', id, { root: true })
  //   })
  // },
  // setSelectedSala ({ commit }, payload) {
  //   commit('salas/SET_SELECTED', payload, { root: true })
  // }
}
