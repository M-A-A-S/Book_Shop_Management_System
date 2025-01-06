import { useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import Input from "../../components/Input";
import Alert from "../../components/Alert";
import Textarea from "../../components/Textarea";
import Button from "../../components/Button";
import { APIUtiliyWithJWT } from "../../utils/apiClient";
import { useAuthContext } from "../../auth/AuthContext";

const AddUpdateSeller = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  const [seller, setSeller] = useState({
    name: "",
    email: "",
    phone: "",
    address: "",
  });
  const [errors, setErrors] = useState({});
  const [alertConfig, setAlertConfig] = useState({
    isOpen: false,
    message: "",
    type: "info",
  });

  const { API_BASE_URL } = useAuthContext();

  useEffect(() => {
    getSeller();
  }, [id]);

  const getSeller = async () => {
    if (id && !isNaN(id)) {
      let result = await APIUtiliyWithJWT.get(
        `${API_BASE_URL}/api/sellers/${id}`
      );
      if (result) {
        setSeller(result);
      }
    }
  };
  const addSeller = async () => {
    let result = await APIUtiliyWithJWT.post(
      `${API_BASE_URL}/api/sellers`,
      seller
    );
    if (result) {
      showAlert("Data Added Successfuly", "success");
    } else {
      showAlert("Something Went Wrong", "error");
    }
  };

  const updateSeller = async () => {
    let result = await APIUtiliyWithJWT.put(
      `${API_BASE_URL}/api/sellers/${id}`,
      {
        id: id,
        ...seller,
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
    setSeller({ ...seller, [name]: value });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    const validationErrors = {};
    if (!seller.name.trim()) {
      validationErrors.name = "Name is required";
    }
    if (!seller.email.trim()) {
      validationErrors.email = "Email is required";
    }
    if (!seller.phone.trim()) {
      validationErrors.phone = "Phone is required";
    } else if (isNaN(seller.phone.trim())) {
      validationErrors.phone = "Phone is not valid";
    } else if (seller.phone.trim().length !== 10) {
      validationErrors.phone = "Phone should be 10 digits";
    }
    if (!seller.address.trim()) {
      validationErrors.address = "Address is required";
    }

    setErrors(validationErrors);
    if (Object.keys(validationErrors).length === 0) {
      if (id && !isNaN(id)) {
        updateSeller();
      } else {
        addSeller();
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
          {id && !isNaN(id) ? "Update" : "Add"} Seller
        </h4>

        <Input
          label="Name"
          name="name"
          value={seller.name}
          onChange={onChange}
          errorMessage={errors.name}
          required
        />
        <Input
          label="Email"
          name="email"
          value={seller.email}
          onChange={onChange}
          errorMessage={errors.email}
          required
        />
        <Input
          label="Phone"
          name="phone"
          value={seller.phone}
          onChange={onChange}
          errorMessage={errors.phone}
          required
        />
        <Textarea
          label="Address"
          name="address"
          value={seller.address}
          onChange={onChange}
          errorMessage={errors.address}
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
          to="/sellers"
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
export default AddUpdateSeller;
