import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

function Home() {
  const [message, setMessage] = useState("Loading...");
  const navigate = useNavigate();

  useEffect(() => {
    const token = localStorage.getItem("token");
    console.log("token : " + token);
    if (!token) {
      navigate("/login"); // Redirect to login if not authenticated
    } else {
      setMessage("Welcome to the Home Page! 🎉");
    }
  }, [navigate]);

  return <div>{message}</div>;
}

export default Home;
