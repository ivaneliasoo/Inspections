import React from 'react';
import {View, Image} from 'react-native';
import {KeyboardAvoidingView} from './KeyboardAvoidingView'
import { Icon, Button, Input, Text } from '@ui-kitten/components';
import {ImageOverlay} from './ImageOverlay'
import { StyleSheet } from 'react-native';
import {AuthContext} from '../authentication-context';
import { PasswordIcon, PersonIcon } from './Icons';
import { API_HOST } from '../services/api/api-accesor';


const LoginIcon = (props: any) => <Icon name="log-in-outline" {...props} />;

const Login = () => {
  const [user, setUser] = React.useState<string>('');
  const [password, setPassword] = React.useState<string>('');
  const authCtx = React.useContext(AuthContext)
  return (
    <KeyboardAvoidingView>
      <ImageOverlay style={styles.container} source={require('../assets/images/LoginBackground.jpg')}>
        <Image style={styles.logo} source={require('../assets/images/logo.png')}/>
        <View style={styles.signInContainer}>
          <Text status="control" category='h4'>Sign In</Text>
        </View>
        <View style={styles.formContainer}>
          <Input placeholder="enter your Username" accessoryLeft={PersonIcon} status='control' label="Username" blurOnSubmit={false} returnKeyType={'next'} value={user} onChangeText={setUser} autoCapitalize='none' />
          <Input placeholder="enter your Password" accessoryLeft={PasswordIcon} status='control' secureTextEntry={true} label="Password" returnKeyType={'done'} value={password} onSubmitEditing={(e) => authCtx.signIn({user, password})} onChangeText={setPassword} autoCapitalize='none' />
          <Button style={styles.evaButton} status='primary' size='large' accessoryRight={LoginIcon} onPress={(e) => authCtx.signIn({user, password}) }>Sign In</Button>
          <Text category='c1'>{ API_HOST }</Text>
        </View>
      </ImageOverlay>
    </KeyboardAvoidingView>
  )};

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