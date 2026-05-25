import { useState, useContext, useEffect } from "react";
import { AuthContext } from "../context/AuthContext";
import { ToastContext } from "../context/ToastContext";
import { api } from "../services/api";
import "./Profile.css";

const Profile = () => {
  const { user, fetchUserData } = useContext(AuthContext);
  const { addToast } = useContext(ToastContext);

  const [myBirthday, setMyBirthday] = useState("");

  useEffect(() => {
    if (user && user.birthday) {
      if (user.birthday.includes(".")) {
        const [day, month, year] = user.birthday.split(".");
        const formattedDate = `${year}-${month}-${day}`;
        setMyBirthday(formattedDate);
      } else if (user.birthday.includes("T")) {
        setMyBirthday(user.birthday.split("T")[0]);
      } else {
        setMyBirthday(user.birthday);
      }
    }
  }, [user]);

  const handleSave = async (e) => {
    e.preventDefault();

    try {
      const response = await api.user.update({ birthday: myBirthday });

      if (response.ok) {
        addToast("Data urodzin została zapisana w bazie!", "success");

        if (fetchUserData) {
          await fetchUserData();
        }
      } else {
        addToast("Wystąpił błąd podczas zapisu.", "error");
      }
    } catch (error) {
      console.error("Błąd zapisywania profilu:", error);
      addToast("Brak połączenia z serwerem.", "error");
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
          {user.username ? user.username.charAt(0).toUpperCase() : "U"}
        </div>
        <div className="user-details">
          <h3>{user.username || "Użytkownik"}</h3>
          <p className="role-text">Rola: {user.occupation || "Brak"}</p>
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
