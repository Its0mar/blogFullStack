import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Register from "./pages/Register";
import Home from "./pages/Home";
import './App.css';
import ProtectedRoute from "./components/common/ProtectedRoute";
import Login from "./pages/Login";

function App() {
  return (
    <Router>
    <Routes>
      <Route path="/register" element={<Register />} />
      <Route path="/login" element={<Login />} />

      <Route element={<ProtectedRoute/>}>
          <Route path="/" element={<Home />} />
          <Route path="/home" element={<Home />} />
      </Route>

      
      
      
    </Routes>
  </Router>
  );
}

export default App;
