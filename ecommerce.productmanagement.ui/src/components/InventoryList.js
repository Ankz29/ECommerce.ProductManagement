// src/components/InventoryList.js
import React, { useEffect, useState } from "react";
import { getInventories, deleteInventory } from "../services/inventoryService";

const InventoryList = ({ onEdit }) => {
  const [inventories, setInventories] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      const data = await getInventories();
      setInventories(data);
    };
    fetchData();
  }, []);

  const handleDelete = async (id) => {
    await deleteInventory(id);
    setInventories(inventories.filter(i => i.id !== id));
  };

  return (
    <ul>
      {inventories.map(i => (
        <li key={i.id}>
          ProductId: {i.productId}, Quantity: {i.quantity}
          <button onClick={() => onEdit(i)}>Edit</button>
          <button onClick={() => handleDelete(i.id)}>Delete</button>
        </li>
      ))}
    </ul>
  );
};
export default InventoryList;