const delay = (ms = 800) => new Promise((resolve) => setTimeout(resolve, ms));

export const api = {
  plans: {
    getAll: async () => {
      await delay(800);
      const savedPlans = localStorage.getItem("user_plans");
      if (savedPlans) {
        return JSON.parse(savedPlans);
      }
      return [
        {
          id: 1,
          title: "Ciężki trening siłowy",
          category: "sport",
          date: "2026-05-18 18:00",
          participants: ["Ja"],
        },
        {
          id: 2,
          title: "Raport - Układy dynamiczne",
          category: "nauka",
          date: "2026-05-20 10:00",
          participants: ["Ja", "Dawid Podsiadło"],
        },
        {
          id: 3,
          title: "Wyjazd na Stawy Milickie",
          category: "zwiedzanie",
          date: "2026-05-24 08:00",
          participants: ["Ja", "Dawid Podsiadło"],
        },
        {
          id: 4,
          title: "Próba taneczna Iskry",
          category: "impreza",
          date: "2026-05-26 19:30",
          participants: ["Ja"],
        },
      ];
    },

    saveAll: async (updatedPlans) => {
      await delay(800);
      localStorage.setItem("user_plans", JSON.stringify(updatedPlans));
      return { success: true, message: "Plany zostały zapisane" };
    },
  },

  friends: {
    getAllUsers: async () => {
      await delay(800);
      return [
        { id: 101, name: "Dawid Podsiadło", role: "Student PWr", avatar: "D" },
        {
          id: 102,
          name: "Jakub Grzegorczyk",
          role: "Student PWr",
          avatar: "J",
        },
        { id: 103, name: "Jan Kowalski", role: "Student", avatar: "J" },
        { id: 104, name: "Anna Nowak", role: "Student", avatar: "A" },
        { id: 105, name: "Piotr Wiśniewski", role: "Absolwent", avatar: "P" },
      ];
    },

    getStatuses: async (userName) => {
      await delay(800);
      const savedStatuses = localStorage.getItem(`friends_${userName}`);
      return savedStatuses ? JSON.parse(savedStatuses) : {};
    },

    saveStatuses: async (userName, statuses) => {
      await delay(300);
      localStorage.setItem(`friends_${userName}`, JSON.stringify(statuses));
      return { success: true, message: "Statusy znajomych zostały zapisane" };
    },
  },
};
