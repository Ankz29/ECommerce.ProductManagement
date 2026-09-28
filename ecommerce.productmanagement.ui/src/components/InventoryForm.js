// src/components/InventoryForm.js
import React, { useState } from "react";
import { createInventory, updateInventory } from "../services/inventoryService";

const InventoryForm = ({ inventory, onSaved }) => {
  const [form, setForm] = useState(inventory || { productId: "", quantity: 0 });

  const handleChange = (e) => setForm({ ...form, [e.target.name]: e.target.value });

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (form.id) {
      await updateInventory(form.id, form);
    } else {
      await createInventory(form);
    }
    onSaved();
  };

  return (
    <form onSubmit={handleSubmit}>
      <input name="productId" value={form.productId} onChange={handleChange} placeholder="Product Id" />
      <input name="quantity" type="number" value={form.quantity} onChange={handleChange} placeholder="Quantity" />
      <button type="submit">{form.id ? "Update" : "Add"} Inventory</button>
    </form>
  );
};
export default InventoryForm;