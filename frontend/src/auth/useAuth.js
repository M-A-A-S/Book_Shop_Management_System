import { useEffect } from "react";
import authService from "./authService";

const useAuth = () => {
  useEffect(() => {
    const interval = setInterval(async () => {
      try {
        await authService.refreshToken();
      } catch (error) {
        console.error("Token refresh failed:", error);
        authService.logout();
      }
    }, 14 * 60 * 1000); // Refresh every 14 minutes

    return () => clearInterval(interval); // Cleanup on unmount
  }, []);
};

export default useAuth;
