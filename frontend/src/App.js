import React, { useState } from 'react';
import LoginForm from './Component/Login';
import RegistrationForm from './Component/Register';
import Dashboard from './Component/Dashboard';
import './Styles.css';


const App = () => {
  const [token, setToken] = useState(null);

  const handleLogin = (newToken) => {
    setToken(newToken);
  };

  const handleLogout = () => {
    setToken(null);
  };

  return (
    <div>
      {token ? (
        <Dashboard token={token} onLogout={handleLogout} />

      ) : (
        <div>

          <RegistrationForm />
          <LoginForm onLogin={handleLogin} />
        </div>
      )}


    </div>
  );
};

export default App;
