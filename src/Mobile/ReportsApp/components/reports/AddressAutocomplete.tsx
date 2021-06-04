import { Input, Select, SelectItem } from '@ui-kitten/components';
import moment from 'moment';
import React, { useEffect, useRef, useState } from 'react'
import { useContext } from 'react';
import { StyleSheet } from 'react-native';
import { AddressDTO } from 'services/api';
import { ReportsContext } from '../../contexts/ReportsContext';
import { useAddresses } from '../../hooks/useAddresses';


export interface AddressSelectedResult {
  formattedAddress: string;
  licenseNumber: string;
}

export const AddressAutocomplete = ({ values, errors, flexType, onSelect }: any) => {
  const { getAddressList, addresses } = useAddresses()

  const reanderOption = (item: AddressDTO, index: number) => {
    return (<SelectItem
      key={index}
      title={`${item.formatedAddress} (${item.number} Expires on ${moment(item.validity?.end).format('DD/MM/YYYY')})`}
    ></SelectItem>
    )
  }

  return (
    <>
      <Select
        placeholder='type to search an address'
        value={values.address!}
        style={[styles.inputMargin]} label='Address'
        onSelect={(e: any) => onSelect({ formattedAddress: addresses[e.row].formatedAddress, licenseNumber: addresses[e.row].number!})}
        status={errors.address ? 'danger' : 'basic'}
        caption={errors.address}
      >
        {addresses.map(reanderOption)}
      </Select>
      <Input
        style={[{ flex: flexType === 'row' ? 2 : 1 }, styles.inputMargin]}
        disabled
        label='License'
        value={values.license?.number!}
        caption='Selected Address License Number' />
    </>
  )
}

const styles = StyleSheet.create({
  inputMargin: {
    marginHorizontal: 3
  }
})