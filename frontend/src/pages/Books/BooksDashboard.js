import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import Button from "../../components/Button";
import Dialog from "../../components/Dialog";
import Alert from "../../components/Alert";
import { APIUtiliyWithJWT } from "../../utils/apiClient";
import { useAuthContext } from "../../auth/AuthContext";

const BooksDashboard = () => {
  const [books, setBooks] = useState([]);
  const [authors, setAuthors] = useState([]);
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
    getAuthors();
    getCategories();
    getBooks();
  }, []);

  const getAuthors = async () => {
    const result = await APIUtiliyWithJWT.get(
      `${API_BASE_URL}/api/authors/all`
    );
    if (result) {
      setAuthors(result);
    }
  };

  const getCategories = async () => {
    const result = await APIUtiliyWithJWT.get(
      `${API_BASE_URL}/api/categories/all`
    );
    if (result) {
      setCategories(result);
    }
  };

  const getBooks = async () => {
    const result = await APIUtiliyWithJWT.get(`${API_BASE_URL}/api/books/all`);
    if (result) {
      setBooks(result);
    }
  };

  const deleteBook = async () => {
    if (recordIdToDelete) {
      const result = await APIUtiliyWithJWT.delete(
        `${API_BASE_URL}/api/books/${recordIdToDelete}`
      );
      if (result) {
        setBooks(books.filter((book) => book.id !== recordIdToDelete));
        showAlert("Data Deleted Successfuly", "success");
      } else {
        showAlert("Something Went Wrong", "error");
      }
    }
    handleCloseDialog();
  };

  const getAuthorNameById = (id) => {
    let authorName = "";
    authors &&
      authors.forEach((author) => {
        if (author.id === id) {
          authorName = author.name;
        }
      });
    return authorName;
  };

  const getCategoryNameById = (id) => {
    let categoryName = "";
    categories &&
      categories.forEach((category) => {
        if (category.id === id) {
          categoryName = category.name;
        }
      });
    return categoryName;
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
        <h2>Books Dashboard</h2>
        <div className="title-underline"></div>
      </div>

      <Link to="/books/add" className="btn">
        Add New Book
      </Link>

      <div className="table-container">
        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Title</th>
              <th>Author</th>
              <th>Category</th>
              <th>Quantity</th>
              <th>Price</th>
              <th>Edit</th>
              <th>Delete</th>
            </tr>
          </thead>
          <tbody>
            {books &&
              books.map((book) => (
                <tr key={book.id}>
                  <td>{book.id}</td>
                  <td>{book.title}</td>
                  <td>{getAuthorNameById(book.authorId)}</td>
                  <td>{getCategoryNameById(book.categoryId)}</td>
                  <td>{book.price}</td>
                  <td>{book.quantity}</td>
                  <td>
                    <Link to={`/books/update/${book.id}`} className="btn">
                      Edit
                    </Link>
                  </td>
                  <td>
                    <Button
                      onClick={() => handleOpenDialog(book.id)}
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
        onConfirm={deleteBook}
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
export default BooksDashboard;
