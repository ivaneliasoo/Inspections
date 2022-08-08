<template>
  <div>
    <alert-dialog
      v-model="componentState.dialogRemove"
      title="Remove Users"
      message="This operation will remove this User and all data related"
      :code="componentState.selectedItem.id"
      :description="componentState.selectedItem.title"
      @yes="deleteUser()"
    />
    <v-data-table
      :class="$device.isTablet ? 'tablet-text' : ''"
      :items="users"
      item-key="userName"
      :search="componentState.filter.filterText"
      dense
      :loading="componentState.loading"
      :headers="headers"
    >
      <template #top="{}">
        <v-toolbar flat color="white">
          <v-toolbar-title>Users</v-toolbar-title>
          <v-divider class="mx-4" inset vertical />
          <grid-filter :filter.sync="componentState.filter.filterText" />
          <v-spacer />
          <v-btn
            class="mx-2"
            x-small
            fab
            dark
            color="primary"
            @click="
              componentState.dialog = true
              componentState.isNew = true
              componentState.item = { isAdmin: false }
            "
          >
            <v-icon dark> mdi-plus </v-icon>
          </v-btn>
          <v-dialog
            v-model="componentState.dialog"
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
                        <ValidationProvider
                          v-slot="{ errors }"
                          rules="required"
                        >
                          <v-text-field
                            v-model="componentState.item.userName"
                            name="title"
                            :disabled="!componentState.isNew"
                            :error-messages="errors"
                            label="User Name"
                          />
                        </ValidationProvider>
                      </v-col>
                      <v-col cols="12" md="6">
                        <v-checkbox
                          v-model="componentState.item.isAdmin"
                          name="isAdmin"
                          label="Administrator"
                        />
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
                            v-model="componentState.item.password"
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
                            v-model="componentState.confirmPassword"
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
                        <ValidationProvider
                          v-slot="{ errors }"
                          rules="required"
                        >
                          <v-text-field
                            v-model="componentState.item.name"
                            :error-messages="errors"
                            name="Name"
                            label="Name"
                          />
                        </ValidationProvider>
                      </v-col>
                      <v-col cols="12" md="6">
                        <ValidationProvider
                          v-slot="{ errors }"
                          rules="required"
                        >
                          <v-text-field
                            v-model="componentState.item.lastName"
                            :error-messages="errors"
                            name="lastName"
                            label="Last Name"
                          />
                        </ValidationProvider>
                      </v-col>
                    </v-row>
                    <v-row>
                      <v-col>
                        <v-btn
                          color="primary"
                          @click="
                            componentState.showSignature =
                              !componentState.showSignature
                          "
                        >
                          {{ componentState.showSignature ? 'Hide' : 'Show' }}
                          Sign
                        </v-btn>
                      </v-col>
                    </v-row>
                    <v-row align="center" justify="center">
                      <v-col>
                        <SignaturePad
                          v-if="componentState.showSignature"
                          v-model="componentState.item.signature"
                          :saved-data="componentState.item.signature"
                        />
                      </v-col>
                    </v-row>
                  </v-container>
                </v-card-text>
                <v-card-actions>
                  <v-spacer />
                  <v-btn
                    color="success"
                    text
                    :loading="componentState.loading"
                    :disabled="
                      componentState.item.report
                        ? componentState.item.report.isClosed
                          ? true
                          : false
                        : false && !valid
                    "
                    @click="upsertUser()"
                  >
                    Save
                  </v-btn>
                  <v-btn
                    color="default"
                    text
                    @click="
                      reset()
                      componentState.item = { principal: false }
                      componentState.showSignature = false
                      componentState.dialog = false
                    "
                  >
                    Cancel
                  </v-btn>
                </v-card-actions>
              </v-card>
            </ValidationObserver>
          </v-dialog>
        </v-toolbar>
      </template>
      <template #[`item.actions`]="{ item }">
        <v-tooltip v-if="$auth.user.isAdmin" top>
          <template #activator="{ on }">
            <v-icon
              color="primary"
              class="mr-2"
              v-on="on"
              @click="
                selectItem(item)
                componentState.isNew = false
                componentState.dialog = true
              "
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
              @click="
                selectItem(item)
                componentState.dialogRemove = true
              "
            >
              mdi-delete
            </v-icon>
          </template>
          <span>Delete</span>
        </v-tooltip>
      </template>
      <template #[`item.isAdmin`]="{ item }">
        <v-simple-checkbox v-model="item.isAdmin" disabled />
      </template>
    </v-data-table>
  </div>
</template>
<script lang="ts">
import { ValidationObserver, ValidationProvider, extend } from 'vee-validate'
import {
  defineComponent,
  reactive,
  computed,
  useFetch,
} from '@nuxtjs/composition-api'
import { User, ChangePasswordDTO } from '../../types/Users'
import { useUsersStore } from '~/composables/useUsersStore'
import useGoBack from '~/composables/useGoBack'

extend('password', {
  params: ['target'],
  validate(value, { target }: any) {
    return value === target
  },
  message: 'Password confirmation does not match',
})

export default defineComponent({
  components: {
    ValidationObserver,
    ValidationProvider,
  },
  setup() {
    useGoBack()

    const usersStore = useUsersStore()

    const headers = [
      {
        text: 'UserName',
        value: 'userName',
        sortable: true,
        align: 'left',
      },
      {
        text: 'Name',
        value: 'name',
        sortable: true,
        align: 'left',
      },
      {
        text: 'Last Name',
        value: 'lastName',
        sortable: true,
        align: 'left',
      },
      {
        text: 'Admin',
        value: 'isAdmin',
        sortable: true,
        align: 'center',
      },
      {
        text: '',
        value: 'actions',
        sortable: false,
        align: 'left',
      },
    ]

    const componentState = reactive({
      dialog: false,
      dialogRemove: false,
      loading: false,
      confirmPassword: '',
      showSignature: false,
      filter: {
        filterText: '',
      },
      selectedItem: { id: 0, title: '' } as unknown as User,
      item: { principal: false, password: '' } as any,
      isNew: false,
    })

    const users = computed(() => {
      return usersStore.$state.users
    })

    const selectItem = (item: User): void => {
      componentState.selectedItem = item
      usersStore
        .getUserByName(componentState.selectedItem.userName)
        .then((resp) => (componentState.item = resp))
    }

    useFetch(async ({ error, $auth }: any) => {
      if (!$auth.user.isAdmin) {
        error({ statusCode: 403, message: 'Forbbiden' })
      }
      componentState.loading = true
      await usersStore.getUsers()
      componentState.loading = false
    })

    const deleteUser = () => {
      usersStore.deleteUser(componentState.selectedItem.userName).then(() => {
        componentState.dialog = false
      })
    }

    const upsertUser = async () => {
      componentState.loading = true
      if (!componentState.isNew) {
        await usersStore.updateUser(componentState.item as unknown as User)
      } else {
        await usersStore.createUser(componentState.item as unknown as User)
        await usersStore.getUsers()
      }

      if (componentState.item.password === componentState.confirmPassword) {
        await usersStore.changePassword({
          userName: componentState.item.userName,
          currentPassword: '',
          newPassword: componentState.item.password,
          newPasswordConfirmation: componentState.confirmPassword,
        } as ChangePasswordDTO)
      }

      componentState.dialog = false
      componentState.isNew = true
      componentState.loading = false
      componentState.showSignature = false
    }

    return {
      headers,
      users,
      selectItem,
      deleteUser,
      upsertUser,
      componentState,
    }
  },
})
</script>
