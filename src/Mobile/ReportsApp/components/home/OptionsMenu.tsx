import { Button, MenuItem, OverflowMenu, Toggle } from "@ui-kitten/components"
import { DarkIcon, LightIcon, OptionsIcon } from "../Icons"
import React, { useContext, useEffect, useState } from "react"
import { ThemeContext } from "../../contexts/ThemeContext"
import { ReportsContext } from "../../contexts/ReportsContext"
import { StyleSheet } from "react-native"
import { useReports } from '../../hooks/useReports';

export interface Props {
  onChanged: (payload: { myReports: boolean; isClosed: boolean; filter?: string }) => void
}

export const OptionsMenu = ({onChanged}: Props) => {
  const context = useContext(ThemeContext)
  const [showMenu, setShowMenu] = useState(false)
  const { setFilter, getReports, reportsState: { myReports, isClosed, filter, descendingSort, orderBy } } = useReports()

  const hide = () => {
    if(showMenu) setShowMenu(false)
  }

  const show = () => {
    if(!showMenu) setShowMenu(true)
  }

  const handleIsClosed = () => {
    setFilter({ isClosed: !isClosed, myReports, filter, descendingSort, orderBy}); 
  }

  const handleMyReports = () => {
    setFilter({ isClosed, myReports: !myReports, filter, descendingSort, orderBy}); 
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

const styles = StyleSheet.create({
  options: {
    justifyContent: 'center'
  },
  toggle: {
    justifyContent: 'center',
    alignContent: 'center'
  }
})
