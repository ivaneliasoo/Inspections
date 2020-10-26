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
      :class="$device.isTablet ? 'tablet-text':''"
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
                        <ValidationProvider rules="required" immediate v-slot="{ errors }">
                          <v-textarea
                            v-model="item.addressLine"
                            autocomplete="nope"
                            name="addressLine"
                            rows="2"
                            :error-messages="errors"
                            label="Address"
                          />
                        </ValidationProvider>
                      </v-col>
                    </v-row>
                    <v-row align="center" justify="space-between">
                     <v-col cols="12" md="6">
                        <ValidationProvider rules="required" immediate v-slot="{ errors }">
                          <v-text-field
                            v-model="item.country"
                            autocomplete="nope"
                            :error-messages="errors"
                            name="country"
                            label="Country"
                          />
                        </ValidationProvider>
                      </v-col>
                      <v-col>
                        <ValidationProvider rules="required" immediate v-slot="{ errors }">
                          <v-text-field
                            v-model="item.unit"
                            autocomplete="nope"
                            :error-messages="errors"
                            name="unit"
                            label="Unit"
                          />
                        </ValidationProvider>
                      </v-col>
                       <v-col cols="12" md="6">
                        <ValidationProvider rules="required" immediate v-slot="{ errors }">
                          <v-text-field
                            v-model="item.postalCode"
                            autocomplete="nope"
                            :error-messages="errors"
                            name="postalCode"
                            label="PostalCode"
                          />
                        </ValidationProvider>
                      </v-col>
                      <v-col>
                        <ValidationProvider rules="required" immediate v-slot="{ errors }">
                          <!-- <v-text-field
                            v-model="item.licenseNumber"
                            autocomplete="nope"
                            :error-messages="errors"
                            name="license"
                            label="License Number"
                          /> -->
                          <v-autocomplete 
                            v-model="item.licenseId"
                            :items="licenses"
                            item-text="number"
                            item-value="licenseId"
                            autocomplete="nope"
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
                            name="addressLine2"
                            label="Address Additional"
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
import { LicensesState } from "store/licenses";
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
      text: "Unit",
      value: "unit",
      sortable: true,
      align: "left",
    },
    {
      text: "Country",
      value: "country",
      sortable: true,
      align: "left",
    },
    {
      text: "Postal Code",
      value: "postalCode",
      sortable: true,
      align: "left",
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

  get licenses() {
    return (this.$store.state.licenses as LicensesState).licensesList
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
    
    Promise.all([ await this.$store.dispatch("addresses/getAddresses", {}, { root: true }),
    await this.$store.dispatch("licenses/getLicenses", {}, { root: true })])
   
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