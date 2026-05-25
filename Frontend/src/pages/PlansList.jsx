import { useState, useContext } from "react";
import { AuthContext } from "../context/AuthContext";
import { PlansContext } from "../context/PlansContext";
import { Navigate, Link } from "react-router-dom";
import "./PlansList.css";

const PlansList = () => {
  const [filter, setFilter] = useState("all");
  const { token } = useContext(AuthContext);
  const { plans, fetchPlans, loading, deletePlan, updatePlan } =
    useContext(PlansContext);

  const [editingPlanId, setEditingPlanId] = useState(null);
  const [editFormData, setEditFormData] = useState({
    title: "",
    date: "",
    category: "",
  });

  if (!token) {
    return <Navigate to="/login" replace />;
  }

  const filteredPlans = plans.filter((plan) => {
    if (filter === "private") {
      return plan.participants.length === 1;
    }
    if (filter === "shared") {
      return plan.participants.length > 1;
    }
    return true;
  });

  const handleEditClick = (plan) => {
    setEditingPlanId(plan.id);
    setEditFormData({
      title: plan.title,
      date: plan.date,
      category: plan.category,
    });
  };

  const handleSaveEdit = (planId) => {
    updatePlan(planId, editFormData);
    setEditingPlanId(null);
  };

  const handleDelete = (planId) => {
    if (window.confirm("Czy na pewno chcesz usunąć ten plan?")) {
      deletePlan(planId);
    }
  };

  if (loading) {
    return (
      <div
        className="plans-container"
        style={{ textAlign: "center", paddingTop: "50px" }}
      >
        <div className="spinner"></div>
        <p style={{ color: "#868e96", marginTop: "10px" }}>
          Ładowanie planów...
        </p>
      </div>
    );
  }

  return (
    <div className="plans-container">
      <h2>Harmonogram Planów</h2>
      <div className="filters">
        <button
          className={filter === "all" ? "active" : ""}
          onClick={() => setFilter("all")}
        >
          Wszystkie
        </button>
        <button
          className={filter === "private" ? "active" : ""}
          onClick={() => setFilter("private")}
        >
          Prywatne
        </button>
        <button
          className={filter === "shared" ? "active" : ""}
          onClick={() => setFilter("shared")}
        >
          Wspólne
        </button>
      </div>

      <div className="plans-list">
        {filteredPlans.map((plan) => (
          <div key={plan.id} className="plan-item">
            {editingPlanId === plan.id ? (
              <div className="edit-mode">
                <input
                  type="text"
                  value={editFormData.title}
                  onChange={(e) =>
                    setEditFormData({ ...editFormData, title: e.target.value })
                  }
                />
                <input
                  type="datetime-local"
                  value={editFormData.date}
                  onChange={(e) =>
                    setEditFormData({ ...editFormData, date: e.target.value })
                  }
                />
                <select
                  value={editFormData.category}
                  onChange={(e) =>
                    setEditFormData({
                      ...editFormData,
                      category: e.target.value,
                    })
                  }
                >
                  <option value="sport">Sport</option>
                  <option value="nauka">Nauka</option>
                  <option value="zwiedzanie">Zwiedzanie</option>
                  <option value="impreza">Impreza</option>
                </select>
                <div className="action-buttons">
                  <button
                    onClick={() => handleSaveEdit(plan.id)}
                    className="btn-save"
                  >
                    Zapisz
                  </button>
                  <button
                    onClick={() => setEditingPlanId(null)}
                    className="btn-cancel"
                  >
                    Anuluj
                  </button>
                </div>
              </div>
            ) : (
              <>
                <div className="plan-header">
                  <h3>{plan.title}</h3>
                  <span className={`badge badge-${plan.category}`}>
                    {plan.category}
                  </span>
                </div>
                <div className="plan-details">
                  <p>
                    <strong>Kiedy:</strong> {plan.date}
                  </p>
                  <p>
                    <strong>Z kim:</strong>{" "}
                    {plan.participants.length === 1 &&
                    plan.participants[0] === "Ja"
                      ? "Tylko ja"
                      : plan.participants.join(", ")}
                  </p>
                </div>
                <div className="action-buttons-right">
                  <button
                    onClick={() => handleEditClick(plan)}
                    className="btn-edit"
                  >
                    Edytuj
                  </button>
                  <button
                    onClick={() => handleDelete(plan.id)}
                    className="btn-delete"
                  >
                    Usuń
                  </button>
                </div>
              </>
            )}
          </div>
        ))}
      </div>
    </div>
  );
};

export default PlansList;
