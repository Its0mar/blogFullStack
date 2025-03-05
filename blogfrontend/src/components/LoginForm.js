import { useState } from "react";
import InputField from "./common/InputField";

const LoginForm = ({onSubmit}) => {

    const [formData, setFormData] = useState({
        userNameOrEmail : "",
        password : "",
        rememberMe : true,
    });

    const [errors, setErrors] = useState({});

    const handleChange = (e) => {
        setFormData({...formData, [e.target.name] : e.target.value});            
    }

    const validate = () => {
        let errors = {};
        if (!formData.userNameOrEmail.trim()) errors.userNameOrEmail = "Email Or User Name is required";
        if (!formData.password.trim()) errors.password = "Password is required";
        setErrors(errors);
        return Object.keys(errors).length === 0;
    }

    const handleSubmit = (e) => {
        e.preventDefault();
        if (validate()) onSubmit(formData)
    }

    return (
        <form onSubmit={handleSubmit}>
            <InputField onChange={handleChange} type="text" name="userNameOrEmail" placeholder="type user name or email" value={formData.userNameOrEmail} />
            <InputField onChange={handleChange} type="password" name="password" placeholder="password" value={formData.password}/>
            <label>
                <input type="checkbox" name="isPersistent" checked={formData.isPersistent} onChange={() => setFormData({ ...formData, isPersistent: !formData.isPersistent })} />
                     Keep me signed in
                 </label>
            <button type="submit">submit</button>
        </form>
    )
}

export default LoginForm;