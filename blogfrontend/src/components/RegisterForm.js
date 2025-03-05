import { useState, useCallback } from "react";
import InputField from "./common/InputField";

const RegisterForm = ({ onSubmit, isLoading }) => {
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

  const handleChange = useCallback((e) => {
    const { name, value, type, checked } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: type === "checkbox" ? checked : value
    }));
    
    // Clear error when user starts typing
    if (errors[name]) {
      setErrors(prev => {
        const newErrors = {...prev};
        delete newErrors[name];
        return newErrors;
      });
    }
  }, [errors]);

  const validateForm = () => {
    let newErrors = {};
    
    // Person Name validation
    if (!formData.personName.trim()) {
      newErrors.personName = "Name is required";
    }
    
    // Username validation
    if (!formData.userName.trim()) {
      newErrors.userName = "Username is required";
    } else if (formData.userName.length < 4) {
      newErrors.userName = "Username must be at least 4 characters";
    }
    
    // Email validation
    if (!formData.email.trim()) {
      newErrors.email = "Email is required";
    } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(formData.email)) {
      newErrors.email = "Invalid email format";
    }
    
    // Password validation
    if (!formData.password) {
      newErrors.password = "Password is required";
    } else if (formData.password.length < 8) {
      newErrors.password = "Password must be at least 8 characters";
    }
    
    // Confirm password validation
    if (!formData.confirmPassword) {
      newErrors.confirmPassword = "Please confirm your password";
    } else if (formData.password !== formData.confirmPassword) {
      newErrors.confirmPassword = "Passwords do not match";
    }
    
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    
    // Prevent the default form submission which would trigger browser validation
    e.preventDefault();
    
    if (validateForm()) {
      // Remove confirmPassword before submitting
      const { confirmPassword, ...submitData } = formData;
      onSubmit(submitData);
    }
  };

  return (
    <form onSubmit={handleSubmit} className="register-form" noValidate>
      <InputField
        type="text"
        name="personName"
        label="Full Name"
        placeholder="Enter your full name"
        value={formData.personName}
        onChange={handleChange}
        error={errors.personName}
        required={false} // Remove required attribute
      />
      
      <InputField
        type="text"
        name="userName"
        label="Username"
        placeholder="Choose a username"
        value={formData.userName}
        onChange={handleChange}
        error={errors.userName}
        required={false} // Remove required attribute
      />
      
      <InputField
        type="email"
        name="email"
        label="Email"
        placeholder="Enter your email address"
        value={formData.email}
        onChange={handleChange}
        error={errors.email}
        required={false} // Remove required attribute
      />
      
      <InputField
        type="tel"
        name="phoneNumber"
        label="Phone Number (Optional)"
        placeholder="Enter your phone number"
        value={formData.phoneNumber}
        onChange={handleChange}
        required={false}
      />
      
      <InputField
        type="text"
        name="profilePicPath"
        label="Profile Picture URL (Optional)"
        placeholder="Enter profile picture URL"
        value={formData.profilePicPath}
        onChange={handleChange}
        required={false}
      />
      
      <InputField
        type="password"
        name="password"
        label="Password"
        placeholder="Create a password"
        value={formData.password}
        onChange={handleChange}
        error={errors.password}
        required={false} // Remove required attribute
      />
      
      <InputField
        type="password"
        name="confirmPassword"
        label="Confirm Password"
        placeholder="Confirm your password"
        value={formData.confirmPassword}
        onChange={handleChange}
        error={errors.confirmPassword}
        required={false} // Remove required attribute
      />
      
      <div className="form-group checkbox">
        <input
          type="checkbox"
          id="isPersistent"
          name="isPersistent"
          checked={formData.isPersistent}
          onChange={handleChange}
        />
        <label htmlFor="isPersistent">Keep me signed in</label>
      </div>
      
      <button 
        type="submit" 
        className="submit-button" 
        disabled={isLoading}
      >
        {isLoading ? "Registering..." : "Create Account"}
      </button>
    </form>
  );
};

export default RegisterForm;

// import { useState } from "react";
// import InputField from "./common/InputField";

// const RegisterForm = ({ onSubmit }) => {
//   const [formData, setFormData] = useState({
//     personName: "",
//     userName: "",
//     email: "",
//     phoneNumber: "",
//     profilePicPath: "",
//     password: "",
//     confirmPassword: "",
//     isPersistent: true,
//   });

//   const [errors, setErrors] = useState({});

//   const handleChange = (e) => {
//     setFormData({ ...formData, [e.target.name]: e.target.value });
//   };

//   const validate = () => {
//     let errors = {};
//     if (!formData.personName.trim()) errors.personName = "Person Name is required";
//     if (!formData.userName.trim()) errors.userName = "User Name is required";
//     if (!formData.email.match(/^[^\s@]+@[^\s@]+\.[^\s@]+$/)) errors.email = "Invalid Email";
//     if (formData.password !== formData.confirmPassword) errors.confirmPassword = "Passwords do not match";
//     setErrors(errors);
//     return Object.keys(errors).length === 0;
//   };

//   const handleSubmit = (e) => {
//     e.preventDefault();
//     if (validate()) onSubmit(formData);
//   };

//   return (
//     <form onSubmit={handleSubmit}>
//       <InputField type="text" name="personName" placeholder="Person Name" value={formData.personName} onChange={handleChange} error={errors.personName} />
//       <InputField type="text" name="userName" placeholder="User Name" value={formData.userName} onChange={handleChange} error={errors.userName} />
//       <InputField type="email" name="email" placeholder="Email" value={formData.email} onChange={handleChange} error={errors.email} />
//       <InputField type="text" name="phoneNumber" placeholder="Phone Number (Optional)" value={formData.phoneNumber} onChange={handleChange} required={false} />
//       <InputField type="text" name="profilePicPath" placeholder="Profile Picture URL (Optional)" value={formData.profilePicPath} onChange={handleChange} required={false} />
//       <InputField type="password" name="password" placeholder="Password" value={formData.password} onChange={handleChange} error={errors.password} />
//       <InputField type="password" name="confirmPassword" placeholder="Confirm Password" value={formData.confirmPassword} onChange={handleChange} error={errors.confirmPassword} />
      
//       <label>
//         <input type="checkbox" name="isPersistent" checked={formData.isPersistent} onChange={() => setFormData({ ...formData, isPersistent: !formData.isPersistent })} />
//         Keep me signed in
//       </label>
      
//       <button type="submit">Register</button>
//     </form>
//   );
// };

// export default RegisterForm;
