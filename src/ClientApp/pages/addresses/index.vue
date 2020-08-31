<template>
  <div>
    <alert-dialog
      v-model="dialogRemove"
      title="Remove Addresses"
      message="This operation will remove this Address. If Proceed, you no longer get it suggested again"
      :code="selectedItem.id"
      :description="selectedItem.title"
      @yes="deleteAddress();"
    />
    <v-data-table
      :items="addresses"
      item-key="id"
      :search="filter.filterText"
      dense
      :loading="loading"
      :headers="headers"
    >
      <template v-slot:top="{}">
        <v-toolbar flat color="white">
          <v-toolbar-title>Addresss</v-toolbar-title>
          <v-divider class="mx-4" inset vertical />
          <grid-filter :filter.sync="filter.filterText" />
          <v-spacer />
          <v-btn
            class="mx-2"
            x-small
            fab
            dark
            color="primary"
            @click="dialog = true; isNew = true; item = { id: 0, addressLine2: '' }"
          >
            <v-icon dark>mdi-plus</v-icon>
          </v-btn>
          <v-dialog
            v-model="dialog"
            persistent
            scrollable
            :fullscreen="$vuetify.breakpoint.smAndDown"
            :max-width="!$vuetify.breakpoint.smAndDown ? '50%' : '100%'"
          >
            <ValidationObserver tag="form" v-slot="{ valid, reset }">
              <v-card>
                <v-card-title>
                  <span class="headline">Edit Address</span>
                </v-card-title>
                <v-card-text>
                  <v-container>
                    <v-row align="center" justify="space-between">
                      <v-col cols="12" md="12">
                        <ValidationProvider rules="required" v-slot="{ errors }">
                          <v-textarea
                            v-model="item.addressLine"
                            autocomplete="nope"
                            name="addressLine"
                            rows="2"
                            :disabled="!isNew"
                            :error-messages="errors"
                            label="Address"
                          />
                        </ValidationProvider>
                      </v-col>
                    </v-row>
                    <v-row align="center" justify="space-between">
                      <v-col>
                          <v-textarea
                            v-model="item.addressLine2"
                            autocomplete="nope"
                            rows="2"
                            :error-messages="errors"
                            name="addressLine2"
                            label="Address Additional"
                          />
                      </v-col>
                      <v-col>
                          <v-text-field
                            v-model="item.city"
                            autocomplete="nope"
                            :error-messages="errors"
                            name="city"
                            label="City"
                          />
                      </v-col>
                    </v-row>
                    <v-row align="center" justify="space-between">
                      <v-col cols="12" md="12">
                          <v-text-field
                            v-model="item.province"
                            autocomplete="nope"
                            :error-messages="errors"
                            name="province"
                            label="Province"
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
                    :loading="loading"
                    :disabled="!valid"
                    @click="upsertAddress()"
                  >Save</v-btn>
                  <v-btn
                    color="default"
                    text
                    @click="reset(); item = { id: 0, addressLine2: '' }; dialog = false"
                  >Cancel</v-btn>
                </v-card-actions>
              </v-card>
            </ValidationObserver>
          </v-dialog>
        </v-toolbar>
      </template>
      <template v-slot:item.actions="{ item }">
        <v-tooltip v-if="$auth.user.isAdmin" top>
          <template v-slot:activator="{ on }">
            <v-icon
              v-on="on"
              color="primary"
              class="mr-2"
              @click="selectItem(item); isNew = false; dialog = true"
            >mdi-pencil</v-icon>
          </template>
          <span>Edit</span>
        </v-tooltip>
        <v-tooltip v-if="$auth.user.isAdmin" top>
          <template v-slot:activator="{ on }">
            <v-icon
              v-on="on"
              color="error"
              @click="selectItem(item); dialogRemove = true"
            >mdi-delete</v-icon>
          </template>
          <span>Delete</span>
        </v-tooltip>
      </template>
    </v-data-table>
  </div>
</template>
<script lang="ts">
import { Vue, Component, mixins } from "nuxt-property-decorator";
import { ValidationObserver, ValidationProvider } from "vee-validate";
import InnerPageMixin from "@/mixins/innerpage";
import { AddressesState } from "store/addresses";
import { AddressDTO } from "@/types/Addresses";

@Component({
  components: {
    ValidationObserver,
    ValidationProvider,
  },
})
export default class AddressesAdmin extends mixins(InnerPageMixin) {
  dialog: boolean = false;
  dialogRemove: boolean = false;
  loading: boolean = false;
  
  filter: any = {
    filterText: "",
  };
  headers: any[] = [
    {
      text: "ID",
      value: "id",
      sortable: true,
      align: "left",
    },
    {
      text: "Address",
      value: "addressLine",
      sortable: true,
      align: "left",
    },
    {
      text: "Address 2",
      value: "addressLine2",
      sortable: true,
      align: "left",
    },
    {
      text: "City",
      value: "city",
      sortable: true,
      align: "center",
    },
    {
      text: "Province",
      value: "province",
      sortable: true,
      align: "center",
    },
    {
      text: "",
      value: "actions",
      sortable: false,
      align: "left",
    },
  ];
  selectedItem: AddressDTO = {} as AddressDTO;
  item: any = { principal: false };
  isNew: boolean = false;

  get addresses(): AddressDTO[] {
    return (this.$store.state.addresses as AddressesState).addressList;
  }

  selectItem(item: AddressDTO): void {
    this.selectedItem = item;
    this.$store
      .dispatch("addresses/getAddressById", this.selectedItem.id, {
        root: true,
      })
      .then((resp) => (this.item = resp));
  }

  async fetch() {
    if (!this.$auth.user.isAdmin)
      this.$nuxt.error({ statusCode: 403, message: "Forbbiden" });
    await this.$store.dispatch("addresses/getAddresses", {}, { root: true });
  }

  deleteAddress() {
    this.$store
      .dispatch("addresses/deleteAddress", this.selectedItem.id, { root: true })
      .then(() => {
        this.dialog = false;
      });
  }

  async upsertAddress() {
    this.loading = true;
    if (!this.isNew)
      await this.$store.dispatch("addresses/updateAddress", this.item, { root: true });
    else {
      await this.$store.dispatch("addresses/createAddress", this.item, { root: true });
      await this.$store.dispatch("addresses/getAddresses", {}, { root: true });
    }
    this.dialog = false;
    this.isNew = true;
    this.loading = false;
  }
}
</script>