import Picker from '@gregfrench/react-native-wheel-picker'
import React, { useEffect, useMemo, useState } from 'react'
import { StyleSheet, Text, View } from 'react-native'
import { setBasicAuthToObject } from 'services/api/common';

const PickerItem = Picker.Item;

type NumericPickerProps = {
  defaultValue: number[];
  preppendLabel?: string;
  appendLabel?: string;
  itemSelected: (value: number) => void
}
const NumericPicker = ({ defaultValue = [0,0,0], itemSelected, appendLabel = 'V', preppendLabel = 'L1-N' }: NumericPickerProps) => {
  const [selectedItem, setSelectedItem] = useState(defaultValue);
  const [firstDigit, setFirstDigit] = useState(selectedItem[0])
  const [secondDigit, setSecondDigit] = useState(selectedItem[1])
  const [thirdDigit, setThirdDigit] = useState(selectedItem[2])
  const [itemList, setItemList] = useState([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);

  const value = useMemo(() => {
    return [firstDigit, secondDigit, thirdDigit]
  }, [firstDigit, secondDigit, thirdDigit])

  useEffect(() => {
    setSelectedItem(value)
    itemSelected(parseInt(value.join("")))
  }, [firstDigit, secondDigit, thirdDigit])

  return (
    <View style={{flexDirection: 'row', justifyContent: 'space-evenly', alignItems: 'center', marginBottom: 10}}>
      <Text style={styles.label}>{preppendLabel}</Text>
      <Text>
        <Picker style={{ width: 30, height: 70 }}
          lineColor="#000000" //to set top and bottom line color (Without gradients)
          lineGradientColorFrom="#008000" //to set top and bottom starting gradient line color
          lineGradientColorTo="#FF5733" //to set top and bottom ending gradient
          selectedValue={firstDigit}
          itemStyle={{ color: "black", fontSize: 28 }}
          onValueChange={setFirstDigit}>
          {itemList.map((value, i) => (
            <PickerItem label={value.toString()} value={i} key={i} />
          ))}
        </Picker>
        <Picker style={{ width: 30, height: 70 }}
          lineColor="#000000" //to set top and bottom line color (Without gradients)
          lineGradientColorFrom="#008000" //to set top and bottom starting gradient line color
          lineGradientColorTo="#FF5733" //to set top and bottom ending gradient
          selectedValue={secondDigit}
          itemStyle={{ color: "black", fontSize: 28 }}
          onValueChange={setSecondDigit}>
          {itemList.map((value, i) => (
            <PickerItem label={value.toString()} value={i} key={i} />
          ))}
        </Picker>
        <Picker style={{ width: 30, height: 70 }}
          lineColor="#000000" //to set top and bottom line color (Without gradients)
          lineGradientColorFrom="#008000" //to set top and bottom starting gradient line color
          lineGradientColorTo="#FF5733" //to set top and bottom ending gradient
          selectedValue={thirdDigit}
          itemStyle={{ color: "black", fontSize: 28 }}
          onValueChange={setThirdDigit}>
          {itemList.map((value, i) => (
            <PickerItem label={value.toString()} value={i} key={i} />
          ))}
        </Picker>
      </Text>
      <Text style={styles.label}>{appendLabel}</Text>
    </View>
  )
}

export default NumericPicker

const styles = StyleSheet.create({
  label: {marginHorizontal: 20, fontSize: 18 }
  
})
