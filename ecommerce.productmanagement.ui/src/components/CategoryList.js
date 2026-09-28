// src/components/CategoryList.js
import React, { useEffect, useState } from "react";
import { getCategories, deleteCategory } from "../services/categoryService";

const CategoryList = ({ onEdit }) => {
  const [categories, setCategories] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      const data = await getCategories();
      setCategories(data);
    };
    fetchData();
  }, []);

  const handleDelete = async (id) => {
    await deleteCategory(id);
    setCategories(categories.filter(c => c.id !== id));
  };

  return (
    <ul>
      {categories.map(c => (
        <li key={c.id}>
          {c.name}
          <button onClick={() => onEdit(c)}>Edit</button>
          <button onClick={() => handleDelete(c.id)}>Delete</button>
        </li>
      ))}
    </ul>
  );
};
export default CategoryList;