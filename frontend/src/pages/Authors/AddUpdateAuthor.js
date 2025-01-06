import { useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import Select from "../../components/Select";
import Input from "../../components/Input";
import Alert from "../../components/Alert";
import Button from "../../components/Button";
import { APIUtiliyWithJWT } from "../../utils/apiClient";
import { useAuthContext } from "../../auth/AuthContext";

const AddUpdateAuthor = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  const [author, setAuthor] = useState({
    name: "",
    isMale: true,
    countryId: 0,
  });
  const [errors, setErrors] = useState({});
  const [alertConfig, setAlertConfig] = useState({
    isOpen: false,
    message: "",
    type: "info",
  });
  const [countries, setCountries] = useState([]);

  const { API_BASE_URL } = useAuthContext();

  useEffect(() => {
    getCountries();
    getAuthor();
  }, [id]);

  const getCountries = async () => {
    const result = await APIUtiliyWithJWT.get(
      `${API_BASE_URL}/api/Countries/All`
    );
    if (result) {
      setCountries(result);
    }
  };

  const getAuthor = async () => {
    if (id && !isNaN(id)) {
      let result = await APIUtiliyWithJWT.get(
        `${API_BASE_URL}/api/authors/${id}`
      );
      if (result) {
        setAuthor(result);
      }
    }
  };
  const addAuthor = async () => {
    let result = await APIUtiliyWithJWT.post(
      `${API_BASE_URL}/api/authors`,
      author
    );
    if (result) {
      showAlert("Data Added Successfuly", "success");
    } else {
      showAlert("Something Went Wrong", "error");
    }
  };

  const updateAuthor = async () => {
    let result = await APIUtiliyWithJWT.put(
      `${API_BASE_URL}/api/authors/${id}`,
      {
        id: id,
        ...author,
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
    if (name === "isMale") {
      if (value === "true") {
        value = true;
      } else {
        value = false;
      }
    }
    setAuthor({ ...author, [name]: value });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    const validationErrors = {};
    if (!author.name.trim()) {
      validationErrors.name = "Name is required";
    }

    setErrors(validationErrors);
    if (Object.keys(validationErrors).length === 0) {
      if (id && !isNaN(id)) {
        updateAuthor();
      } else {
        addAuthor();
      }
    }
  };

  const showAlert = (message, type = "info", duration) => {
    setAlertConfig({ isOpen: true, message, type, duration });
  };
  const closeAlert = () => {
    setAlertConfig((prev) => ({ ...prev, isOpen: false }));
    navigate("/authors");
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
          {id && !isNaN(id) ? "Update" : "Add"} Author
        </h4>

        <Input
          label="Name"
          name="name"
          value={author.name}
          onChange={onChange}
          errorMessage={errors.name}
          required
        />
        <Select
          label="Gender"
          name="isMale"
          value={author.isMale}
          onChange={onChange}
          errorMessage={errors.isMale}
          options={[
            { value: true, label: "Male" },
            { value: false, label: "Female" },
          ]}
        />
        <Select
          label="Country"
          name="countryId"
          value={author.countryId}
          onChange={onChange}
          errorMessage={errors.countryId}
          options={
            countries &&
            countries.map((country) => {
              return { value: country.countryID, label: country.countryName };
            })
          }
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
          to="/authors"
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
export default AddUpdateAuthor;
