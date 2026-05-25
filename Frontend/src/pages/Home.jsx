import { useContext } from "react";
import { AuthContext } from "../context/AuthContext";
import { FriendsContext } from "../context/FriendsContext";
import { PlansContext } from "../context/PlansContext";
import { Navigate, Link } from "react-router-dom";
import "./Home.css";

const Home = () => {
  const { token, user } = useContext(AuthContext);
  const { friendStatuses } = useContext(FriendsContext);
  const { plans, fetchPlans } = useContext(PlansContext);

  if (!token) {
    return <Navigate to="/login" replace />;
  }

  const myBirthday = user?.username
    ? localStorage.getItem(`birthday_${user.username}`)
    : null;

  const checkIsBirthday = (dateString) => {
    if (!dateString) return false;
    const [, month, day] = dateString.split("-");
    const today = new Date();
    const currentMonth = String(today.getMonth() + 1).padStart(2, "0");
    const currentDay = String(today.getDate()).padStart(2, "0");
    return month === currentMonth && day === currentDay;
  };

  const isBirthdayToday = checkIsBirthday(myBirthday);

  const allBirthdays = plans
    .filter((plan) => plan.category === "birthday")
    .map((plan) => ({
      id: plan.id,
      name: plan.title,
      date: plan.date,
    }));

  const friendsBirthdays = allBirthdays.filter(
    (person) => friendStatuses[person.id] === "accepted",
  );

  const calculateDaysToBirthday = (targetDateString) => {
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    const birthDate = new Date(targetDateString);

    const nextBirthday = new Date(
      today.getFullYear(),
      birthDate.getMonth(),
      birthDate.getDate(),
    );

    if (nextBirthday < today) {
      nextBirthday.setFullYear(today.getFullYear() + 1);
    }

    const diffTime = nextBirthday - today;
    const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));

    return diffDays;
  };

  const upcomingPlans = plans
    .filter((plan) => new Date(plan.date) > new Date())
    .sort((a, b) => new Date(a.date) - new Date(b.date))
    .slice(0, 3);

  return (
    <div className="home-container">
      {isBirthdayToday && (
        <div className="birthday-banner">
          🎉 Wszystkiego najlepszego, {user?.username.split(" ")[0]}! Spełnienia
          marzeń i świetnej zabawy! 🎁
        </div>
      )}

      <h2 className="section-title">Twoje Najbliższe Plany 📅</h2>
      {upcomingPlans.length > 0 ? (
        <div className="cards-grid">
          {upcomingPlans.map((plan) => (
            <div key={plan.id} className="dashboard-card plan-card">
              <h3>{plan.title}</h3>
              <p className="date-text">
                <strong>Kiedy:</strong> {plan.date}
              </p>
              <p className="category-text">
                <strong>Kategoria:</strong> {plan.category}
              </p>
              <div className="participants-text">
                <strong>Z kim:</strong>{" "}
                {plan.participants.length === 1 && plan.participants[0] === "Ja"
                  ? "Tylko ja"
                  : plan.participants.join(", ")}
              </div>
            </div>
          ))}
        </div>
      ) : (
        <div className="empty-state">
          <p>Nie masz jeszcze żadnych nadchodzących planów.</p>
          <Link to="/add-plan" className="empty-state-link">
            Stwórz nowy plan
          </Link>
        </div>
      )}

      <h2 className="section-title" style={{ marginTop: "40px" }}>
        Nadchodzące Urodziny Znajomych 🎂
      </h2>
      {friendsBirthdays.length > 0 ? (
        <div className="cards-grid">
          {friendsBirthdays.map((person) => {
            const daysLeft = calculateDaysToBirthday(person.date);
            return (
              <div key={person.id} className="dashboard-card birthday-card">
                <h3>{person.name}</h3>
                <p className="date-text">
                  <strong>Data:</strong> {person.date}
                </p>
                <div className="countdown">
                  Za <strong>{daysLeft}</strong> dni
                </div>
              </div>
            );
          })}
        </div>
      ) : (
        <div className="empty-state">
          <p>Brak znajomych na liście.</p>
          <Link to="/friends" className="empty-state-link">
            Przejdź do zakładki Znajomi
          </Link>
        </div>
      )}
    </div>
  );
};

export default Home;
