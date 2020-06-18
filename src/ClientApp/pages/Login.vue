<template>
  <v-app id="inspire">
    <v-main>
      <v-parallax
        dark
        height="900"
        src="/LoginBackground.jpg"
      >
        <v-container
          class="fill-height"
          fluid
        >
          <v-row
            align="center"
            justify="center"
          >
            <v-col
              cols="12"
              sm="8"
              md="4"
            >
              <v-card class="elevation-12">
                <v-row >
                  <v-col cols="12" class="text-center" ><img src="/Logo.jpeg" height="70px" width="150px"></v-col>
                  <v-col cols="12" class="text-center">
                    <h1>Login</h1>
                  </v-col>
                </v-row>
                <v-card-text>
                  <v-form>
                    <v-text-field
                      v-model="userName"
                      label="UserName"
                      name="userName"
                      prepend-icon="mdi-account"
                      type="text"
                    />

                    <v-text-field
                      id="password"
                      v-model="password"
                      label="Password"
                      name="password"
                      prepend-icon="mdi-lock"
                      type="password"
                    />
                  </v-form>
                </v-card-text>
                <v-card-actions>
                  <v-spacer />
                  <v-btn color="primary" @click="login">
                    Login
                  </v-btn>
                </v-card-actions>
              </v-card>
            </v-col>
          </v-row>
        </v-container>
      </v-parallax>
    </v-main>
  </v-app>
</template>

<script lang="ts">
import { Vue, Component } from 'nuxt-property-decorator'

  @Component({
    layout: 'guest'
  })
export default class LoginPage extends Vue {
      userName: string = ''
      password: string = ''

      async login () {
        await this.$auth.loginWith('local', { data: { userName: this.userName, password: this.password } })
      }
}
</script>
