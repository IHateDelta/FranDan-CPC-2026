import { createContext, useState, useEffect, useContext } from "react";
import { api } from "../services/api";
import { AuthContext } from "./AuthContext";
import { ToastContext } from "./ToastContext";

export const PlansContext = createContext();

export const PlansProvider = ({ children }) => {
  const { token } = useContext(AuthContext);
  const { addToast } = useContext(ToastContext);
  const [plans, setPlans] = useState([]);
  const [loading, setLoading] = useState(false);

  // 1. POBIERANIE PLANÓW Z BACKENDU
  const fetchPlans = async () => {
    if (!token) return;
    setLoading(true);
    try {
      const response = await api.plans.getFull();

      if (response.ok) {
        const data = await response.json();

        const mappedPlans = data.map((plan) => ({
          ...plan,
          date: plan.startTime ? plan.startTime.replace("T", " ") : "",
          participants: plan.participants || ["Ja"],
        }));

        setPlans(mappedPlans);
      }
    } catch (error) {
      console.error("Błąd pobierania planów:", error);
      addToast("Błąd! Nie udało się pobrać planów z serwera.", "error");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchPlans();
  }, [token]);

  const addPlan = async (newPlan) => {
    const tempPlan = { ...newPlan, id: Date.now() };
    setPlans((prev) => [...prev, tempPlan]);

    try {
      const payload = {
        title: newPlan.title,
        category: newPlan.category,
        startTime: newPlan.date.replace(" ", "T"),
        description: "",
      };

      const response = await api.plans.create(payload);

      if (!response.ok) {
        throw new Error("Błąd API podczas tworzenia planu");
      }

      fetchPlans();
    } catch (error) {
      console.error("Błąd zapisu planu:", error);
      setPlans((prev) => prev.filter((p) => p.id !== tempPlan.id));
      addToast("Błąd serwera. Nie udało się zapisać planu.", "error");
    }
  };

  const updatePlan = (planId, updatedData) => {
    setPlans(
      plans.map((plan) =>
        plan.id === planId ? { ...plan, ...updatedData } : plan,
      ),
    );
    addToast("Zaktualizowano plan (tylko lokalnie).", "info");
  };

  const deletePlan = (planId) => {
    setPlans(plans.filter((plan) => plan.id !== planId));
    addToast("Usunięto plan (tylko lokalnie).", "info");
  };

  return (
    <PlansContext.Provider
      value={{ plans, loading, addPlan, deletePlan, updatePlan, fetchPlans }}
    >
      {children}
    </PlansContext.Provider>
  );
};
