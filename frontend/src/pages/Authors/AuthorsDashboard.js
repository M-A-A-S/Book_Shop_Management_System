import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import Button from "../../components/Button";
import Dialog from "../../components/Dialog";
import Alert from "../../components/Alert";
import { APIUtiliyWithJWT } from "../../utils/apiClient";
import { useAuthContext } from "../../auth/AuthContext";

const AuthorsDashboard = () => {
  const [authors, setAuthors] = useState([]);
  const [countries, setCountries] = useState([]);
  const [recordIdToDelete, setRecordIdToDelete] = useState(0);
  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const [alertConfig, setAlertConfig] = useState({
    isOpen: false,
    message: "",
    type: "info",
  });

  const { API_BASE_URL } = useAuthContext();

  useEffect(() => {
    getAuthors();
    getCountries();
  }, []);

  const getAuthors = async () => {
    const result = await APIUtiliyWithJWT.get(
      `${API_BASE_URL}/api/authors/all`
    );
    if (result) {
      setAuthors(result);
    }
  };

  const getCountries = async () => {
    const result = await APIUtiliyWithJWT.get(
      `${API_BASE_URL}/api/Countries/All`
    );
    if (result) {
      setCountries(result);
    }
  };

  const deleteAuthor = async () => {
    if (recordIdToDelete) {
      const result = await APIUtiliyWithJWT.delete(
        `${API_BASE_URL}/api/authors/${recordIdToDelete}`
      );
      if (result) {
        setAuthors(authors.filter((author) => author.id !== recordIdToDelete));
        showAlert("Data Deleted Successfuly", "success");
      } else {
        showAlert("Something Went Wrong", "error");
      }
    }
    handleCloseDialog();
  };

  const getCountryNameById = (countryID) => {
    let countryName = "";
    countries &&
      countries.forEach((country) => {
        if (country.countryID === countryID) {
          countryName = country.countryName;
        }
      });
    return countryName;
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
        <h2>Authors Dashboard</h2>
        <div className="title-underline"></div>
      </div>

      <Link to="/authors/add" className="btn">
        Add New Author
      </Link>

      <div className="table-container">
        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Name</th>
              <th>Gender</th>
              <th>Country</th>
              <th>Edit</th>
              <th>Delete</th>
            </tr>
          </thead>
          <tbody>
            {authors &&
              authors.map((author) => (
                <tr key={author.id}>
                  <td>{author.id}</td>
                  <td>{author.name}</td>
                  <td>{author.isMale ? "Male" : "Female"}</td>
                  <td>{getCountryNameById(author.countryId)}</td>
                  <td>
                    <Link to={`/authors/update/${author.id}`} className="btn">
                      Edit
                    </Link>
                  </td>
                  <td>
                    <Button
                      onClick={() => handleOpenDialog(author.id)}
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
        onConfirm={deleteAuthor}
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
export default AuthorsDashboard;
