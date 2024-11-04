import React, {useState} from 'react';
import {jwtDecode} from 'jwt-decode'
import { GoogleOAuthProvider, GoogleLogin, googleLogout } from '@react-oauth/google'
import { useNavigate } from 'react-router-dom'

declare const google: any;

interface GoogleUser {
    email: string;
}

const GoogleAuthButton: React.FC = () => {
    const [user, setUser] = useState<GoogleUser | null>(null);
    const navigate = useNavigate();
    const clientId = import.meta.env.VITE_CLIENT_ID_FOR_OATH;

    const handleCredentialResponse = async (response: any) => {
        const backEndResponse = await fetch(`${import.meta.env.VITE_SERVER_URL}/api/AuthController/google-signin`, {
            method: 'POST',
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(response)
        });
        if (backEndResponse.ok) {
            const decodedUser = jwtDecode<GoogleUser>(response.credential);
            setUser({
                email: decodedUser.email,
            });
            const { jwtToken, isNewUser } = await backEndResponse.json();
            if (isNewUser) {
                sessionStorage.setItem('tmpToken', jwtToken);
                console.log("New user logged In");
                navigate("/complete-profile");
            } else {
                sessionStorage.setItem('authToken', jwtToken);
                console.log("Old user logged In");
            }

        } else {

        }

    };

    const logout = () => {
        googleLogout();
        setUser(null);
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