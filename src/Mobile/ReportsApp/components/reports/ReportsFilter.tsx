import { IndexPath, Input, Select, SelectItem, Toggle } from '@ui-kitten/components';
import React, { useEffect } from 'react'
import { StyleSheet, Text, View } from 'react-native'
import { useReports } from '../../hooks/useReports';
import { SearchIcon } from '../Icons';
import { ReportsState, ReportsContext } from '../../contexts/ReportsContext';
import { useContext } from 'react';

export const ReportsFilter =() => {
  const { reportsState: { descendingSort, orderBy } } = useContext(ReportsContext)
  const { getReports, setSorting, filter, setFilterText } = useReports()

  return (
    <View>
      <Input style={styles.inpustSearch} status="info" accessoryLeft={SearchIcon} value={filter} onChangeText={setFilterText} onEndEditing={getReports} />
        <View style={{ marginVertical: 20, marginHorizontal: 20, flexDirection: 'row', justifyContent: 'space-between' }}>
          <Select
            style={{ minWidth: 200 }}
            size='large'
            placeholder='select an item to sort by...'
            label='Sort By'
            value={orderBy}
            onSelect={(e) => { setSorting(descendingSort, ['date', 'name', 'address', 'company'][(e as IndexPath).row]);}}
          >
            {['date', 'name', 'address', 'company'].map((column, index) =>
              <SelectItem
                key={index}
                title={column}
              ></SelectItem>)}
          </Select>
          <Toggle checked={descendingSort} onChange={() => { setSorting(!descendingSort, orderBy); }}>
            Descending
          </Toggle>
        </View>
    </View>
  )
}

const styles = StyleSheet.create({
  inpustSearch: { paddingHorizontal: 15 },
})
