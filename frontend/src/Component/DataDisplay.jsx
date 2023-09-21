import React, { useEffect, useState } from "react";
import axios from "axios";

const DataDisplay = ({ userId }) => {
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [selectedItem, setSelectedItem] = useState(null);
  const [formData, setFormData] = useState({
    title: "",
    contentType: "",
  });
  const [selectedMultiItems, setSelectedMultiItems] = useState([]);

  const handleInputChange = (event) => {
    const { name, value } = event.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleItemSelect = (itemId) => {
    if (selectedMultiItems.includes(itemId)) {
      setSelectedMultiItems(selectedMultiItems.filter((id) => id !== itemId));
    } else {
      setSelectedMultiItems([...selectedMultiItems, itemId]);
    }
  };

  const formDataToSend = new FormData();
  formDataToSend.append("id", formData.id);
  formDataToSend.append("title", formData.title);
  formDataToSend.append("contentType", formData.contentType);

  const toggleUpdateForm = (item) => {
    setSelectedItem(item);
    setFormData({
      id: item.id,
      title: item.title,
      contentType: item.contentType,
    });
  };

  useEffect(() => {
    axios
      .get(
        `https://localhost:7160/api/Documents/mydocuments?studentId=${userId}`
      )
      .then((response) => {
        setData(response.data);
        setLoading(false);
      })
      .catch((error) => {
        console.error("Error fetching data:", error);
        setLoading(false);
      });
  }, [userId]);

  const handleUpdateItem = async (e) => {
    e.preventDefault();
    axios
      .put(
        `https://localhost:7160/api/Documents/${formData.id}`,
        formDataToSend
      )
      .then((response) => {
        const updatedData = data.map((item) => {
          if (item.id === formData.id) {
            return response.data;
          }
          return item;
        });
        setData(updatedData);

        setFormData({ title: "", contentType: "" });
        setSelectedItem(null);
      })
      .catch((error) => {
        console.error("Error updating item:", error);
        console.log(formData);
      });
  };

  const handleDeleteItem = (itemId) => {
    axios
      .delete(`https://localhost:7160/api/Documents/${itemId}`)
      .then(() => {
        const updatedData = data.filter((item) => item.id !== itemId);
        setData(updatedData);
      })
      .catch((error) => {
        console.error("Error deleting item:", error);
      });
  };

  const handleDeleteMultipleItems = () => {
    axios
      .delete("https://localhost:7160/api/Documents/delete-multiple", {
        selectedMultiItems,
      })
      .then(() => {
        const updatedData = data.filter(
          (item) => !selectedMultiItems.includes(item.id)
        );
        setData(updatedData);
        setSelectedMultiItems([]);
      })
      .catch((error) => {
        console.error("Error deleting multiple items:", error);
        console.log({ selectedMultiItems });
      });
  };

  const handleDeleteAllItems = () => {
    axios
      .delete(`https://localhost:7160/api/Documents/delete-all/${userId}`)
      .then(() => {
        setData([]);
        setLoading(true);
      })
      .catch((error) => {
        console.error("Error deleting all items:", error);
      });
  };

  return (
    <div>
      <h2>Document Details</h2>
      {loading ? (
        <p className="danger">Empty...</p>
      ) : (
        <div>
          <table className="data-table">
            <thead>
              <tr>
                <th>Document Id</th>
                <th>Title</th>
                <th>Content Type</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {data.map((item) => (
                <tr key={item.id}>
                  <td>{item.id}</td>
                  <td>{item.title}</td>
                  <td>{item.contentType}</td>
                  <td>
                    <button onClick={() => toggleUpdateForm(item)}>Edit</button>
                  </td>
                  <td>
                    <input
                      type="checkbox"
                      checked={selectedMultiItems.includes(item.id)}
                      onChange={() => handleItemSelect(item.id)}
                    />
                    <button
                      className="delete"
                      onClick={() => handleDeleteItem(item.id)}>
                      Delete
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
          <button
            className="delete"
            onClick={handleDeleteMultipleItems}>
            Delete Selected
          </button>
          <button
            className="delete"
            onClick={handleDeleteAllItems}>
            Delete All
          </button>
          {selectedItem && (
            <div>
              <h3>Edit Item</h3>
              <form
                className="form-contianer"
                onSubmit={handleUpdateItem}>
                <input
                  name="id"
                  value={formData.id}
                  onChange={handleInputChange}
                />
                <label>Title</label>
                <input
                  type="text"
                  name="title"
                  placeholder="Title"
                  value={formData.title}
                  onChange={handleInputChange}
                />
                <label>Content Type</label>
                <input
                  type="text"
                  name="contentType"
                  placeholder="Content Type"
                  value={formData.contentType}
                  onChange={handleInputChange}
                />
                <button type="submit">Update</button>
              </form>
            </div>
          )}
        </div>
      )}
    </div>
  );
};

export default DataDisplay;
