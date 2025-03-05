import { Navigate, Outlet } from "react-router-dom";
// outlet : "Render whatever child route is currently active inside this component."
const ProtectedRoute = () => {

    const token = localStorage.getItem("token");
    return token ? <Outlet/> : <Navigate to="/login"></Navigate>

}
export default ProtectedRoute;