import { useState, useContext } from "react";
import { useNavigate, Navigate, Link } from "react-router-dom";
import { AuthContext } from "../context/AuthContext";
import { PlansContext } from "../context/PlansContext";
import { FriendsContext } from "../context/FriendsContext";
import { ToastContext } from "../context/ToastContext";
import "./AddPlan.css";

const AddPlan = () => {
  const [title, setTitle] = useState("");
  const [date, setDate] = useState("");
  const [category, setCategory] = useState("inny");

  const { addPlan } = useContext(PlansContext);
  const { token } = useContext(AuthContext);
  const { addToast } = useContext(ToastContext);
  const { allUsers, friendStatuses } = useContext(FriendsContext);
  const navigate = useNavigate();

  const [selectedFriendsIds, setSelectedFriendsIds] = useState([]);

  if (!token) {
    return <Navigate to="/login" replace />;
  }

  const myFriends = allUsers.filter((u) => friendStatuses[u.id] === "accepted");

  const handleCheckboxChange = (friendId) => {
    if (selectedFriendsIds.includes(friendId)) {
      setSelectedFriendsIds(selectedFriendsIds.filter((id) => id !== friendId));
    } else {
      setSelectedFriendsIds([...selectedFriendsIds, friendId]);
    }
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    if (!title.trim()) {
      addToast("Tytuł planu nie może być pusty!", "error");
      return;
    }

    const selectedDate = new Date(date);
    const now = new Date();

    if (selectedDate < now) {
      addToast("Nie możesz zaplanować wydarzenia w przeszłości!", "error");
      return;
    }

    const selectedFriends = selectedFriendsIds
      .map((id) => {
        const friend = allUsers.find((u) => u.id === id);
        return friend ? friend : null;
      })
      .filter(Boolean);

    const newPlan = {
      id: Date.now(),
      title: title.trim(),
      category,
      date: date.replace("T", " "),
      participants: ["Ja", ...selectedFriends.map((f) => f.name)],
    };

    addPlan(newPlan);
    addToast("Plan został pomyślnie dodany!", "success");
    navigate("/plans");
  };

  return (
    <div className="addplan-form-container">
      <h2>Dodaj nowy plan</h2>
      <form onSubmit={handleSubmit} className="plan-form">
        <div className="form-group">
          <label>Tytuł planu:</label>
          <input
            type="text"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            required
          />
        </div>
        <div className="form-group">
          <label>Data i godzina:</label>
          <input
            type="datetime-local"
            value={date}
            onChange={(e) => setDate(e.target.value)}
            required
          />
        </div>
        <div className="form-group">
          <label>Kategoria:</label>
          <select
            value={category}
            onChange={(e) => setCategory(e.target.value)}
          >
            <option value="sport">Sport</option>
            <option value="nauka">Nauka</option>
            <option value="zwiedzanie">Zwiedzanie</option>
            <option value="impreza">Impreza</option>
            <option value="inny">Inny</option>
          </select>
        </div>
        <div className="form-group">
          <label>Wybierz uczestników wydarzenia:</label>
          {myFriends.length > 0 ? (
            <div className="checkbox-friends-list">
              {myFriends.map((friend) => (
                <label key={friend.id} className="checkbox-friend-item">
                  <input
                    type="checkbox"
                    checked={selectedFriendsIds.includes(friend.id)}
                    onChange={() => handleCheckboxChange(friend.id)}
                  />
                  <span>{friend.name}</span>
                </label>
              ))}
            </div>
          ) : (
            <p>Brak dostępnych znajomych do zaproszenia.</p>
          )}
        </div>

        <button type="submit" className="submit-btn">
          Utwórz Plan
        </button>
      </form>
    </div>
  );
};

export default AddPlan;
