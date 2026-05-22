import { createContext, useEffect, useState } from "react";
import { api } from "../services/api";

export const PlansContext = createContext();

export const PlansProvider = ({ children }) => {
  const [plans, setPlans] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchPlans = async () => {
      try {
        setLoading(true);
        const data = await api.plans.getAll();
        setPlans(data);
      } catch (error) {
        console.error("Błąd podczas pobierania planów:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchPlans();
  }, []);

  const addPlan = async (newPlan) => {
    const updatedPlans = [...plans, newPlan];
    setPlans(updatedPlans);
    await api.plans.saveAll(updatedPlans);
  };

  const deletePlan = async (planId) => {
    const updatedPlans = plans.filter((plan) => plan.id !== planId);
    setPlans(updatedPlans);
    await api.plans.saveAll(updatedPlans);
  };

  const updatePlan = async (planId, updatedData) => {
    const updatedPlans = plans.map((plan) =>
      plan.id === planId ? { ...plan, ...updatedData } : plan,
    );
    setPlans(updatedPlans);
    await api.plans.saveAll(updatedPlans);
  };

  return (
    <PlansContext.Provider
      value={{ plans, loading, addPlan, deletePlan, updatePlan }}
    >
      {children}
    </PlansContext.Provider>
  );
};
