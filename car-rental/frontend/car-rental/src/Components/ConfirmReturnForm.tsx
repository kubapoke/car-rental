import { Rent } from "../Context/RentsContext.tsx";
import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";

interface FormData {
    Id: string;
    ActualStartDate: string;
    ActualEndDate: string;
    Description: string;
    Image: File | null; // Store the uploaded file
}

const ConfirmReturnForm: React.FC<{ rent: Rent }> = ({ rent }) => {
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();
    const [preview, setPreview] = useState<string | null>(null);

    // Utility to extract date only (YYYY-MM-DD)
    const formatToDateInput = (dateString: string): string => {
        const date = new Date(dateString);
        return date.toISOString().split("T")[0];
    };

    const [formData, setFormData] = useState<FormData>({
        Id: rent.id,
        ActualStartDate: formatToDateInput(rent.rentStart),
        ActualEndDate: rent.rentEnd ? formatToDateInput(rent.rentEnd) : "",
        Description: "",
        Image: null,
    });

    useEffect(() => {
        return () => {
            if (preview) {
                URL.revokeObjectURL(preview);
            }
        };
    }, [preview]);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const handleImageChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const file = e.target.files?.[0];
        if (file) {
            setFormData({ ...formData, Image: file });
            setPreview(URL.createObjectURL(file));
        }
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            const token = sessionStorage.getItem("authToken");
            const dataToSend = new FormData();
            dataToSend.append("Id", String(Number(formData.Id)));
            dataToSend.append("ActualStartDate", new Date(formData.ActualStartDate).toISOString());
            dataToSend.append("ActualEndDate", new Date(formData.ActualEndDate).toISOString());
            dataToSend.append("Description", formData.Description);
            if (formData.Image) {
                dataToSend.append("Image", formData.Image);
            }
            const response = await fetch(`${import.meta.env.VITE_SERVER_URL}/api/Rents/close-rent`, {
                method: "POST",
                headers: {
                    ...(token && { Authorization: `Bearer ${token}` }),
                },
                body: dataToSend,
            });
            if (!response.ok) {
                throw new Error("Could not send information to the server");
            }
            navigate("/logged-in/cockpit");
        } catch {
            setError("Failed to confirm return, try again");
        }
    };

    return (
        <div className="max-w-lg mx-auto bg-white p-6 rounded-lg shadow-md">
            {error && <p className="text-red-500 text-center mb-4">{error}</p>}

            {/* Display Section */}
            <div className="mb-6">
                <h3 className="text-lg font-semibold text-black">
                    {rent.car.model.brand.name} {rent.car.model.name}
                </h3>
                <p className="text-sm text-gray-500">
                    <strong>Status:</strong> {rent.status}
                </p>
                <p className="text-sm text-gray-500">
                    <strong>Start Date:</strong> {formatToDateInput(rent.rentStart)}
                </p>
                <p className="text-sm text-gray-500">
                    <strong>End Date:</strong> {rent.rentEnd ? formatToDateInput(rent.rentEnd) : "N/A"}
                </p>
            </div>

            <form onSubmit={handleSubmit} className="space-y-4">
                <div className="form-group">
                    <label className="block text-sm font-medium text-gray-700">
                        Actual Start of the Rent:
                    </label>
                    <input
                        type="date"
                        name="ActualStartDate"
                        value={formData.ActualStartDate}
                        onChange={handleChange}
                        required
                        className="mt-1 p-2 w-full border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
                    />
                </div>
                <div className="form-group">
                    <label className="block text-sm font-medium text-gray-700">
                        Actual End of the Rent:
                    </label>
                    <input
                        type="date"
                        name="ActualEndDate"
                        value={formData.ActualEndDate}
                        onChange={handleChange}
                        required
                        className="mt-1 p-2 w-full border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
                    />
                </div>
                <div className="form-group">
                    <label className="block text-sm font-medium text-gray-700">Description:</label>
                    <input
                        type="text"
                        name="Description"
                        value={formData.Description}
                        onChange={handleChange}
                        maxLength={200}
                        required
                        className="mt-1 p-2 w-full border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
                    />
                </div>
                <div className="form-group">
                    <label className="block text-sm font-medium text-gray-700">Image:</label>
                    <input
                        type="file"
                        name="Image"
                        accept="image/*"
                        required
                        onChange={handleImageChange}
                        className="mt-1"
                    />
                    {preview && (
                        <div className="mt-2">
                            <p className="text-sm text-gray-500">Image Preview:</p>
                            <img
                                src={preview}
                                alt="Preview"
                                className="mt-1 border border-gray-300 rounded-md shadow-sm"
                                style={{ maxWidth: "100%", height: "auto" }}
                            />
                        </div>
                    )}
                </div>
                <div className="form-group flex justify-center">
                    <button
                        type="submit"
                        className="w-full py-2 bg-black text-white rounded-md shadow-md hover:bg-gray-800 focus:outline-none focus:ring-2 focus:ring-black"
                    >
                        Submit
                    </button>
                </div>
            </form>
        </div>
    );
};

export default ConfirmReturnForm;
