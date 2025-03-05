import LoginForm from "../components/LoginForm";
import { useNavigate } from "react-router-dom";
import { useEffect } from "react";
//const InputField = ({ type, name, placeholder, value, onChange, error, required =true }) => {

const Login = () => {

    const navigate = useNavigate();

    useEffect(() => {
      if (localStorage.getItem("token")) {
        navigate("/home");
      }
    }, [navigate]);

    const handleLogin = async (formData) => {
        try {
            const response = await fetch("https://localhost:7210/api/auth/login",{
                method : "POST",
                headers : {"Content-Type" : "application/json"},
                body : JSON.stringify(formData),
            });
            const data = await response.json();
            
            if (response.ok) {
                if (data?.token) {
                    localStorage.setItem("token",data.token);
                    window.location.href = data.returnurl || "/home";
                }
                else {
                    alert("login failed");
                }
            }
        }
        catch (error) {
            console.log("error  :  " + error);
            alert("something went wrong");
        }
    }

    return (
        <div>
            <LoginForm onSubmit={handleLogin}/>
        </div>
    )

}

export default Login;