import { useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import Select from "../../components/Select";
import Input from "../../components/Input";
import Alert from "../../components/Alert";
import Button from "../../components/Button";
import { APIUtiliyWithJWT } from "../../utils/apiClient";
import { useAuthContext } from "../../auth/AuthContext";

const AddUpdateBook = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  const [book, setBook] = useState({
    title: "",
    authorId: 0,
    categoryId: 0,
    price: 0,
    quantity: 0,
  });
  const [errors, setErrors] = useState({});
  const [alertConfig, setAlertConfig] = useState({
    isOpen: false,
    message: "",
    type: "info",
  });
  const [authors, setAuthors] = useState([]);
  const [categories, setCategories] = useState([]);

  const { API_BASE_URL } = useAuthContext();

  useEffect(() => {
    getAuthors();
    getCategories();
    getBook();
  }, [id]);

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
  const getBook = async () => {
    if (id && !isNaN(id)) {
      let result = await APIUtiliyWithJWT.get(
        `${API_BASE_URL}/api/books/${id}`
      );
      if (result) {
        setBook(result);
      }
    }
  };
  const addBook = async () => {
    let result = await APIUtiliyWithJWT.post(`${API_BASE_URL}/api/books`, book);
    if (result) {
      showAlert("Data Added Successfuly", "success");
    } else {
      showAlert("Something Went Wrong", "error");
    }
  };

  const updateBook = async () => {
    let result = await APIUtiliyWithJWT.put(`${API_BASE_URL}/api/books/${id}`, {
      id: id,
      ...book,
    });
    if (result) {
      showAlert("Data Updated Successfuly", "success");
    } else {
      showAlert("Something Went Wrong", "error");
    }
  };

  const onChange = (e) => {
    let { name, value } = e.target;
    setBook({ ...book, [name]: value });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    const validationErrors = {};
    if (!book.title.trim()) {
      validationErrors.title = "Title is required";
    }

    if (!book.price.toString().trim()) {
      validationErrors.price = "Price is required";
    } else if (isNaN(book.price.toString().trim())) {
      validationErrors.price = "Price is not valid";
    }

    if (!book.quantity.toString().trim()) {
      validationErrors.quantity = "Quantity is required";
    } else if (isNaN(book.quantity.toString().trim())) {
      validationErrors.quantity = "Quantity is not valid";
    }

    setErrors(validationErrors);
    if (Object.keys(validationErrors).length === 0) {
      if (id && !isNaN(id)) {
        updateBook();
      } else {
        addBook();
      }
    }
  };

  const showAlert = (message, type = "info", duration) => {
    setAlertConfig({ isOpen: true, message, type, duration });
  };
  const closeAlert = () => {
    setAlertConfig((prev) => ({ ...prev, isOpen: false }));
    navigate("/books");
  };

  return (
    <section className="section section-center">
      <form className="form" onSubmit={handleSubmit}>
        <h4
          style={{
            marginBottom: "1rem",
            textAlign: "center",
            color: "var(--primary-500)",
            fontWeight: "bold",
          }}
        >
          {id && !isNaN(id) ? "Update" : "Add"} Book
        </h4>

        <Input
          label="Title"
          name="title"
          value={book.title}
          onChange={onChange}
          errorMessage={errors.title}
          required
        />
        <Select
          label="Author"
          name="authorId"
          value={book.authorId}
          onChange={onChange}
          errorMessage={errors.authorId}
          options={
            authors &&
            authors.map((author) => {
              return { value: author.id, label: author.name };
            })
          }
        />
        <Select
          label="Category"
          name="categoryId"
          value={book.categoryId}
          onChange={onChange}
          errorMessage={errors.categoryId}
          options={
            categories &&
            categories.map((category) => {
              return { value: category.id, label: category.name };
            })
          }
        />
        <Input
          label="Price"
          name="price"
          value={book.price}
          onChange={onChange}
          errorMessage={errors.price}
          required
        />
        <Input
          label="Quantity"
          name="quantity"
          value={book.quantity}
          onChange={onChange}
          errorMessage={errors.quantity}
          required
        />

        <Button
          style={{ marginBottom: "0.5rem" }}
          onClick={handleSubmit}
          className="btn-block"
          children={id && !isNaN(id) ? "Update" : "Add"}
        />
        <Link
          style={{ textAlign: "center" }}
          className="btn btn-block"
          to="/books"
        >
          Cancel
        </Link>
        <Alert
          isOpen={alertConfig.isOpen}
          message={alertConfig.message}
          type={alertConfig.type}
          duration={alertConfig.duration}
          onClose={closeAlert}
        />
      </form>
    </section>
  );
};
export default AddUpdateBook;
