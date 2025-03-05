import { useState } from "react";

function Register() {
  const [formData, setFormData] = useState({
    personName: "",
    userName: "",
    email: "",
    phoneNumber: "",
    profilePicPath: "",
    password: "",
    confirmPassword: "",
    isPersistent: true,
  });

  const [errors, setErrors] = useState({});

  // Handle input changes
  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  // Validate inputs
  const validate = () => {
    let errors = {};
    
    if (!formData.personName.trim()) errors.personName = "Person Name is required";
    if (!formData.userName.trim()) errors.userName = "User Name is required";
    
    if (!formData.email.trim()) {
      errors.email = "Email is required";
    } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(formData.email)) {
      errors.email = "Invalid Email";
    }
    
    if (formData.phoneNumber && !/^\d{10,15}$/.test(formData.phoneNumber)) {
      errors.phoneNumber = "Invalid Phone Number";
    }

    if (!formData.password.trim()) {
      errors.password = "Password is required";
    }

    if (!formData.confirmPassword.trim()) {
      errors.confirmPassword = "Confirm Password is required";
    } else if (formData.password !== formData.confirmPassword) {
      errors.confirmPassword = "Passwords do not match";
    }

    setErrors(errors);
    return Object.keys(errors).length === 0;
  };

  // Handle form submission
  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!validate()) return;

    const response = await fetch("https://localhost:7210/api/auth/register", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(formData),
    });
    // const textResponse = await response.text();
    // console.log("Raw response:", textResponse); // Debugging
    // const data = JSON.parse(textResponse);
    const data = await response.json();
    if (response.ok) {

        if (data.token) {
            localStorage.setItem("token", data.token);
        }
        if (data.returnurl) {
            window.location.href = data.returnurl;
        }
        else {
            window.location.href = "/home";
        }
        console.log(data.returnurl + " token\n" + data.token);
      alert("Registration successful!");
    } else {
      console.error("Error:", data);
      alert("Registration failed!");
    }
  };

  const handleLogout = async () => {
    try {
      const token = localStorage.getItem("token"); // Get JWT token
  
      // Call the API to logout (if required)
      await fetch("https://localhost:7210/api/auth/logout", {
        method: "GET",
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
  
      // Remove JWT from localStorage
      localStorage.removeItem("token");
  
      // Redirect to login page
      window.location.href = "/login";
    } catch (error) {
      console.error("Logout failed", error);
    }
  };
  

  return (
    <div>
      <h2>Register</h2>
      <form onSubmit={handleSubmit}>
        <input type="text" name="personName" placeholder="Person Name" onChange={handleChange} required />
        {errors.personName && <p>{errors.personName}</p>}
        
        <input type="text" name="userName" placeholder="User Name" onChange={handleChange} required />
        {errors.userName && <p>{errors.userName}</p>}

        <input type="email" name="email" placeholder="Email" onChange={handleChange} required />
        {errors.email && <p>{errors.email}</p>}

        <input type="text" name="phoneNumber" placeholder="Phone Number (Optional)" onChange={handleChange} />
        {errors.phoneNumber && <p>{errors.phoneNumber}</p>}

        <input type="text" name="profilePicPath" placeholder="Profile Picture URL (Optional)" onChange={handleChange} />

        <input type="password" name="password" placeholder="Password" onChange={handleChange} required />
        {errors.password && <p>{errors.password}</p>}

        <input type="password" name="confirmPassword" placeholder="Confirm Password" onChange={handleChange} required />
        {errors.confirmPassword && <p>{errors.confirmPassword}</p>}

        <label>
          <input type="checkbox" name="isPersistent" checked={formData.isPersistent} onChange={() => setFormData({ ...formData, isPersistent: !formData.isPersistent })} />
          Keep me signed in
        </label>

        <button type="submit">Register</button>
        <button onClick={handleLogout}>Logout</button>
      </form>
    </div>
  );
}



export default Register;
