import { useState, useContext } from "react";
import { AuthContext } from "../context/AuthContext";
import { FriendsContext } from "../context/FriendsContext";
import { ToastContext } from "../context/ToastContext";
import { Navigate } from "react-router-dom";
import "./Friends.css";

const Friends = () => {
  const { token, user } = useContext(AuthContext);
  const { addToast } = useContext(ToastContext);
  const [searchQuery, setSearchQuery] = useState("");

  const {
    allUsers,
    friendStatuses,
    loading,
    inviteFriend,
    acceptFriend,
    rejectFriend,
    removeFriend,
  } = useContext(FriendsContext);

  if (!token) {
    return <Navigate to="/login" replace />;
  }

  if (loading) {
    return (
      <div
        className="friends-container"
        style={{ textAlign: "center", paddingTop: "50px" }}
      >
        <div className="spinner"></div>
        <p style={{ color: "#868e96", marginTop: "10px" }}>
          Wyszukiwanie znajomych w bazie danych...
        </p>
      </div>
    );
  }

  const others = allUsers.filter((u) => u.id !== user?.id);

  const filteredUsers = others.filter((friend) =>
    friend.name.toLowerCase().includes(searchQuery.toLowerCase()),
  );

  return (
    <div className="friends-container">
      <h2>Znajomi</h2>

      <div className="search-box">
        <input
          type="text"
          placeholder="Wyszukaj znajomego ..."
          value={searchQuery}
          onChange={(e) => setSearchQuery(e.target.value)}
        />
      </div>

      <div className="friends-grid">
        {filteredUsers.length > 0 ? (
          filteredUsers.map((person) => (
            <div key={person.id} className="friend-card">
              <div className="friend-info">
                <div className="friend-avatar">{person.avatar}</div>
                <div>
                  <h3>{person.name}</h3>
                  <p>{person.role}</p>
                </div>
              </div>

              <div className="friend-actions">
                {friendStatuses[person.id] === "accepted" ? (
                  <>
                    <span className="status-badge accepted">Znajomy</span>
                    <button
                      className="btn-remove"
                      onClick={() => {
                        removeFriend(person.id);
                        addToast("Znajomy został usunięty.", "info");
                      }}
                      title="Usuń ze znajomych"
                    >
                      ❌
                    </button>
                  </>
                ) : friendStatuses[person.id] === "pending" ? (
                  <>
                    <span className="status-badge pending">⏳ Wysłano</span>
                    <button
                      className="btn-accept"
                      onClick={() => {
                        acceptFriend(person.id);
                        addToast(
                          "Zaproszenie zostało zaakceptowane!",
                          "success",
                        );
                      }}
                      title="Akceptuj zaproszenie"
                    >
                      ✔️
                    </button>
                    <button
                      className="btn-remove"
                      onClick={() => {
                        rejectFriend(person.id);
                        addToast("Zaproszenie zostało odrzucone!", "info");
                      }}
                      title="Odrzuć zaproszenie"
                    >
                      ❌
                    </button>
                  </>
                ) : friendStatuses[person.id] === "rejected" ? (
                  <>
                    <span className="status-badge rejected">Odrzucono</span>
                    <button
                      className="btn-remove"
                      onClick={() => {
                        removeFriend(person.id);
                        addToast("Status został zresetowany!", "info");
                      }}
                      title="Zresetuj status"
                    >
                      🔄
                    </button>
                  </>
                ) : (
                  <button
                    className="btn-invite"
                    onClick={() => {
                      inviteFriend(person.id);
                      addToast("Wysłano zaproszenie!", "success");
                    }}
                  >
                    Zaproś
                  </button>
                )}
              </div>
            </div>
          ))
        ) : (
          <p style={{ textAlign: "center", width: "100%", color: "#888" }}>
            Nie znaleziono nikogo o takich danych.
          </p>
        )}
      </div>
    </div>
  );
};

export default Friends;
