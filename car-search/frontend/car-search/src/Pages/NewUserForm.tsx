import React, {useState} from "react";
import {useNavigate} from "react-router-dom";


const NewUserForm: React.FC = () => {
    const [formData, setFormData] = useState({name: "", surname: "", birthDate: "", licenseDate: ""});
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setFormData({...formData, [e.target.name]: e.target.value});
    }

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            const token = sessionStorage.getItem('authToken'); // get our token to access [Authorize] api
            const response = await fetch(`${import.meta.env.VITE_SERVER_URL}/api/Auth/complete-registration`, {
                method: 'POST',
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `Bearer ${token}`, // this is required when we use [Authorize] api's
                },
                body: JSON.stringify(formData)
            });
            if (!response.ok) {
                throw new Error('Failed to complete registration');
            }
            const data = await response.json();
            console.log(data.sessionToken);
            sessionStorage.removeItem('authToken'); // remove our temporary token
            sessionStorage.setItem('authToken', data.sessionToken); // add main token
            navigate('/');

        } catch {
            setError("Failed to complete registration, please try again.");
        }
    }

    return (
        <div className="max-w-md mx-auto p-6 bg-white rounded-lg shadow-lg mt-32">
            <h1 className="text-xl font-semibold text-center mb-4">Complete your registration</h1>
            {error && <p style={{color: 'red'}}>{error}</p>}
            <form onSubmit={handleSubmit} className="space-y-4">
                <div className="form-group">
                    <label className="block text-sm font-medium text-gray-700">Name:</label>
                    <input
                        type="text"
                        name="name"
                        value={formData.name}
                        onChange={handleChange}
                        required
                        className="mt-1 p-2 w-full border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
                    />
                </div>

                <div className="form-group">
                    <label className="block text-sm font-medium text-gray-700">Surname:</label>
                    <input
                        type="text"
                        name="surname"
                        value={formData.surname}
                        onChange={handleChange}
                        required
                        className="mt-1 p-2 w-full border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
                    />
                </div>
                <div className="form-group">
                    <label htmlFor="birthDate" className="block text-sm font-medium text-gray-700">Birth Date:</label>
                    <input
                        id="birthDate"
                        type="date"
                        name="birthDate"
                        placeholder="dd-mm-yyyy"
                        value={formData.birthDate}
                        onChange={handleChange}
                        required
                        className="mt-1 p-2 w-full border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
                    />
                </div>
                <div className="form-group">
                    <label htmlFor="licenseDate" className="block text-sm font-medium text-gray-700">License
                        Date:</label>
                    <input
                        id="licenseDate"
                        type="date"
                        name="licenseDate"
                        placeholder="dd-mm-yyyy"
                        value={formData.licenseDate}
                        onChange={handleChange}
                        required
                        className="mt-1 p-2 w-full border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
                    />
                </div>
                <div className="form-group">
                    <button type="submit"
                            className="w-full py-2 bg-indigo-600 text-white rounded-md shadow-md hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-500">
                        Submit
                    </button>
                </div>
            </form>
        </div>
    )
}

export default NewUserForm;