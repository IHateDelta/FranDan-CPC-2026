const API_BASE_URL = "https://localhost:7074";

const getHeaders = () => {
  const token = localStorage.getItem("token");
  return {
    "Content-Type": "application/json",
    ...(token ? { Authorization: `Bearer ${token}` } : {}),
  };
};

export const api = {
  auth: {
    login: async (credentials) =>
      fetch(`${API_BASE_URL}/api/auth/login`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(credentials),
      }),
    register: async (userData) =>
      fetch(`${API_BASE_URL}/api/auth/register`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(userData),
      }),
    verify: async (data) =>
      fetch(`${API_BASE_URL}/api/auth/verify`, {
        method: "PATCH",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
      }),
  },

  user: {
    getFull: async () =>
      fetch(`${API_BASE_URL}/api/user/full`, {
        method: "GET",
        headers: getHeaders(),
      }),
  },

  friends: {
    invite: async (userId) =>
      fetch(`${API_BASE_URL}/api/friend/invite`, {
        method: "POST",
        headers: getHeaders(),
        body: JSON.stringify({ userId }),
      }),
    accept: async (userId) =>
      fetch(`${API_BASE_URL}/api/friend/accept`, {
        method: "PATCH",
        headers: getHeaders(),
        body: JSON.stringify({ userId }),
      }),
    reject: async (userId) =>
      fetch(`${API_BASE_URL}/api/friend/reject`, {
        method: "PATCH",
        headers: getHeaders(),
        body: JSON.stringify({ userId }),
      }),
    delete: async (userId) =>
      fetch(`${API_BASE_URL}/api/friend/delete`, {
        method: "DELETE",
        headers: getHeaders(),
        body: JSON.stringify({ userId }),
      }),
  },

  plans: {
    create: async (planData) =>
      fetch(`${API_BASE_URL}/api/plan/create`, {
        method: "POST",
        headers: getHeaders(),
        body: JSON.stringify(planData),
      }),
    getFull: async () =>
      fetch(`${API_BASE_URL}/api/plan/full`, {
        method: "POST",
        headers: getHeaders(),
      }), // Zgodnie z dokumentacją to jest POST
  },

  participation: {
    add: async (payload) =>
      fetch(`${API_BASE_URL}/api/participation/add`, {
        method: "POST",
        headers: getHeaders(),
        body: JSON.stringify(payload),
      }),
    remove: async (payload) =>
      fetch(`${API_BASE_URL}/api/participation/delete`, {
        method: "DELETE",
        headers: getHeaders(),
        body: JSON.stringify(payload),
      }),
  },
};
