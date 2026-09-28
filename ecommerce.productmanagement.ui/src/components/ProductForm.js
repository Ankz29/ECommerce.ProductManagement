// src/components/ProductForm.js
import React, { useState } from "react";
import { createProduct, updateProduct } from "../services/productService";

const ProductForm = ({ product, onSaved }) => {
  const [form, setForm] = useState(product || { name: "", description: "", price: 0, categoryId: "", quantity: 0 });

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (form.id) {
      await updateProduct(form.id, form);
    } else {
      await createProduct(form);
    }
    onSaved();
  };

  return (
    <form onSubmit={handleSubmit}>
      <input name="name" value={form.name} onChange={handleChange} placeholder="Name" />
      <input name="description" value={form.description} onChange={handleChange} placeholder="Description" />
      <input name="price" type="number" value={form.price} onChange={handleChange} placeholder="Price" />
      <input name="categoryId" value={form.categoryId} onChange={handleChange} placeholder="Category Id" />
      <input name="quantity" type="number" value={form.quantity} onChange={handleChange} placeholder="Inventory Quantity" />
      <button type="submit">{form.id ? "Update" : "Add"} Product</button>
    </form>
  );
};

export default ProductForm;