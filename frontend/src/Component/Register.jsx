import React, { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

const RegistrationForm = () => {
  const [formData, setFormData] = useState({
    studentId: "",
    studentName: "",
    password: "",
  });

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleRegistration = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post(
        "https://localhost:7160/api/Students/register",
        formData
      );
      if (response.status === 200) {
        alert("Registration Complete");
        setFormData({ studentId: "", studentName: "", password: "" });
      }
    } catch (error) {
      console.error("Registration failed", error);
    }
  };

  return (
    <div>
      <h2>Registration</h2>
      <form onSubmit={handleRegistration}>
        <input
          type="text"
          name="studentId"
          placeholder="studerntId"
          value={formData.studentId}
          onChange={handleInputChange}
        />
        <input
          type="text"
          name="studentName"
          placeholder="studerntName"
          value={formData.studentName}
          onChange={handleInputChange}
        />
        <input
          type="password"
          name="password"
          placeholder="Password"
          value={formData.password}
          onChange={handleInputChange}
        />
        <button type="submit">Register</button>
      </form>
    </div>
  );
};

export default RegistrationForm;
