import React from "react";
import ProductList from "../components/ProductLists";
import LogoutButton from "../components/LogoutButton";

const ProductPage = () => {
  return (
    <div>
      <h1>Product Management</h1>
      <ProductList />
    
      <h1>Logout</h1>
      <LogoutButton />
    </div>
  );
};

export default ProductPage;