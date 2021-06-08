import React, { ReactElement, useRef } from 'react';
import { View, Image } from 'react-native';
import { KeyboardAvoidingView } from './KeyboardAvoidingView'
import { Icon, Button, Input, Text, useTheme } from '@ui-kitten/components';
import { ImageOverlay } from './ImageOverlay'
import { StyleSheet } from 'react-native';
import { AuthContext } from '../contexts/AuthContext';
import { PasswordIcon, PersonIcon } from './Icons';
import { API_HOST } from '../services/api/api-accesor';
import { showMessage, hideMessage } from 'react-native-flash-message'


const LoginIcon = (props: any) => <Icon name="log-in-outline" {...props} />;

const Login = () => {
  const theme = useTheme()
  const [user, setUser] = React.useState<string>('');
  const passwordInput = useRef(null)
  const [password, setPassword] = React.useState<string>('');
  const authCtx = React.useContext(AuthContext)

  const logIn = async () => {
    try {
      await authCtx.signIn({ user, password })
    } catch (error: any) {
      if (error && error.response) {
        showMessage({
          message: 'invalid username or password',
          animated: true,
          autoHide: true,
          backgroundColor: theme['color-danger-700'],
          duration: 3000,
          position: 'bottom'
        })
      } else {
        showMessage({
          message: 'invalid username or password',
          animated: true,
          autoHide: true,
          backgroundColor: theme['color-danger-700'],
          duration: 3000,
          position: 'bottom'
        })
      }
    }
  }

  const formIsValid = React.useMemo(() => user && password, [user, password])


  return (
    <KeyboardAvoidingView>
      <ImageOverlay style={styles.container} source={require('../assets/images/LoginBackground.jpg')}>
        <Image style={styles.logo} source={require('../assets/images/logo.png')} />
        <View style={styles.signInContainer}>
          <Text status="control" category='h4'>Sign In</Text>
        </View>
        <View style={styles.formContainer}>
          <Input placeholder="enter your Username"
            accessoryLeft={PersonIcon}
            status='control'
            label="Username"
            blurOnSubmit={false}
            returnKeyType={'next'}
            value={user}
            onChangeText={setUser}
            onSubmitEditing={() => passwordInput.current!.focus()}
            autoCapitalize='none'
          />
          <Input ref={passwordInput}
            placeholder="enter your Password"
            accessoryLeft={PasswordIcon}
            status='control'
            secureTextEntry={true}
            label="Password"
            returnKeyType={'done'}
            value={password}
            onSubmitEditing={logIn}
            onChangeText={setPassword}
            autoCapitalize='none' />
          <Button style={styles.evaButton}
            disabled={!formIsValid}
            status='primary'
            size='large'
            accessoryRight={LoginIcon} o
            nPress={logIn}>Sign In</Button>
          <Text category='c1'>{API_HOST}</Text>
        </View>
      </ImageOverlay>
    </KeyboardAvoidingView>
  )
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    paddingVertical: 24,
    paddingHorizontal: 16,
  },
  signInContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    marginTop: 24,
  },
  evaButton: {
    marginTop: 24,
  },
  formContainer: {
    flex: 1,
    marginTop: 48,
  },
  passwordInput: {
    marginTop: 16,
  },
  signInLabel: {
    flex: 1,
  },
  logo: {
    flex: 1,
    alignSelf: 'center',
    alignItems: 'center',
    maxHeight: 100,
    maxWidth: 250,
  }
})

export { Login }