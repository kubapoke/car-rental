import React, {useEffect, useState} from 'react';
import {jwtDecode} from 'jwt-decode'
import {GoogleOAuthProvider, GoogleLogin, googleLogout, CredentialResponse} from '@react-oauth/google'
import { useNavigate } from 'react-router-dom'
import axios from 'axios'
import ProfileIcon from "./ProfileIcon.tsx";
import {useAuth} from "../Context/AuthContext.tsx";

interface GoogleUser {
    email: string;
}

const GoogleAuthButton: React.FC<{ onLoginSuccess?: () => void; onLoginRedirect?: () => void }> = ({ onLoginSuccess, onLoginRedirect }) => {
    const { isLoggedIn, setIsLoggedIn } = useAuth();
    const [user, setUser] = useState<GoogleUser | null>(null);
    const navigate = useNavigate();
    const clientId = import.meta.env.VITE_CLIENT_ID_FOR_OATH;

    useEffect(() => {
        if(isLoggedIn)
        {
            const existingToken = sessionStorage.getItem('authToken');
            if (existingToken) {
                try {
                    const decodedUser = jwtDecode<GoogleUser>(existingToken);
                    setUser({
                        email: decodedUser.email, // Decode email from token
                    });
                } catch (error) {
                    console.error("Error decoding token:", error);
                    sessionStorage.removeItem('authToken'); // Remove invalid token
                    setIsLoggedIn(false);
                }
            }
        }
    }, [isLoggedIn, setIsLoggedIn]);

    // logic for successful GoogleLogin. response: answer from google. response.credential: google identity token
    const handleCredentialResponse = async (response: CredentialResponse) => {

        const url = `${import.meta.env.VITE_SERVER_URL}/api/Auth/google-signin`; // url of API

        // Send POST request using axios to API, which will confirm identity (verify google token), and return token,
        // which will be used to get access to [Authorize] api's. Also will return a flag, telling is current user
        // logged in for the first time
        const backEndResponse = await axios.post(url, response.credential, {
            headers: {
                'Content-Type': 'application/json', // Ensures JSON format
            },
        });

        if (backEndResponse.status == 200 && response.credential) {
            const decodedUser = jwtDecode<GoogleUser>(response.credential); // get user information from Google
            setUser({
                email: decodedUser.email, // this is part mostly for UI representation of logged in user
            });
            const { jwtToken, isNewUser } = await backEndResponse.data;
            if (isNewUser) {
                if (onLoginRedirect) {
                    onLoginRedirect();
                }

                sessionStorage.setItem('authToken', jwtToken); // store token in sessionStorage of browser
                console.log("New user logged In");
                setIsLoggedIn(false);

                navigate("/new-user-form");
            } else {
                sessionStorage.setItem('authToken', jwtToken); // store token in sessionStorage of browser
                console.log("Old user logged In");
                setIsLoggedIn(true);

                if (onLoginSuccess) {
                    onLoginSuccess();
                }
            }
        } else {
            // some error message here
        }

    };

    const logout = () => {
        googleLogout();
        setUser(null);
        sessionStorage.removeItem('tmpToken');
        sessionStorage.removeItem('authToken');
        setIsLoggedIn(false);
        navigate("/");
        console.log("Logged Out");
    }

    return (
        <GoogleOAuthProvider clientId={clientId}>
            <div>
                {user ? (
                    <ProfileIcon email={user.email} logout={logout}/>
                ) : (
                    <GoogleLogin
                        onSuccess={(credentialResponse) => handleCredentialResponse(credentialResponse)}
                        onError={() => {console.log("Login error")}}/>
                )}
            </div>
        </GoogleOAuthProvider>
    )
}

export default GoogleAuthButton;