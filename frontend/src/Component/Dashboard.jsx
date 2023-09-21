import React, { useState } from "react";
import { Link } from "react-router-dom";
import jwt_decode from "jwt-decode";
import axios from "axios";
import DataDisplay from "./DataDisplay";

const Dashboard = ({ token, onLogout }) => {
  const getUserIdFromToken = () => {
    try {
      const decodedToken = jwt_decode(token);
      return decodedToken.unique_name;
    } catch (error) {
      console.error("Error decoding token:", error);
      return "";
    }
  };
  const [viewData, setViewData] = useState(false);

  const handleViewDataClick = () => {
    setViewData(true);
  };

  const userId = getUserIdFromToken();

  const [formData, setFormData] = useState({
    title: "",
    file: null,
  });

  const handleInputChange = (e) => {
    const { name, value, files } = e.target;

    const newValue = name === "file" ? files[0] : value;

    setFormData({ ...formData, [name]: newValue });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const formDataToSend = new FormData();
    formDataToSend.append("studentId", userId);
    formDataToSend.append("title", formData.title);
    formDataToSend.append("file", formData.file);

    try {
      const response = await axios.post(
        "https://localhost:7160/api/Documents/upload",
        formDataToSend,
        {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        }
      );
      if (response.status === 200) {
        console.log("Form submission response:", response.data);
        setFormData({ title: "", file: null });
        setViewData(false);
      }
    } catch (error) {
      console.error("Form submission error:", error);
    }
  };

  return (
    <div>
      {token ? (
        <div className="container">
          <h2>Welcome, {userId}!</h2>

          <button onClick={onLogout}>Logout</button>

          <form onSubmit={handleSubmit}>
            <input
              type="hidden"
              name="studentId"
              value={userId}
            />

            <div>
              <label>Title:</label>
              <input
                type="text"
                name="title"
                value={formData.title}
                onChange={handleInputChange}
              />
            </div>

            <div>
              <label>Upload File:</label>
              <input
                type="file"
                name="file"
                onChange={handleInputChange}
              />
            </div>

            <button type="submit">Upload</button>
          </form>
          <button onClick={handleViewDataClick}>View Data</button>
          {viewData && <DataDisplay userId={userId} />}
        </div>
      ) : (
        <div>
          <h2>Unauthorized</h2>
          <p>
            <Link to="/login">Login</Link> to access the dashboard.
          </p>
        </div>
      )}
    </div>
  );
};

export default Dashboard;
