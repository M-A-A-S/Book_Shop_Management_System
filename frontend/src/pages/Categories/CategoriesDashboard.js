import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import Button from "../../components/Button";
import Dialog from "../../components/Dialog";
import Alert from "../../components/Alert";
import { APIUtiliyWithJWT } from "../../utils/apiClient";
import { useAuthContext } from "../../auth/AuthContext";

const CategoriesDashboard = () => {
  const [categories, setCategories] = useState([]);
  const [recordIdToDelete, setRecordIdToDelete] = useState(0);
  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const [alertConfig, setAlertConfig] = useState({
    isOpen: false,
    message: "",
    type: "info",
  });
  const { API_BASE_URL } = useAuthContext();

  useEffect(() => {
    getCategories();
  }, []);

  const getCategories = async () => {
    const result = await APIUtiliyWithJWT.get(
      `${API_BASE_URL}/api/categories/all`
    );
    if (result) {
      setCategories(result);
    }
  };

  const deleteCategory = async () => {
    if (recordIdToDelete) {
      const result = await APIUtiliyWithJWT.delete(
        `${API_BASE_URL}/api/categories/${recordIdToDelete}`
      );
      if (result) {
        setCategories(
          categories.filter((category) => category.id !== recordIdToDelete)
        );
        showAlert("Data Deleted Successfuly", "success");
      } else {
        showAlert("Something Went Wrong", "error");
      }
    }
    handleCloseDialog();
  };

  const handleOpenDialog = (recordId) => {
    setRecordIdToDelete(recordId);
    setIsDialogOpen(true);
  };

  const handleCloseDialog = () => {
    setRecordIdToDelete(0);
    setIsDialogOpen(false);
  };

  const showAlert = (message, type = "info", duration) => {
    setAlertConfig({ isOpen: true, message, type, duration });
  };

  const closeAlert = () => {
    setAlertConfig((prev) => ({ ...prev, isOpen: false }));
  };

  return (
    <section className="section section-center">
      <div className="title" style={{ marginBottom: "2rem" }}>
        <h2>Categories Dashboard</h2>
        <div className="title-underline"></div>
      </div>

      <Link to="/categories/add" className="btn">
        Add New Category
      </Link>

      <div className="table-container">
        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Name</th>
              <th>Description</th>
              <th>Edit</th>
              <th>Delete</th>
            </tr>
          </thead>
          <tbody>
            {categories &&
              categories.map((category) => (
                <tr key={category.id}>
                  <td>{category.id}</td>
                  <td>{category.name}</td>
                  <td>{category.description}</td>
                  <td>
                    <Link
                      to={`/categories/update/${category.id}`}
                      className="btn"
                    >
                      Edit
                    </Link>
                  </td>
                  <td>
                    <Button
                      onClick={() => handleOpenDialog(category.id)}
                      children={"Delete"}
                    />
                  </td>
                </tr>
              ))}
          </tbody>
        </table>
      </div>

      <Dialog
        isOpen={isDialogOpen}
        title="Confirm Your Action"
        message="Are you sure you want to delete this record?"
        onConfirm={deleteCategory}
        onCancel={handleCloseDialog}
      />
      <Alert
        isOpen={alertConfig.isOpen}
        message={alertConfig.message}
        type={alertConfig.type}
        duration={alertConfig.duration}
        onClose={closeAlert}
      />
    </section>
  );
};
export default CategoriesDashboard;
