import {useNavigate} from "react-router-dom";
import {useAuth} from "../Context/AuthContext.tsx";

const LogOutButton = () => {
    const { setIsLoggedIn } = useAuth();
    const navigate = useNavigate();

    const handleLogout = () => {
        sessionStorage.removeItem("authToken");
        setIsLoggedIn(false);
        navigate("/log-in");
    };
    
    return (
        <button
            onClick={handleLogout}
            className="px-4 py-2 bg-red-500 text-white rounded-md shadow-md hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-red-500"
        >
            Log Out
        </button>
    );
};

export default LogOutButton;