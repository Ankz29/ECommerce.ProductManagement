import React, { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

const Login = () => {
  const [form, setForm] = useState({
    username: "",
    password: ""
  });

  const navigate = useNavigate();

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      const response = await axios.post(
        "https://localhost:7061/api/Auth/login",
        form
      );

      const token = response.data.token || response.data.jwt;
      const role = response.data.role || "Admin";

      localStorage.setItem("jwt", token);
      localStorage.setItem("role", role);

      navigate("/products");
    } catch (err) {
      console.error("Login failed:", err.response?.data || err.message);
      alert(err.response?.data?.message || "Invalid credentials");
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <h2>Admin Login</h2>
      <input
  name="username"
  value={form.username}
  onChange={handleChange}
  placeholder="Username"
/>

<input
  name="password"
  type="password"
  value={form.password}
  onChange={handleChange}
  placeholder="Password"
/>
      <button type="submit">Login</button>
    </form>
  );
};

export default Login;