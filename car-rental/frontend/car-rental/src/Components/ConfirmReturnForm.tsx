import {Rent} from "../Context/RentsContext.tsx";
import React, {useState} from "react";
import {useNavigate} from "react-router-dom";

interface FormData {
    Id: string;
    ActualStartDate: string;
    ActualEndDate: string;
    Description: string;
    Image: File | null; // Store the uploaded file
    Preview: string | null; // Store the preview URL
}

const ConfirmReturnForm: React.FC<{ rent: Rent }> = ({rent}) => {
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();
    const [formData, setFormData] = useState<FormData>({
        Id: rent.id,
        ActualStartDate: rent.rentStart,
        ActualEndDate: rent.rentEnd,
        Description: "",
        Image: null,
        Preview: null,
    });

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setFormData({...formData, [e.target.name]: e.target.value});
    }

    const handleImageChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const file = e.target.files?.[0];
        if (file) {
            setFormData({...formData, Image: file, Preview: URL.createObjectURL(file) });
        }

    }

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            const token = sessionStorage.getItem('authToken');
            const response = await fetch(`${import.meta.env.VITE_SERVER_URL}/api/Rents/close-rent`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    ...(token && { Authorization: `Bearer ${token}` }),
                },
                body: JSON.stringify(formData),
            });
            if (!response.ok) {
                throw new Error("Could not send information to a server");
            }
            navigate("/logged-in/cockpit");
        } catch {
            setError("Failed to confirm return, try again");
        }
    }

    return (
        <div>
            {error && <p style={{color: 'red'}}>{error}</p>}
            <form onSubmit={handleSubmit}>
                <div className="form-group">
                    <label className="block text-sm font-medium text-black">Actual start of the rent:</label>
                    <input
                        type="date"
                        name="ActualStartDate"
                        value={formData.ActualStartDate}
                        onChange={handleChange}
                        required={true}
                        className="mt-1 p-2 w-full border border-gray-300 rounded-md shadow-sm focus:outline-emerald-100 focus:ring-2 focus:ring-indigo-500"
                    />
                </div>
                <div className="form-group">
                    <label className="block text-sm font-medium text-black">Actual end of the rent:</label>
                    <input
                        type="date"
                        name="ActualStartDate"
                        value={formData.ActualEndDate}
                        onChange={handleChange}
                        required={true}
                        className="mt-1 p-2 w-full border border-gray-300 rounded-md shadow-sm focus:outline-emerald-100 focus:ring-2 focus:ring-indigo-500"
                    />
                </div>
                <div className="form-group">
                    <label className="block text-sm font-medium text-black">Description:</label>
                    <input
                        type="text"
                        name="Description"
                        value={formData.Description}
                        onChange={handleChange}
                        maxLength={200}
                        required={true}
                        className="mt-1 p-2 w-full border border-gray-300 rounded-md shadow-sm focus:outline-emerald-100 focus:ring-2 focus:ring-indigo-500"
                    />
                </div>
                <div className="form-group">
                    <label className="block text-sm font-medium text-black">Image:</label>
                    <input
                        type="file"
                        name="Image"
                        accept="image/*"
                        onChange={handleImageChange}
                    />
                    {formData.Preview && (
                        <div>
                            <p>Image Preview:</p>
                            <img
                                src={formData.Preview}
                                alt="Preview"
                                style={{ width: "200px" }}
                            />
                        </div>
                    )}
                </div>

            </form>
        </div>);
};

export default ConfirmReturnForm;