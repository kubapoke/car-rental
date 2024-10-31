import React, {useState} from 'react';
import {jwtDecode} from 'jwt-decode'
import { GoogleOAuthProvider, GoogleLogin, googleLogout } from '@react-oauth/google'

declare const google: any;

interface GoogleUser {
    email: string;
}

const GoogleAuthButton: React.FC = () => {
    const [user, setUser] = useState<GoogleUser | null>(null);
    const clientId = import.meta.env.VITE_CLIENT_ID_FOR_OATH;

    const handleCredentialResponse = (response: any) => {
        const decodedUser = jwtDecode<GoogleUser>(response.credential);
        setUser({
            email: decodedUser.email,
        });
        console.log("Logged In");
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
                        <h2>Welcome</h2>
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