import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import Button from "../../components/Button";
import Dialog from "../../components/Dialog";
import Alert from "../../components/Alert";
import { APIUtiliyWithJWT } from "../../utils/apiClient";
import { useAuthContext } from "../../auth/AuthContext";

const SellersDashboard = () => {
  const [sellers, setSellers] = useState([]);
  const [recordIdToDelete, setRecordIdToDelete] = useState(0);
  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const [alertConfig, setAlertConfig] = useState({
    isOpen: false,
    message: "",
    type: "info",
  });

  const { API_BASE_URL } = useAuthContext();

  useEffect(() => {
    getSellers();
  }, []);

  const getSellers = async () => {
    const result = await APIUtiliyWithJWT.get(
      `${API_BASE_URL}/api/sellers/all`
    );
    if (result) {
      setSellers(result);
    }
  };

  const deleteSeller = async () => {
    if (recordIdToDelete) {
      const result = await APIUtiliyWithJWT.delete(
        `${API_BASE_URL}/api/sellers/${recordIdToDelete}`
      );
      if (result) {
        setSellers(sellers.filter((seller) => seller.id !== recordIdToDelete));
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
        <h2>Sellers Dashboard</h2>
        <div className="title-underline"></div>
      </div>

      <Link to="/sellers/add" className="btn">
        Add New Seller
      </Link>

      <div className="table-container">
        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Name</th>
              <th>Email</th>
              <th>Phone</th>
              <th>Address</th>
              <th>Edit</th>
              <th>Delete</th>
            </tr>
          </thead>
          <tbody>
            {sellers &&
              sellers.map((seller) => (
                <tr key={seller.id}>
                  <td>{seller.id}</td>
                  <td>{seller.name}</td>
                  <td>{seller.email}</td>
                  <td>{seller.phone}</td>
                  <td>{seller.address}</td>
                  <td>
                    <Link to={`/sellers/update/${seller.id}`} className="btn">
                      Edit
                    </Link>
                  </td>
                  <td>
                    <Button
                      onClick={() => handleOpenDialog(seller.id)}
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
        onConfirm={deleteSeller}
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
export default SellersDashboard;
