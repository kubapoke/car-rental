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
            const response = await fetch(`${import.meta.env.VITE_SERVER_URL}/api/Auth/complete-registration`, {
                method: 'POST',
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(formData)
            });
            if (!response.ok) {
                throw new Error('Failed to complete registration');
            }
            const data = await response.json();
            sessionStorage.removeItem('tmpToken');
            sessionStorage.setItem('authToken', data.jwtToken);
            navigate('/');

        } catch (error) {
            setError("Failed to complete registration, please try again.");
        }
    }

    return (
        <div>
            <h1>Complete your registration</h1>
            {error && <p style={{color: 'red'}}>{error}</p>}
            <form onSubmit={handleSubmit}>
                <label>
                    Name:
                    <input type="text" name="name" value={formData.name} onChange={handleChange} required/>
                </label>
                <label>
                    Surname:
                    <input type="text" name="surname" value={formData.surname} onChange={handleChange} required/>
                </label>
                <label>
                    Birth Date:
                    <input type="date" name="birthDate" value={formData.birthDate} onChange={handleChange} required/>
                </label>
                <label>
                    License Date:
                    <input type="date" name="licenseDate" value={formData.licenseDate} onChange={handleChange} required/>
                </label>
                <button type="submit">Submit</button>
            </form>
        </div>
    )
}

export default NewUserForm;