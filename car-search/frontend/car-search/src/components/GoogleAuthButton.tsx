import React, {useState} from 'react';
import {jwtDecode} from 'jwt-decode'
import { GoogleOAuthProvider, GoogleLogin, googleLogout } from '@react-oauth/google'
import { useNavigate } from 'react-router-dom'
import axios from 'axios'

declare const google: any;

interface GoogleUser {
    email: string;
}

const GoogleAuthButton: React.FC = () => {
    const [user, setUser] = useState<GoogleUser | null>(null);
    const navigate = useNavigate();
    const clientId = import.meta.env.VITE_CLIENT_ID_FOR_OATH;

    const handleCredentialResponse = async (response: any) => {

        const url = `${import.meta.env.VITE_SERVER_URL}/api/Auth/google-signin`;


        // Send POST request using axios
        const backEndResponse = await axios.post(url, response.credential, {
            headers: {
                'Content-Type': 'application/json', // Ensures JSON format
            },
        });

        if (backEndResponse.status == 200) {
            const decodedUser = jwtDecode<GoogleUser>(response.credential);
            setUser({
                email: decodedUser.email,
            });
            const { jwtToken, isNewUser } = await backEndResponse.data;
            if (isNewUser) {
                sessionStorage.setItem('tmpToken', jwtToken);
                console.log("New user logged In");
                navigate("/new-user-form");
            } else {
                sessionStorage.setItem('authToken', jwtToken);
                console.log("Old user logged In");
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
        navigate("/");
        console.log("Logged Out");
    }

    return (
        <GoogleOAuthProvider clientId={clientId}>
            <div>
                {user ? (
                    <div>
                        <h3>Welcome {user.email}</h3>
                        <button onClick={logout}>Logout</button>
                    </div>
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