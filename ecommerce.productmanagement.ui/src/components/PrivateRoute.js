import React from "react";
import { Navigate } from "react-router-dom";

const PrivateRoute = ({ children, role }) => {
  const token = localStorage.getItem("jwt");
  const userRole = localStorage.getItem("role")?.toLowerCase();
  const requiredRole = role?.toLowerCase();

  if (!token) return <Navigate to="/login" replace />;
  if (requiredRole && userRole !== requiredRole) {
    return <Navigate to="/products" replace />;
  }

  return children;
};

export default PrivateRoute;