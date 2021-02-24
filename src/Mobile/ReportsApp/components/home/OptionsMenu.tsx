import { Button, MenuItem, OverflowMenu, Toggle } from "@ui-kitten/components"
import { DarkIcon, LightIcon, OptionsIcon } from "../Icons"
import React, { useContext, useState } from "react"
import { ThemeContext } from "../../theme-context"

export const OptionsMenu = () => {
  const context = useContext(ThemeContext)
  const [showMenu, setShowMenu] = useState(false)
  return (
    <>
      <OverflowMenu
        anchor={() => <Button appearance='ghost' accessoryLeft={OptionsIcon} onPress={() => setShowMenu(true)} />}
        visible={showMenu}
        onBackdropPress={() => setShowMenu(false)}>
        <MenuItem title={() => <Button style={{justifyContent: 'center'}} onPress={context.toggleTheme} accessoryRight={context.theme === 'dark' ? DarkIcon:LightIcon} />} />
        <MenuItem title={() => <Toggle style={{justifyContent: 'center'}} onPress={context.toggleTheme}>Mine Only</Toggle>} />
        <MenuItem title={() => <Toggle style={{justifyContent: 'center'}} onPress={context.toggleTheme}>Closed Only</Toggle>} />
      </OverflowMenu></>
  )
}