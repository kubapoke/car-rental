import React, {useEffect, useState} from 'react';
import {jwtDecode} from 'jwt-decode'

declare const google: any;

interface GoogleUser {
    email: string;
}

const GoogleAuthButton: React.FC = () => {
    const [user, setUser] = useState<GoogleUser | null>(null);
    const clientId = import.meta.env.VITE_CLIENT_ID_FOR_OATH;

    useEffect(() => {
        const loadGoogleAuth = () => {
            google.accounts.id.initialize({
                client_id: clientId,
                callback: handleCredentialResponse,
            });

            google.accounts.id.renderButton(
                document.getElementById('googleSignInDiv'),
                {
                    theme: 'outline',
                    size: 'large',
                }
            );
        };

        if (window.google) {
            loadGoogleAuth();
        } else {
            const script = document.createElement('script');
            script.src = 'https://accounts.google.com/gsi/client';
            script.async = true;
            script.onload = loadGoogleAuth;
            document.body.appendChild(script);
        }
    }, []);

    const handleCredentialResponse = (response: any) => {
        const decodedUser = jwtDecode<GoogleUser>(response.credential);
        setUser({
            email: decodedUser.email,
        });
        console.log(user?.email);
    };

    const logout = () => {
        google.accounts.id.disableAutoSelect();
        setUser(null);
        console.log('Logedout');
    }

    return (
        <div>
            {user ? (
                <div>
                    <button onClick={logout}>Logout</button>
                </div>
            ) : (
                <div id='googleSignInDiv'></div>
            )}
        </div>
    )
}

export default GoogleAuthButton;