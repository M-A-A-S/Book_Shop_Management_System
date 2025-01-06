const API_BASE_URL = "https://localhost:7150/api/auth"; // ASP.NET Core API URL
let accessToken = null; // Store access token in memory

const authService = {
  // Login and fetch tokens
  async login(credentials) {
    try {
      const response = await fetch(`${API_BASE_URL}/login`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(credentials),
        credentials: "include", // Include cookies in requests
      });

      if (!response.ok) {
        return false;
      }

      const data = await response.json();
      accessToken = data.accessToken; // Store access token in memory
      return true;
    } catch (error) {
      console.error(error.message);
    }
  },

  // Refresh the access token
  async refreshToken() {
    try {
      const response = await fetch(`${API_BASE_URL}/refresh`, {
        method: "POST",
        credentials: "include", // Include HttpOnly refresh token in cookies
      });

      if (!response.ok) {
        return false;
      }

      const data = await response.json();
      accessToken = data.accessToken; // Store access token in memory
      return true;
    } catch (error) {
      console.error(error);
    }
  },

  // Logout and clear tokens
  async logout() {
    try {
      await fetch(`${API_BASE_URL}/logout`, {
        method: "POST",
        credentials: "include", // Send cookies
      });
      accessToken = null; // Clear in-memory token
    } catch (error) {
      console.error(error);
    }
  },

  // Get the current access token
  getAccessToken() {
    return accessToken;
  },

  // Fetch API with automatic token handling
  async fetchWithAuth(url, options = {}) {
    if (!accessToken) {
      throw new Error("Access token is missing");
    }

    const response = await fetch(`${url}`, {
      ...options,
      headers: {
        ...options.headers,
        Authorization: `Bearer ${accessToken}`,
      },
    });

    // If the access token has expired, try to refresh it
    if (response.status === 401) {
      try {
        await this.refreshToken();
        return this.fetchWithAuth(url, options); // Retry request
      } catch (error) {
        console.error("Failed to refresh token, logging out...");
        this.logout();
        throw error;
      }
    }
    return response;
  },
};

export default authService;
