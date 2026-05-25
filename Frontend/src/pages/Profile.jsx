import { useState, useContext, useEffect } from "react";
import { AuthContext } from "../context/AuthContext";
import { ToastContext } from "../context/ToastContext";
import "./Profile.css";

const Profile = () => {
  const { user } = useContext(AuthContext);
  const { addToast } = useContext(ToastContext);

  const [myBirthday, setMyBirthday] = useState(() => {
    if (user?.userName) {
      return localStorage.getItem(`birthday_${user.userName}`) || "";
    }
    return "";
  });

  useEffect(() => {
    if (user?.userName) {
      const savedDate = localStorage.getItem(`birthday_${user.userName}`);
      setMyBirthday(savedDate || "");
    }
  }, [user]);

  const handleSave = (e) => {
    e.preventDefault();
    if (user?.userName) {
      localStorage.setItem(`birthday_${user.userName}`, myBirthday);
      addToast("Data urodzin została zapisana!", "success");
    }
  };

  const checkIsBirthdayToday = () => {
    if (!myBirthday) return false;
    const [, month, day] = myBirthday.split("-");
    const today = new Date();
    const currentMonth = String(today.getMonth() + 1).padStart(2, "0");
    const currentDay = String(today.getDate()).padStart(2, "0");
    return month === currentMonth && day === currentDay;
  };

  const isBirthdayToday = checkIsBirthdayToday();

  if (!user) {
    return (
      <p style={{ textAlign: "center", marginTop: "50px" }}>
        Musisz się zalogować.
      </p>
    );
  }

  return (
    <div
      className={`profile-container ${isBirthdayToday ? "birthday-bg" : ""}`}
    >
      {isBirthdayToday ? (
        <h2 className="party-title">Czas na imprezę! 🥳</h2>
      ) : (
        <h2>Ustawienia Profilu ⚙️</h2>
      )}

      <div className="profile-card">
        <div className="avatar-placeholder">
          {user?.userName?.charAt(0) || "U"}
        </div>
        <div className="user-details">
          <h3>{user?.userName || "Użytkownik"}</h3>
          <p className="role-text">Rola: {user?.role || "Brak"}</p>
        </div>
      </div>

      <form onSubmit={handleSave} className="profile-form">
        <div className="form-group">
          <label>Moja data urodzin:</label>
          <input
            type="date"
            value={myBirthday}
            onChange={(e) => setMyBirthday(e.target.value)}
            required
          />
        </div>
        <button type="submit" className="save-btn">
          Zapisz zmiany
        </button>
      </form>
    </div>
  );
};

export default Profile;
