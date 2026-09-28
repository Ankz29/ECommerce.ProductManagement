import axios from "axios";

const API_URL = "https://localhost:7061/api/Categories";

// GET all categories
export const getCategories = async () => {
  const response = await axios.get(API_URL);
  return response.data;
};

// GET category by Id
export const getCategoryById = async (id) => {
  const response = await axios.get(`${API_URL}/${id}`);
  return response.data;
};

// POST new category
export const createCategory = async (category) => {
  const response = await axios.post(API_URL, category);
  return response.data;
};

// PUT update category
export const updateCategory = async (id, category) => {
  await axios.put(`${API_URL}/${id}`, category);
};

// DELETE category
export const deleteCategory = async (id) => {
  await axios.delete(`${API_URL}/${id}`);
};