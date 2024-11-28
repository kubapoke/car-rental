import React, {useEffect, useState} from 'react';
import {jwtDecode} from 'jwt-decode'
import {useAuth} from "../Context/AuthContext.tsx"
import {useNavigate} from "react-router-dom";

interface Manager {
    UserName: string;
}

const LogInButton: React.FC<{ onLoginSuccess?: () => void; onLoginRedirect?: () => void}> = () => {
    const [user, setUser] = useState<Manager | null>(null);
    const {isLoggedIn, setIsLoggedIn} = useAuth();
    const navigate = useNavigate();

    useEffect(() => {
        if (isLoggedIn) {
            const token = sessionStorage.getItem("authToken");
            if (token) {
                try {
                    const decodedUser = jwtDecode<Manager>(token);
                    setUser({
                        UserName: decodedUser.UserName, // Decode email from token
                    });
                } catch (error) {
                    console.error("Error decoding token:", error);
                    sessionStorage.removeItem('authToken'); // Remove invalid token
                    setIsLoggedIn(false);
                }
            }
        }
    }, [isLoggedIn, setIsLoggedIn]);

    const handleLogIn = async () => {
        navigate("/log-in-page");
    }
    const handleLogOut = async () => {
        setUser(null);
        sessionStorage.removeItem('authToken');
        setIsLoggedIn(false);
        navigate("/");
        console.log("Logged Out");

    }


    return (
        <div>
            {user ? (
                <div>
                    <div>Hello {user.UserName}</div>
                    <button onClick={ () => {handleLogOut()}}>Log Out</button>
                </div>
            ) : (
                <button onClick={() => {handleLogIn()} } type="button">Log In</button>
            )}
        </div>
    )
}

export default LogInButton;