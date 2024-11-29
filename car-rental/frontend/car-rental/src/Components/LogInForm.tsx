import React, {useState} from "react";
import {useNavigate} from "react-router-dom";
import {useAuth} from "../Context/AuthContext.tsx";

const LogInForm = () => {
    const {setIsLoggedIn} = useAuth();
    const [formData, setFormData] = useState({UserName: "", Password: ""});
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();
    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setFormData({...formData, [e.target.name]: e.target.value});
    }

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            const response = await fetch(`${import.meta.env.VITE_SERVER_URL}/api/Auth/log-in`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({UserName: formData.UserName, Password: formData.Password}),
            });
            if (!response.ok) {
                throw new Error("Could not log in");
            }
            const data = await response.json();
            console.log("Logged In");
            sessionStorage.setItem("authToken", data.jwtToken);
            const token = sessionStorage.getItem("authToken");
            console.log(`token: ${token}`);
            setIsLoggedIn(true);
            navigate("/logged-in/cockpit");
        } catch {
            setError("Failed to complete registration, please try again.");
        }
    }
    
    return (
        <div className="max-w-md mx-auto">
            {error && <p style={{color: 'red'}}>{error}</p>}
            <form onSubmit={handleSubmit} className="space-y-4">
                <div className="form-group">
                    <label className="block text-sm font-medium text-white">Username:</label>
                    <input
                        type="text"
                        name="UserName"
                        value={formData.UserName}
                        onChange={handleChange}
                        required
                        className="mt-1 p-2 w-full border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
                    />
                </div>
        
                <div className="form-group">
                    <label className="block text-sm font-medium text-white">Password:</label>
                    <input
                        type="password"
                        name="Password"
                        value={formData.Password}
                        onChange={handleChange}
                        required
                        autoComplete="off"
                        className="mt-1 p-2 w-full border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
                    />
                </div>
                <div className="form-group flex justify-center">
                    <button type="submit"
                            className="w-1/2 py-2 bg-indigo-500 text-black rounded-md shadow-md hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-500">
                        Submit
                    </button>
                </div>
            </form>
        </div>
    );
};

export default LogInForm;