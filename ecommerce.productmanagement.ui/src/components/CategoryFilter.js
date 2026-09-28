// src/components/CategoryFilter.js
import React from "react";

const CategoryFilter = ({ categories, onFilter }) => {
  return (
    <select onChange={(e) => onFilter(e.target.value)}>
      <option value="">All Categories</option>
      {categories.map(c => (
        <option key={c.id} value={c.id}>{c.name}</option>
      ))}
    </select>
  );
};

export default CategoryFilter;