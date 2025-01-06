import { BrowserRouter, Route, Routes } from "react-router-dom";
import Books from "./pages/Books";
import AddUpdateBook from "./pages/Books/AddUpdateBook";
import Authors from "./pages/Authors";
import AddUpdateAuthor from "./pages//Authors/AddUpdateAuthor";
import Sellers from "./pages/Sellers";
import AddUpdateSeller from "./pages/Sellers/AddUpdateSeller";
import Categories from "./pages/Categories";
import AddUpdateCategory from "./pages/Categories/AddUpdateCategory";
import PrivateRoute from "./auth/PrivateRoute";
import NotFound from "./pages/NotFound";
import Login from "./pages/Login";
import useAuth from "./auth/useAuth";

function App() {
  useAuth(); // Enable automatic token refresh
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route
          path="/"
          element={
            <PrivateRoute>
              <Books />
            </PrivateRoute>
          }
        />
        <Route
          path="/books"
          element={
            <PrivateRoute>
              <Books />
            </PrivateRoute>
          }
        />
        <Route
          path="/books/add"
          element={
            <PrivateRoute>
              <AddUpdateBook />
            </PrivateRoute>
          }
        />
        <Route
          path="/books/update/:id"
          element={
            <PrivateRoute>
              <AddUpdateBook />
            </PrivateRoute>
          }
        />
        <Route
          path="/authors"
          element={
            <PrivateRoute>
              <Authors />
            </PrivateRoute>
          }
        />
        <Route
          path="/authors/add"
          element={
            <PrivateRoute>
              <AddUpdateAuthor />
            </PrivateRoute>
          }
        />
        <Route
          path="/authors/update/:id"
          element={
            <PrivateRoute>
              <AddUpdateAuthor />
            </PrivateRoute>
          }
        />
        <Route
          path="/sellers"
          element={
            <PrivateRoute>
              <Sellers />
            </PrivateRoute>
          }
        />
        <Route
          path="/sellers/add"
          element={
            <PrivateRoute>
              <AddUpdateSeller />
            </PrivateRoute>
          }
        />
        <Route
          path="/sellers/update/:id"
          element={
            <PrivateRoute>
              <AddUpdateSeller />
            </PrivateRoute>
          }
        />
        <Route
          path="/categories"
          element={
            <PrivateRoute>
              <Categories />
            </PrivateRoute>
          }
        />
        <Route
          path="/categories/add"
          element={
            <PrivateRoute>
              <AddUpdateCategory />
            </PrivateRoute>
          }
        />
        <Route
          path="/categories/update/:id"
          element={
            <PrivateRoute>
              <AddUpdateCategory />
            </PrivateRoute>
          }
        />
        <Route path="*" element={<NotFound />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
