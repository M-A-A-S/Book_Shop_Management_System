import { createContext, useContext, useState } from "react";
import authService from "./authService";

export const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const API_BASE_URL = "https://localhost:7150";

  const login = async (credentials) => {
    if (await authService.login(credentials)) {
      setUser(credentials);
    }
  };

  const logout = async () => {
    if (await authService.logout()) {
      setUser(null);
    }
  };

  return (
    <AuthContext.Provider value={{ user, login, logout, API_BASE_URL }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuthContext = () => {
  return useContext(AuthContext);
};
