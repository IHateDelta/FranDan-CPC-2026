import { BrowserRouter, Routes, Route } from "react-router-dom";
import { AuthProvider } from "./context/AuthContext";
import { PlansProvider } from "./context/PlansContext";
import { FriendsProvider } from "./context/FriendsContext";
import { ToastProvider } from "./context/ToastContext";
import Navbar from "./components/Navbar";

import Home from "./pages/Home";
import PlansList from "./pages/PlansList";
import AddPlan from "./pages/AddPlan";
import Profile from "./pages/Profile";
import Login from "./pages/Login";
import Friends from "./pages/Friends";

function App() {
  return (
    <ToastProvider>
      <AuthProvider>
        <FriendsProvider>
          <PlansProvider>
            <BrowserRouter>
              <Navbar />
              <div style={{ padding: "20px" }}>
                <Routes>
                  <Route path="/" element={<Home />} />
                  <Route path="/plans" element={<PlansList />} />
                  <Route path="/add-plan" element={<AddPlan />} />
                  <Route path="/profile" element={<Profile />} />
                  <Route path="/login" element={<Login />} />
                  <Route path="/friends" element={<Friends />} />
                </Routes>
              </div>
            </BrowserRouter>
          </PlansProvider>
        </FriendsProvider>
      </AuthProvider>
    </ToastProvider>
  );
}

export default App;
