<template>
  <div>
    <alert-dialog
      v-model="dialogRemove"
      title="Remove Users"
      message="This operation will remove this User and all data related"
      :code="selectedItem.id"
      :description="selectedItem.title"
      @yes="deleteUser();"
    />
    <v-data-table
      :class="$device.isTablet ? 'tablet-text':''"
      :items="users"
      item-key="userName"
      :search="filter.filterText"
      dense
      :loading="loading"
      :headers="headers"
    >
      <template #top="{}">
        <v-toolbar flat color="white">
          <v-toolbar-title>Users</v-toolbar-title>
          <v-divider class="mx-4" inset vertical />
          <grid-filter :filter.sync="filter.filterText" />
          <v-spacer />
          <v-btn
            class="mx-2"
            x-small
            fab
            dark
            color="primary"
            @click="dialog = true; isNew = true; item = { isAdmin: false }"
          >
            <v-icon dark>
              mdi-plus
            </v-icon>
          </v-btn>
          <v-dialog
            v-model="dialog"
            persistent
            scrollable
            :fullscreen="$vuetify.breakpoint.smAndDown"
            :max-width="!$vuetify.breakpoint.smAndDown ? '50%' : '100%'"
          >
            <ValidationObserver v-slot="{ valid, reset }" tag="form">
              <v-card>
                <v-card-title>
                  <span class="headline">Edit User</span>
                </v-card-title>
                <v-card-text>
                  <v-container>
                    <v-row align="center" justify="space-between">
                      <v-col cols="12" md="6">
                        <ValidationProvider v-slot="{ errors }" rules="required">
                          <v-text-field
                            v-model="item.userName"
                            name="title"
                            :disabled="!isNew"
                            :error-messages="errors"
                            label="User Name"
                          />
                        </ValidationProvider>
                      </v-col>
                      <v-col cols="12" md="6">
                        <v-checkbox v-model="item.isAdmin" name="isAdmin" label="Administrator" />
                      </v-col>
                    </v-row>
                    <v-row align="center" justify="space-between">
                      <v-col>
                        <ValidationProvider
                          v-slot="{ errors }"
                          name="pass"
                          rules="required|password:@confirm"
                        >
                          <v-text-field
                            v-model="item.password"
                            :error-messages="errors"
                            type="password"
                            name="password"
                            label="Password"
                          />
                        </ValidationProvider>
                      </v-col>
                      <v-col>
                        <ValidationProvider
                          v-slot="{ errors }"
                          name="confirm"
                          rules="required|password:@pass"
                        >
                          <v-text-field
                            v-model="confirmPassword"
                            :error-messages="errors"
                            type="password"
                            name="confirmPassword"
                            label="Confirm Password"
                          />
                        </ValidationProvider>
                      </v-col>
                    </v-row>
                    <v-row align="center" justify="space-between">
                      <v-col cols="12" md="6">
                        <ValidationProvider v-slot="{ errors }" rules="required">
                          <v-text-field
                            v-model="item.name"
                            :error-messages="errors"
                            name="Name"
                            label="Name"
                          />
                        </ValidationProvider>
                      </v-col>
                      <v-col cols="12" md="6">
                        <ValidationProvider v-slot="{ errors }" rules="required">
                          <v-text-field
                            v-model="item.lastName"
                            :error-messages="errors"
                            name="lastName"
                            label="Last Name"
                          />
                        </ValidationProvider>
                      </v-col>
                    </v-row>
                  </v-container>
                </v-card-text>
                <v-card-actions>
                  <v-spacer />
                  <v-btn
                    color="success"
                    text
                    :loading="loading"
                    :disabled="item.report ? item.report.isClosed ? true:false : false && !valid"
                    @click="upsertUser()"
                  >
                    Save
                  </v-btn>
                  <v-btn
                    color="default"
                    text
                    @click="reset(); item = { principal: false }; dialog = false"
                  >
                    Cancel
                  </v-btn>
                </v-card-actions>
              </v-card>
            </ValidationObserver>
          </v-dialog>
        </v-toolbar>
      </template>
      <template #item.actions="{ item }">
        <v-tooltip v-if="$auth.user.isAdmin" top>
          <template #activator="{ on }">
            <v-icon
              color="primary"
              class="mr-2"
              v-on="on"
              @click="selectItem(item); isNew = false; dialog = true"
            >
              mdi-pencil
            </v-icon>
          </template>
          <span>Edit / Change Password</span>
        </v-tooltip>
        <v-tooltip v-if="$auth.user.isAdmin" top>
          <template #activator="{ on }">
            <v-icon
              :disabled="item.userName === $auth.user.userName"
              color="error"
              v-on="on"
              @click="selectItem(item); dialogRemove = true"
            >
              mdi-delete
            </v-icon>
          </template>
          <span>Delete</span>
        </v-tooltip>
      </template>
      <template #item.isAdmin="{ item }">
        <v-simple-checkbox v-model="item.isAdmin" disabled />
      </template>
    </v-data-table>
  </div>
</template>
<script lang="ts">
import { Component, mixins } from 'nuxt-property-decorator'
import { ValidationObserver, ValidationProvider, extend } from 'vee-validate'
import { User, ChangePasswordDTO } from '../../types/Users'
import InnerPageMixin from '@/mixins/innerpage'
import { UserState } from 'store/users'

extend('password', {
  params: ['target'],
  validate (value, { target }: any) {
    return value === target
  },
  message: 'Password confirmation does not match'
})

@Component({
  components: {
    ValidationObserver,
    ValidationProvider
  }
})
export default class UserAdmin extends mixins(InnerPageMixin) {
  dialog: boolean = false
  dialogRemove: boolean = false
  loading: boolean = false
  confirmPassword: string = ''
  filter: any = {
    filterText: ''
  }

  headers: any[] = [
    {
      text: 'UserName',
      value: 'userName',
      sortable: true,
      align: 'left'
    },
    {
      text: 'Name',
      value: 'name',
      sortable: true,
      align: 'left'
    },
    {
      text: 'Last Name',
      value: 'lastName',
      sortable: true,
      align: 'left'
    },
    {
      text: 'Admin',
      value: 'isAdmin',
      sortable: true,
      align: 'center'
    },
    {
      text: '',
      value: 'actions',
      sortable: false,
      align: 'left'
    }
  ]

  selectedItem: User = {} as User
  item: any = { principal: false }
  isNew: boolean = false

  get users (): User[] {
    return (this.$store.state.users as UserState).users
  }

  selectItem (item: User): void {
    this.selectedItem = item
    this.$store
      .dispatch('users/getUserByName', this.selectedItem.userName, {
        root: true
      })
      .then(resp => (this.item = resp))
  }

  async fetch ({ error, $auth, store }: any) {
    if (!$auth.user.isAdmin) { error({ statusCode: 403, message: 'Forbbiden' }) }
    this.loading = true
    await store.dispatch('users/getUsers', {}, { root: true })
    this.loading = false
  }

  deleteUser () {
    this.$store
      .dispatch('users/deleteUser', this.selectedItem.userName, { root: true })
      .then(() => {
        this.dialog = false
      })
  }

  async upsertUser () {
    this.loading = true
    if (!this.isNew) { await this.$store.dispatch('users/updateUser', this.item, { root: true }) } else {
      await this.$store.dispatch('users/createUser', this.item, { root: true })
      await this.$store.dispatch('users/getUsers', {}, { root: true })
    }

    if (this.item.password === this.confirmPassword) {
      await this.$store.dispatch(
        'users/changePassword',
        {
          userName: this.item.userName,
          currentPassword: '',
          newPassword: this.item.password,
          newPasswordConfirmation: this.confirmPassword
        } as ChangePasswordDTO,
        { root: true }
      )
    }

    this.dialog = false
    this.isNew = true
    this.loading = false
  }
}
</script>
