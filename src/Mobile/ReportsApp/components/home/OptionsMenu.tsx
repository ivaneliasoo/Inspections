import { Button, MenuItem, OverflowMenu, Toggle } from "@ui-kitten/components"
import { DarkIcon, LightIcon, OptionsIcon } from "../Icons"
import React, { useContext, useState } from "react"
import { ThemeContext } from "../../contexts/ThemeContext"
import { ReportsContext } from "../../contexts/ReportsContext"
import { StyleSheet } from "react-native"

type Props = {
  onChanged: any
}
export const OptionsMenu = ({onChanged}: Props) => {
  const context = useContext(ThemeContext)
  const [showMenu, setShowMenu] = useState(false)
  const { setMyReports, setIsClosed, myReports, isClosed } = useContext<any>(ReportsContext)

  const hide = () => {
    if(showMenu) setShowMenu(false)
  }

  const show = () => {
    if(!showMenu) setShowMenu(true)
  }

  return (
    <>
      <OverflowMenu
        anchor={() => <Button appearance='ghost' accessoryLeft={OptionsIcon} onPress={show} />}
        visible={showMenu}
        onBackdropPress={hide}>
        <MenuItem style={styles.toggle} title={() => <Button status='control' style={styles.options} onPress={context.toggleTheme} accessoryRight={context.theme === 'dark' ? DarkIcon : LightIcon} />} />
        <MenuItem title={() => <Toggle style={styles.toggle} onPress={context.toggleTheme} checked={myReports} onChange={() => { setMyReports(!myReports); onChanged({ myReports: !myReports, isClosed: isClosed }) }}>My Reports</Toggle>} />
        <MenuItem title={() => <Toggle style={styles.toggle} onPress={context.toggleTheme} checked={isClosed} onChange={() => { setIsClosed(!isClosed); onChanged({ myReports: myReports, isClosed: !isClosed }) }}>Closed Only</Toggle>} />
      </OverflowMenu></>
  )
}
OptionsMenu.whyDidYouRender = true
const styles = StyleSheet.create({
  options: {
    justifyContent: 'center'
  },
  toggle: {
    justifyContent: 'center',
    alignContent: 'center'
  }
})
