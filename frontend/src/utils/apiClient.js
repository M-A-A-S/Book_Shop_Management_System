import authService from "../auth/authService";

export class APIUtiliyWithJWT {
  // Get
  static async get(url) {
    try {
      let response = await authService.fetchWithAuth(url, {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
        },
      });
      const result = await response.json();
      if (response.ok) {
        return result;
      } else {
        console.error(`Error: ${response.status} - ${response.statusText}`);
        return null;
      }
    } catch (error) {
      console.error(`Fetch failed: ${error.message}`);
      return null;
    }
  }
  // Put
  static async put(url, object) {
    try {
      let response = await authService.fetchWithAuth(url, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(object),
      });
      if (response.ok) {
        return true;
      } else {
        console.error(`Error: ${response.status} - ${response.statusText}`);
        return false;
      }
    } catch (error) {
      console.error(`Fetch failed: ${error.message}`);
      return false;
    }
  }
  // Post
  static async post(url, object) {
    try {
      let response = await authService.fetchWithAuth(url, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(object),
      });
      if (response.ok) {
        return true;
      } else {
        console.error(`Error: ${response.status} - ${response.statusText}`);
        return false;
      }
    } catch (error) {
      console.error(`Fetch failed: ${error.message}`);
      return false;
    }
  }
  // Delete
  static async delete(url) {
    try {
      let response = await authService.fetchWithAuth(url, {
        method: "DELETE",
        headers: {
          "Content-Type": "application/json",
        },
      });

      if (response.ok) {
        return true;
      } else {
        console.error(`Error: ${response.status} - ${response.statusText}`);
        return false;
      }
    } catch (error) {
      console.error(`Fetch failed: ${error.message}`);
      return false;
    }
  }
}
