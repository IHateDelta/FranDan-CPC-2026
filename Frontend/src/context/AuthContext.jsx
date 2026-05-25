import { createContext, useState, useEffect, useContext } from "react";
import { api } from "../services/api";
import { ToastContext } from "./ToastContext";

export const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [token, setToken] = useState(localStorage.getItem("token") || null);
  const [loading, setLoading] = useState(true);
  const { addToast } = useContext(ToastContext);

  const fetchUserData = async () => {
    try {
      const response = await api.user.getFull();
      if (response.ok) {
        const userData = await response.json();
        setUser(userData);
      } else {
        handleLogout();
        addToast("Sesja wygasła. Zaloguj się ponownie.", "error");
      }
    } catch (error) {
      console.error("Błąd pobierania danych użytkownika:", error);
      addToast("Błąd połączenia z serwerem.", "error");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (token) {
      fetchUserData();
    } else {
      setLoading(false);
    }
  }, [token]);

  const login = async (username, password) => {
    try {
      const response = await api.auth.login({ username, password });

      if (response.ok) {
        const tokenString = await response.text();

        localStorage.setItem("token", tokenString);
        setToken(tokenString);
        addToast("Zalogowano pomyślnie!", "success");
        return true;
      } else {
        addToast("Błędny login lub hasło.", "error");
        return false;
      }
    } catch (error) {
      console.error("Błąd logowania:", error);
      addToast("Błąd serwera. Spróbuj ponownie później.", "error");
      return false;
    }
  };

  const handleLogout = () => {
    localStorage.removeItem("token");
    setToken(null);
    setUser(null);
  };

  return (
    <AuthContext.Provider
      value={{ user, token, loading, login, logout: handleLogout }}
    >
      {children}
    </AuthContext.Provider>
  );
};
