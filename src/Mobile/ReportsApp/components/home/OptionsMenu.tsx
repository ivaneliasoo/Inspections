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
  const { setFilter, reportsState: { myReports, isClosed, filter } } = useContext(ReportsContext)

  const hide = () => {
    if(showMenu) setShowMenu(false)
  }

  const show = () => {
    if(!showMenu) setShowMenu(true)
  }

  const handleIsClosed = () => {
    setFilter({ isClosed: !isClosed, myReports, filter}); 
    onChanged({ myReports: myReports, isClosed: !isClosed })
  }

  const handleMyReports = () => {
    setFilter({ isClosed, myReports: !myReports, filter}); 
    onChanged({ myReports: myReports, isClosed: !isClosed })
  }

  return (
    <>
      <OverflowMenu
        anchor={() => <Button appearance='ghost' accessoryLeft={OptionsIcon} onPress={show} />}
        visible={showMenu}
        onBackdropPress={hide}>
        <MenuItem style={styles.toggle} title={() => <Button status='control' style={styles.options} onPress={context.toggleTheme} accessoryRight={context.theme === 'dark' ? DarkIcon : LightIcon} />} />
        <MenuItem title={() => <Toggle style={styles.toggle} checked={myReports} onChange={handleMyReports}>My Reports</Toggle>} />
        <MenuItem title={() => <Toggle style={styles.toggle} checked={isClosed} onChange={handleIsClosed}>Closed Only</Toggle>} />
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
