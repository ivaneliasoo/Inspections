<template>
  <div>
  <alert-dialog
      v-model="dialogRemove"
      title="Remove Reports"
      message="This operation will remove this User and all data related"
      :code="selectedItem.id"
      :description="selectedItem.title"
      @yes="deleteUser();"
    />
    <v-data-table :items="users" item-key="userName" :search="filter.filterText" dense :loading="loading" :headers="headers">
      <template v-slot:top="{}">
        <v-toolbar flat color="white">
          <v-toolbar-title>Users</v-toolbar-title>
          <v-divider class="mx-4" inset vertical />
          <grid-filter :filter.sync="filter.filterText" />
          <v-spacer />
          <v-btn class="mx-2" x-small 
            fab dark color="primary"
            @click="dialog = true; isNew = true; item = { isAdmin: false }">
              <v-icon dark>mdi-plus</v-icon>
          </v-btn>
          <v-dialog v-model="dialog" persistent  scrollable
            :fullscreen="$vuetify.breakpoint.smAndDown"
            :max-width="!$vuetify.breakpoint.smAndDown ? '50%' : '100%'">
          <ValidationObserver tag="form" v-slot="{ valid, reset }">
            <v-card>
              <v-card-title>
                <span class="headline">Edit User</span>
              </v-card-title>
              <v-card-text>
                <v-container>
                    <v-row align="center" justify="space-between">
                      <v-col cols="12" md="6">
                        <ValidationProvider rules="required" v-slot="{ errors }">
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
                        <v-checkbox 
                          v-model="item.isAdmin"
                          name="isAdmin"
                          label="Administrator" 
                        />
                      </v-col>
                     
                    </v-row>
                    <v-row align="center" justify="space-between">
                      <v-col>
                        <ValidationProvider name="pass" rules="required|password:@confirm" v-slot="{ errors }">
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
                        <ValidationProvider name="confirm" rules="required|password:@pass" v-slot="{ errors }">
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
                         <ValidationProvider rules="required" v-slot="{ errors }">
                        <v-text-field 
                          v-model="item.name" 
                          :error-messages="errors" 
                          name="Name"
                          label="Name" 
                        />
                         </ValidationProvider>
                      </v-col>
                      <v-col cols="12" md="6">
                        <ValidationProvider rules="required" v-slot="{ errors }">
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
                <v-btn color="success" text :disabled="item.report ? item.report.isClosed ? true:false : false && !valid" 
                @click="upsertUser()">
                  Save
                </v-btn>
                <v-btn color="default" text @click="reset(); item = { principal: false }; dialog = false">
                  Cancel
                </v-btn>
              </v-card-actions>
            </v-card>
                  </ValidationObserver>
          </v-dialog>
        </v-toolbar>
      </template>
      <template v-slot:item.actions="{ item }">
        <v-icon
          small
          color="primary"
          class="mr-2"
          @click="selectItem(item); isNew = false; dialog = true"
        >
          mdi-lock
        </v-icon>
        <v-icon
          small
          color="primary"
          class="mr-2"
          @click="selectItem(item); isNew = false; dialog = true"
        >
          mdi-pencil
        </v-icon>
        <v-icon
          small
          :disabled="item.userName === $auth.user.userName"
          color="error"
          @click="selectItem(item); dialogRemove = true"
        >
          mdi-delete
        </v-icon>
      </template>
    </v-data-table>
  </div>
</template>
<script lang="ts">
import { Vue, Component, mixins } from 'nuxt-property-decorator'
import { ValidationObserver, ValidationProvider } from 'vee-validate'
import InnerPageMixin from '@/mixins/innerpage'
import { UserState } from 'store/users'
import { User, ChangePasswordDTO } from '../../types/Users'

import { extend } from 'vee-validate';

extend('password', {
  params: ['target'],
  validate(value, { target }: any) {
    return value === target;
  },
  message: 'Password confirmation does not match'
});

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
      align: 'center'
    },
    {
      text: 'Name',
      value: 'name',
      sortable: true,
      align: 'center'
    },
    {
      text: 'Last Name',
      value: 'lastName',
      sortable: true,
      align: 'center'
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
      align: 'center'
    }
  ];
  selectedItem: User = {} as User
  item: any = { principal: false }
  isNew: boolean = false

  get users (): User[] {
    return (this.$store.state.users as UserState)
      .users
  }

   selectItem (item: User): void{
    this.selectedItem = item
    this.$store.dispatch('users/getUserByName', this.selectedItem.userName, { root: true })
      .then(resp => this.item = resp)
  }

  async fetch () {
    if (!this.$auth.user.isAdmin)
      this.$nuxt.error({ statusCode: 403, message: 'Forbbiden' })
    await this.$store.dispatch('users/getUsers', {}, { root: true })
  }

  deleteUser () {
    this.$store.dispatch('users/deleteUser', this.selectedItem.userName, { root: true })
      .then(() => {
        this.dialog = false
      })
  }

  async upsertUser () {
    if(!this.isNew)
      await this.$store.dispatch('users/updateUser', this.item, { root: true })
    else{
      await this.$store.dispatch('users/createUser', this.item, { root: true })
      await this.$store.dispatch('users/getUsers', {}, { root: true })
    }

    if(this.item.password === this.confirmPassword)
      await this.$store.dispatch('users/changePassword',
      {
        userName: this.item.userName,
        currentPassword: '',
        newPassword: this.item.password,
        newPasswordConfirmation: this.confirmPassword
      } as ChangePasswordDTO, { root: true })

    this.dialog = false
    this.isNew = true
  }
}
</script>