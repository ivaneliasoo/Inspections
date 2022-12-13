<template>
  <v-app id="inspire">
    <v-main>
      <v-parallax dark height="900" src="/LoginBackground.jpg">
        <v-container class="fill-height" fluid>
          <v-row align="center" justify="center">
            <v-col cols="12" sm="8" md="4">
              <v-card class="elevation-12">
                <v-row>
                  <v-col cols="12" class="text-center">
                    <img src="/Logo.jpeg" height="70px" width="150px" />
                  </v-col>
                  <v-col cols="12" class="text-center">
                    <h1>Login</h1>
                  </v-col>
                </v-row>
                <v-form id="signin-form" action="" @submit.prevent="userLogin">
                  <v-card-text>
                    <v-text-field
                      id="username"
                      v-model="state.userName"
                      label="UserName"
                      name="userName"
                      autocomplete="username"
                      prepend-icon="mdi-account"
                      type="text"
                    />

                    <v-text-field
                      id="password"
                      v-model="state.password"
                      label="Password"
                      autocomplete="current-password"
                      name="password"
                      prepend-icon="mdi-lock"
                      type="password"
                    />
                  </v-card-text>
                  <v-card-actions>
                    <v-spacer />
                    <v-btn
                      color="primary"
                      :loading="state.loading"
                      type="submit"
                      @click.prevent="login"
                    >
                      Login
                    </v-btn>
                  </v-card-actions>
                </v-form>
                <v-row>
                  <v-alert
                    :value="state.hasError"
                    color="pink"
                    dark
                    border="top"
                    icon="mdi-home"
                    transition="scale-transition"
                  >
                    Invalid Credentials. Try again
                  </v-alert>
                </v-row>
              </v-card>
            </v-col>
          </v-row>
        </v-container>
      </v-parallax>
    </v-main>
  </v-app>
</template>

<script lang="ts">
import { useContext, defineComponent, reactive } from '@nuxtjs/composition-api'

export default defineComponent({
  layout: 'guest',
  setup() {
    const state = reactive({
      userName: '',
      password: '',
      loading: false,
      hasError: false
    })

    const { $auth } = useContext()

    const login = async () => {
      state.loading = true
      await $auth
        .login({ data: { userName: state.userName, password: state.password } })
        .catch((error) => {
          state.hasError = true

          setTimeout(() => {
            state.hasError = false
          }, 3000)
        })
        .finally(() => {
          state.loading = false
        })
    }

    return {
      login,
      state
    }
  }
})
</script>
