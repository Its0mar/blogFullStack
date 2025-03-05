import React from "react";

const InputField = ({ 
  type, 
  name, 
  label, 
  placeholder, 
  value, 
  onChange,
  error, 
  required = false // Changed default to false
}) => {
  return (
    <div className="form-group">
      {label && (
        <label htmlFor={name}>
          {label}
          {required && <span className="required">*</span>}
        </label>
      )}
      <input
        id={name}
        type={type}
        name={name}
        placeholder={placeholder}
        value={value}
        onChange={onChange}
        className={error ? "input-error" : ""}
        // Remove required attribute completely
      />
      {error && (
        <p className="error-message">
          {error}
        </p>
      )}
    </div>
  );
};

export default React.memo(InputField);

// const InputField = ({ type, name, placeholder, value, onChange, error, required =true }) => {
//     return (
//       <div>
//         <input type={type} name={name} placeholder={placeholder} value={value} onChange={onChange} required={required} />
//         {error && <p style={{ color: "red" }}>{error}</p>}
//       </div>
//     );
//   };
  
//   export default InputField;
  