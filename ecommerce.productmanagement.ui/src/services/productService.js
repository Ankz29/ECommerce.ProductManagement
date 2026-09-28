import axios from "axios";

const API_URL = "https://localhost:7061/api/Products";

// GET all products
export const getProducts = async () => {
  const response = await axios.get(API_URL);
  return response.data;
};

// POST new product
export const createProduct = async (product) => {
  const response = await axios.post(API_URL, product);
  return response.data;
};

// PUT update product
export const updateProduct = async (id, product) => {
  await axios.put(`${API_URL}/${id}`, product);
};

// DELETE product
export const deleteProduct = async (id) => {
  await axios.delete(`${API_URL}/${id}`);
};