import { createContext, useState, useEffect, useContext } from "react";
import { AuthContext } from "./AuthContext";
import { ToastContext } from "./ToastContext";
import { api } from "../services/api";

export const FriendsContext = createContext();

export const FriendsProvider = ({ children }) => {
  const { user } = useContext(AuthContext);
  const { addToast } = useContext(ToastContext);

  const [friendStatuses, setFriendStatuses] = useState({});
  const [allUsers, setAllUsers] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchAllUsers = async () => {
      const users = await api.friends.getAllUsers();
      setAllUsers(users);
    };

    fetchAllUsers();
  }, []);

  useEffect(() => {
    const fetchFriendStatuses = async () => {
      if (user?.userName) {
        try {
          setLoading(true);
          const [usersData, statusesData] = await Promise.all([
            api.friends.getAllUsers(),
            api.friends.getStatuses(user.userName),
          ]);
          setAllUsers(usersData);
          setFriendStatuses(statusesData);
        } catch (error) {
          console.error("Błąd pobierania danych znajomych:", error);
        } finally {
          setLoading(false);
        }
      } else {
        setFriendStatuses({});
        setAllUsers([]);
        setLoading(false);
      }
    };

    fetchFriendStatuses();
  }, [user]);

  const inviteFriend = async (friendId) => {
    if (!user?.userName) return;

    const updatedStatuses = { ...friendStatuses, [friendId]: "pending" };
    setFriendStatuses(updatedStatuses);

    try {
      await api.friends.saveStatuses(user.userName, updatedStatuses);
    } catch (error) {
      console.error("Błąd podczas wysyłania zaproszenia:", error);
      addToast("Nie udało się wysłać zaproszenia. Spróbuj ponownie.", "error");
      setFriendStatuses(friendStatuses);
    }
  };

  const acceptFriend = async (friendId) => {
    if (!user?.userName) return;

    const updatedStatuses = { ...friendStatuses, [friendId]: "accepted" };
    setFriendStatuses(updatedStatuses);
    try {
      await api.friends.saveStatuses(user.userName, updatedStatuses);
    } catch (error) {
      console.error("Błąd podczas akceptowania znajomego:", error);
      addToast(
        "Nie udało się zaakceptować znajomego. Spróbuj ponownie.",
        "error",
      );
      setFriendStatuses(friendStatuses);
    }
  };

  const rejectFriend = async (friendId) => {
    if (!user?.userName) return;

    const updatedStatuses = { ...friendStatuses, [friendId]: "rejected" };
    setFriendStatuses(updatedStatuses);
    try {
      await api.friends.saveStatuses(user.userName, updatedStatuses);
    } catch (error) {
      console.error("Błąd podczas odrzucania zaproszenia:", error);
      addToast(
        "Nie udało się odrzucić zaproszenia. Spróbuj ponownie.",
        "error",
      );
      setFriendStatuses(friendStatuses);
    }
  };

  const removeFriend = async (friendId) => {
    if (!user?.userName) return;

    const updatedStatuses = { ...friendStatuses };
    delete updatedStatuses[friendId];
    setFriendStatuses(updatedStatuses);
    try {
      await api.friends.saveStatuses(user.userName, updatedStatuses);
    } catch (error) {
      console.error("Błąd podczas usuwania znajomego:", error);
      addToast("Nie udało się usunąć znajomego. Spróbuj ponownie.", "error");
      setFriendStatuses(friendStatuses);
    }
  };

  return (
    <FriendsContext.Provider
      value={{
        allUsers,
        friendStatuses,
        inviteFriend,
        acceptFriend,
        rejectFriend,
        removeFriend,
      }}
    >
      {children}
    </FriendsContext.Provider>
  );
};
