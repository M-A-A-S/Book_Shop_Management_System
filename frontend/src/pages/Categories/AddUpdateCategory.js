import { useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import Input from "../../components/Input";
import Alert from "../../components/Alert";
import Button from "../../components/Button";
import { APIUtiliyWithJWT } from "../../utils/apiClient";
import { useAuthContext } from "../../auth/AuthContext";

const AddUpdateCategory = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  const [category, setCategory] = useState({
    name: "",
    description: "",
  });
  const [errors, setErrors] = useState({});
  const [alertConfig, setAlertConfig] = useState({
    isOpen: false,
    message: "",
    type: "info",
  });
  const { API_BASE_URL } = useAuthContext();

  useEffect(() => {
    getCategory();
  }, [id]);

  const getCategory = async () => {
    if (id && !isNaN(id)) {
      let result = await APIUtiliyWithJWT.get(
        `${API_BASE_URL}/api/categories/${id}`
      );
      if (result) {
        setCategory(result);
      }
    }
  };
  const addCategory = async () => {
    let result = await APIUtiliyWithJWT.post(
      `${API_BASE_URL}/api/categories`,
      category
    );
    if (result) {
      showAlert("Data Added Successfuly", "success");
    } else {
      showAlert("Something Went Wrong", "error");
    }
  };

  const updateCategory = async () => {
    let result = await APIUtiliyWithJWT.put(
      `${API_BASE_URL}/api/categories/${id}`,
      {
        id: id,
        ...category,
      }
    );
    if (result) {
      showAlert("Data Updated Successfuly", "success");
    } else {
      showAlert("Something Went Wrong", "error");
    }
  };

  const onChange = (e) => {
    let { name, value } = e.target;
    setCategory({ ...category, [name]: value });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    const validationErrors = {};
    if (!category.name.trim()) {
      validationErrors.name = "Name is required";
    }

    if (!category.description.trim()) {
      validationErrors.description = "Description is required";
    }

    setErrors(validationErrors);
    if (Object.keys(validationErrors).length === 0) {
      if (id && !isNaN(id)) {
        updateCategory();
      } else {
        addCategory();
      }
    }
  };

  const showAlert = (message, type = "info", duration) => {
    setAlertConfig({ isOpen: true, message, type, duration });
  };
  const closeAlert = () => {
    setAlertConfig((prev) => ({ ...prev, isOpen: false }));
    navigate("/categories");
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
          {id && !isNaN(id) ? "Update" : "Add"} Category
        </h4>

        <Input
          label="Name"
          name="name"
          value={category.name}
          onChange={onChange}
          errorMessage={errors.name}
          required
        />
        <Input
          label="Description"
          name="description"
          value={category.description}
          onChange={onChange}
          errorMessage={errors.description}
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
          to="/categories"
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
export default AddUpdateCategory;
