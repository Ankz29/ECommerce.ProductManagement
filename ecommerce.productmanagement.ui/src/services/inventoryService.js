import axios from "axios";

const API_URL = "https://localhost:7061/api/Inventory";

// GET all inventories
export const getInventories = async () => {
  const response = await axios.get(API_URL);
  return response.data;
};

// GET inventory by product Id
export const getInventoryByProductId = async (productId) => {
  const response = await axios.get(`${API_URL}/${productId}`);
  return response.data;
};

// POST new inventory
export const createInventory = async (inventory) => {
  const response = await axios.post(API_URL, inventory);
  return response.data;
};

// PUT update inventory
export const updateInventory = async (id, inventory) => {
  await axios.put(`${API_URL}/${id}`, inventory);
};

// DELETE inventory
export const deleteInventory = async (id) => {
  await axios.delete(`${API_URL}/${id}`);
};