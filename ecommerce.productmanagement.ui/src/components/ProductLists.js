// src/components/ProductList.js
import React, { useEffect, useState } from "react";
import { getProducts, deleteProduct } from "../services/productService";

const ProductList = ({ onEdit }) => {
  const [products, setProducts] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      const data = await getProducts();
      setProducts(data);
    };
    fetchData();
  }, []);

  const handleDelete = async (id) => {
    await deleteProduct(id);
    setProducts(products.filter(p => p.id !== id));
  };

  return (
    <table>
      <thead>
        <tr>
          <th>Name</th><th>Category</th><th>Price</th><th>Inventory</th><th>Actions</th>
        </tr>
      </thead>
      <tbody>
        {products.map(p => (
          <tr key={p.id}>
            <td>{p.name}</td>
            <td>{p.category?.name}</td>
            <td>{p.price}</td>
            <td>{p.inventory?.quantity}</td>
            <td>
              <button onClick={() => onEdit(p)}>Edit</button>
              <button onClick={() => handleDelete(p.id)}>Delete</button>
            </td>
          </tr>
        ))}
      </tbody>
    </table>
  );
};

export default ProductList;