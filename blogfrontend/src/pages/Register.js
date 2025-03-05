import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import RegisterForm from "../components/RegisterForm";
import { toast } from "react-toastify"; // Assuming you have a toast library installed

const Register = () => {
  const navigate = useNavigate();
  const [isLoading, setIsLoading] = useState(false);
  const [apiError, setApiError] = useState("");

  useEffect(() => {
    // Check if user is already authenticated
    const token = localStorage.getItem("token");
    if (token) {
      navigate("/home", { replace: true });
    }
  }, [navigate]);

  const handleRegister = async (formData) => {
    setIsLoading(true);
    setApiError("");
    try {
      // Log the data being sent to help debug API issues
      console.log("Sending registration data:", formData);
      
      const response = await fetch("https://localhost:7210/api/auth/register", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(formData),
      });

      const data = await response.json();
      
      // Log the full server response to debug
      console.log("Server response:", await response.status, data);

      if (response.ok) {
        localStorage.setItem("token", data.token);
        toast.success("Registration successful!");
        // Use navigate instead of window.location for better React integration
        navigate(data.returnurl || "/home", { replace: true });
      } else {
        // Store the error from API to display in the form
        setApiError(data.message || data.error || "Registration failed. Please check your information.");
        toast.error(data.message || "Registration failed");
      }
    } catch (error) {
      console.error("Registration error:", error);
      setApiError("Connection error. Please try again later.");
      toast.error("Connection error. Please try again later.");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="register-container">
      <h2>Create Your Account</h2>
      {apiError && <div className="api-error-message">{apiError}</div>}
      <RegisterForm onSubmit={handleRegister} isLoading={isLoading} />
    </div>
  );
};

export default Register;

// import { useEffect } from "react";
// import { useNavigate } from "react-router-dom";
// import RegisterForm from "../components/RegisterForm";

// const Register = () => {
//   const navigate = useNavigate();

//   useEffect(() => {
//     if (localStorage.getItem("token")) {
//       navigate("/home");
//     }
//   }, [navigate]);

//   const handleRegister = async (formData) => {
//     try {
//       const response = await fetch("https://localhost:7210/api/auth/register", {
//         method: "POST",
//         headers: { "Content-Type": "application/json" },
//         body: JSON.stringify(formData),
//       });

//       const data = await response.json();

//       if (response.ok) {
//         localStorage.setItem("token", data.token);
//         window.location.href = data.returnurl || "/home";
//       } else {
//         alert("Registration failed!");
//       }
//     } catch (error) {
//       console.error("Error:", error);
//       alert("Something went wrong!");
//     }
//   };

//   return (
//     <div>
//       <h2>Register</h2>
//       <RegisterForm onSubmit={handleRegister} />
//     </div>
//   );
// };

// export default Register;

