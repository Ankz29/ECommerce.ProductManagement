// src/components/CategoryForm.js
import React, { useState } from "react";
import { createCategory, updateCategory } from "../services/categoryService";

const CategoryForm = ({ category, onSaved }) => {
  const [form, setForm] = useState(category || { name: "" });

  const handleChange = (e) => setForm({ ...form, [e.target.name]: e.target.value });

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (form.id) {
      await updateCategory(form.id, form);
    } else {
      await createCategory(form);
    }
    onSaved();
  };

  return (
    <form onSubmit={handleSubmit}>
      <input name="name" value={form.name} onChange={handleChange} placeholder="Category Name" />
      <button type="submit">{form.id ? "Update" : "Add"} Category</button>
    </form>
  );
};
export default CategoryForm;