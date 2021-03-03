import { Button, MenuItem, OverflowMenu, Toggle } from "@ui-kitten/components"
import { DarkIcon, LightIcon, OptionsIcon } from "../Icons"
import React, { useContext, useState } from "react"
import { ThemeContext } from "../../theme-context"
import { ReportsContext } from "../../reports-contexts"

export const OptionsMenu = ({onChanged}) => {
  const context = useContext(ThemeContext)
  const [showMenu, setShowMenu] = useState(false)
  const { setMyReports, setIsClosed, myReports, isClosed } = useContext<any>(ReportsContext)
  return (
    <>
      <OverflowMenu
        anchor={() => <Button appearance='ghost' accessoryLeft={OptionsIcon} onPress={() => setShowMenu(true)} />}
        visible={showMenu}
        onBackdropPress={() => setShowMenu(false)}>
        <MenuItem style={{ alignContent: 'center' }} title={() => <Button status='control' style={{ justifyContent: 'center', alignSelf: 'center' }} onPress={context.toggleTheme} accessoryRight={context.theme === 'dark' ? DarkIcon : LightIcon} />} />
        <MenuItem title={() => <Toggle style={{ justifyContent: 'center' }} onPress={context.toggleTheme} checked={myReports} onChange={() => { setMyReports(!myReports); onChanged() }}>My Reports</Toggle>} />
        <MenuItem title={() => <Toggle style={{ justifyContent: 'center' }} onPress={context.toggleTheme} checked={isClosed} onChange={() => { setIsClosed(!isClosed); onChanged() }}>Closed Only</Toggle>} />
      </OverflowMenu></>
  )
}