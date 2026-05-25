import { useState, useContext } from "react";
import { useNavigate } from "react-router-dom";
import { AuthContext } from "../context/AuthContext";
import "./Login.css";

const Login = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const { login } = useContext(AuthContext);
  const navigate = useNavigate();

  const handleSubmit = (e) => {
    e.preventDefault();

    const Users = {
      "franciszek@pwr.pl": { userName: "Franciszek Pora", role: "Student" },
      "dawid@pwr.pl": { userName: "Dawid Podsiadło", role: "Student" },
      "jakub@pwr.pl": { userName: "Jakub Grzegorzek", role: "Student" },
    };

    const fakeToken = "fake-jwt-token";
    const userData = Users[email] || {
      userName: email.split("@")[0],
      role: "Gość",
    };

    login(fakeToken, userData);
    alert(`Witaj, ${userData.userName}! Zostałeś zalogowany.`);
    navigate("/");
  };

  return (
    <div className="login-container">
      <div className="login-box">
        <h2>Zaloguj się</h2>
        <form onSubmit={handleSubmit} className="login-form">
          <div className="form-group">
            <label>Adres E-mail: </label>
            <input
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              placeholder="name@example.com"
              required
            />
          </div>

          <div className="form-group">
            <label>Hasło: </label>
            <input
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </div>

          <button type="submit" className="login-btn">
            Zaloguj się do systemu
          </button>
        </form>
      </div>
    </div>
  );
};

export default Login;
