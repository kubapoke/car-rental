import {Link} from 'react-router-dom';
import {FaCar} from "react-icons/fa";
import { useState, useEffect } from 'react';
import {jwtDecode} from "jwt-decode";
const RentConfirmationPage = () => {
    const [email, setEmail] = useState<string | null>(null);

    useEffect(() => {
        const authToken = sessionStorage.getItem('authToken');

        if (authToken) {
            try {
                const {email} = jwtDecode<{email : string}>(authToken);
                setEmail(email);
            } catch (error) {
                console.error("Error decoding token:", error);
            }
        }
    }, []);
    return (
        <section className="text-center flex flex-col justify-center items-center h-96">
            <FaCar className={"fas text-yellow-400 text-6xl mb-4"}/>
            <h1 className="text-6xl font-bold mb-4">Check your email</h1>
            <p className="text-xl mb-5">We've sent a verification link to {email}.</p>
            <p className="text-xl mb-5">Click on the link to complete the verification process. You might need to check you spam folder.</p>
            <Link
                to="/"
                className="text-white bg-blue-700 hover:bg-blue-900 rounded-md px-3 py-2 mt-4"
            >Go Back</Link
            >
        </section>
    );
};

export default RentConfirmationPage;