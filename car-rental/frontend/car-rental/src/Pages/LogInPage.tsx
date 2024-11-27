import {useAuth} from "../Context/AuthContext.tsx";
import {useNavigate} from "react-router-dom";

const LogInPage = () => {
    const { isLoggedIn, setIsLoggedIn } = useAuth();
    const navigate = useNavigate()
    return (
        <div className="text-indigo-800 text-xl">
            LogInPage
            <button onClick={() => {setIsLoggedIn(true); console.log(isLoggedIn); navigate("/logged-in/cockpit")}}>
                Log In
            </button>
        </div>
    );
};

export default LogInPage;