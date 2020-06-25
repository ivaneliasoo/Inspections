<template>
    <div>
        <v-row align="center" justify="space-between">
            <v-col cols="6">
                 <v-text-field
                    v-model="currentReport.name"
                    label="Report Name"
                />
            </v-col>
            <v-col cols="6" class="text-right">
                <h4>{{ currentReport.formName }}</h4>
            </v-col>
        </v-row>
        <v-row>
            <v-col>
               
            </v-col>
        </v-row>  
        <v-row>
            <v-col>
                <v-textarea
                    label="Address"
                    rows="3"
                />
            </v-col>
        </v-row>  
    <v-row>
        <v-col cols="12" md="2">
            <v-select
                :items="emaTypes"
                label="EMA License Type"
            />
            </v-col>
            <v-col cols="12" md="3">
                <v-text-field
                    type="number"
                    label="License"
                />
            </v-col>
            <v-col cols="12" md="2">
                <DatePickerBase
                    type="number"
                    label="License"
                    :min="new Date().toISOString()"
                    max= ""
                />
            </v-col>
            <v-col cols="6" md="2">
                <v-checkbox label="Completed" disabled />
            </v-col>
            <v-col cols="6" md="2">
                <v-checkbox label="Close Report" />
            </v-col>
        </v-row> 
        <v-row>
            <v-col cols="12">
                
                <v-tabs
                    v-model="tabs"
                    fixed-tabs
                    >
                    <v-tabs-slider></v-tabs-slider>
                    <v-tab
                        href="#checklists"
                        class="primary--text"
                    >
                        <v-icon>mdi-check</v-icon>
                    </v-tab>

                    <v-tab
                        href="#notes"
                        class="primary--text"
                    >
                        <v-icon>mdi-message-bulleted</v-icon>
                    </v-tab>

                    <v-tab
                        href="#photos"
                        class="primary--text"
                    >
                        <v-icon>mdi-folder-multiple-image</v-icon>
                    </v-tab>
                    <v-tab
                        href="#signatures"
                        class="primary--text"
                    >
                        <v-icon>mdi-signature-freehand</v-icon>
                    </v-tab>
                </v-tabs>
                <v-tabs-items v-model="tabs">
                    <v-tab-item
                        key="checklists"
                        value="checklists"
                    >
                        <v-card flat>
                        <v-card-text>
                            <v-list
                            subheader
                            two-line
                            flat
                            >
                            <v-subheader>Items &amp; Params</v-subheader>
                            <v-list-item v-for="(item, checkListIndex) in currentReport.checkList" :key="item.id">
                                <template v-slot:default="">

                                <v-list-item-content class="text-left">
                                    <v-list-item-title>
                                        <v-row>
                                            <v-col cols="10">
                                                {{ checkListIndex + 1 }} .- {{ item.text }} 
                                                
                                            </v-col>
                                            <v-col cols="2">
                                                <span v-if="item.completed">
                                                    <v-chip small color="success">
                                                        Completed
                                                    </v-chip>
                                                </span>
                                            </v-col>
                                        </v-row>
                                        <v-row>
                                            <v-col cols="12">
                                                <v-list-item v-for="(checkItem, checkListItemIndex) in item.checks" :key="checkItem.id">
                                                    <template v-slot:default="{ active, toggle }">
                                                        

                                                        <v-list-item-title :title="checkItem.text">
                                                           
                                                           <v-row align="center" justify="space-between">
                                                               <v-col cols="12" md="7">
                                                                   {{ checkListIndex + 1 }}.{{ checkListItemIndex + 1}} .- {{ checkItem.text }}
                                                               </v-col>
                                                                <v-col cols="2" md="1">
                                                                    <v-simple-checkbox
                                                                    color="primary"
                                                                    @click="toggle"
                                                                    v-model="checkItem.checked"
                                                                    />
                                                                    
                                                                </v-col>
                                                                <v-col cols="10" md="4">
                                                                    <v-text-field :label="currentReport.remarksLabelText" />
                                                                </v-col>
                                                            </v-row>
                                                        </v-list-item-title>
                                                            
                                                    </template>
                                                </v-list-item>
                                            </v-col>
                                        </v-row>
                                    </v-list-item-title>
                                </v-list-item-content>
                                </template>
                            </v-list-item>
                            </v-list>
                        </v-card-text>
                        </v-card>
                    </v-tab-item>
                    <v-tab-item
                        key="notes"
                        value="notes"
                    >
                        <v-card flat>
                        <v-card-text>asadasdasdasd</v-card-text>
                        </v-card>
                    </v-tab-item>
                    <v-tab-item
                        key="photos"
                        value="photos"
                    >
                        <v-card flat>
                        <FileUploader />
                        </v-card>
                    </v-tab-item>
                    <v-tab-item
                        key="signatures"
                        value="signatures"
                    >
                        <v-card flat>
                        <v-card-text>
                            
                        </v-card-text>
                        </v-card>
                    </v-tab-item>
                </v-tabs-items>
            </v-col>
        </v-row> 
    </div>

</template>

<script lang="ts">
import { Vue, Component } from 'nuxt-property-decorator'
import { Report, EMALicenseType } from '../../../types'

@Component
export default class EditReport extends Vue {
    tabs: any = 0
    currentReport: Report = {} as Report
    emaTypes: any = Object.keys(EMALicenseType).map(key => { 
        if(!isNaN(Number(key.toString())))
            return
        
        return { id: EMALicenseType[key], text: key } }).filter(i=>i!==undefined)

    async fetch() {
        const result = await this.$store.dispatch('reportstrore/getReportById', this.$route.params.id, { root: true })
        this.currentReport= Object.assign({}, result)
    }
}
</script>