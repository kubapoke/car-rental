import {useAuth} from "../Context/AuthContext.tsx";
import {useNavigate} from "react-router-dom";
import {FaLock} from 'react-icons/fa';

const LockComponent = () => {
    return (
        <div className="flex items-center justify-center">
            <span className="text-3xl text-white fa-solid"> <FaLock /> </span>
        </div>
    )
}


const LogInPage = () => {
    const {isLoggedIn, setIsLoggedIn} = useAuth();
    const navigate = useNavigate()
    return (<div className="grid grid-cols-3 grid-rows-3 min-h-screen bg-black">
            <LockComponent/>
            <LockComponent/>
            <LockComponent/>
            <LockComponent/>
            
            <div className="flex items-center justify-center">
                <button
                    onClick={() => {
                        setIsLoggedIn(true);
                        console.log(isLoggedIn);
                        navigate("/logged-in/cockpit");
                    }}
                    className="bg-white text-black py-2 px-4 rounded"
                >
                    Log In
                </button>
            </div>
            
            <LockComponent/>
            <LockComponent/>
            <LockComponent/>
            <LockComponent/>
        </div>

    );
};


export default LogInPage;