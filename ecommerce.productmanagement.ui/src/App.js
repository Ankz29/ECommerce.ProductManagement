import React from "react";
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import ProductPage from "./pages/ProductPage";
import Login from "./pages/Login";
import ProductForm from "./components/ProductForm";
import PrivateRoute from "./components/PrivateRoute";
import { AuthProvider } from "./context/AuthContext";

function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<Navigate to="/login" replace />} />
          <Route path="/login" element={<Login />} />

          <Route
            path="/products"
            element={
              <PrivateRoute>
                <ProductPage />
              </PrivateRoute>
            }
          />

          <Route
            path="/create"
            element={
              <PrivateRoute role="Admin">
                <ProductForm />
              </PrivateRoute>
            }
          />
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  );
}

export default App;