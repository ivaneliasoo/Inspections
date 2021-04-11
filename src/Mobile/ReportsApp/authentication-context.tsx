import React from 'react';

export const AuthContext = React.createContext({
  signIn: ({user, password}: {user: string, password: string}) => {}
});
