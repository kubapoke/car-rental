import React from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { Rent } from "../Context/RentsContext.tsx";

const ReturnedRentPage: React.FC = () => {
    const location = useLocation();
    const navigate = useNavigate();
    const { rent } = location.state as { rent: Rent };

    return (
        <div className="max-w-4xl mx-auto mt-8 p-6 bg-white shadow-md rounded-md">
            <h1 className="text-2xl font-bold text-gray-800 mb-4">Returned Rent Details</h1>
            <div className="border-t border-gray-300 pt-4">
                <h2 className="text-xl font-semibold text-gray-700">{rent.car.model.brand.name} {rent.car.model.name}</h2>
                <p className="text-gray-600"><strong>Status:</strong> {rent.status}</p>
                <p className="text-gray-600"><strong>Start Date:</strong> {new Date(rent.rentStart).toLocaleDateString()}</p>
                <p className="text-gray-600"><strong>End Date:</strong> {rent.rentEnd ? new Date(rent.rentEnd).toLocaleDateString() : "N/A"}</p>
                {rent.actualReturnDate && (
                    <p className="text-gray-600"><strong>Actual Return Date:</strong> {new Date(rent.actualReturnDate).toLocaleDateString()}</p>
                )}
            </div>

            {rent.description && (
                <div className="mt-4">
                    <h3 className="text-lg font-semibold text-gray-800">Return Description</h3>
                    <p className="text-gray-600">{rent.description}</p>
                </div>
            )}

            {rent.image && (
                <div className="mt-4">
                    <h3 className="text-lg font-semibold text-gray-800">Return Photo</h3>
                    <img
                        src={rent.image}
                        alt="Returned Car"
                        className="mt-2 w-full max-h-96 object-cover border border-gray-300 rounded-md shadow-sm"
                    />
                </div>
            )}

            <button
                onClick={() => navigate(-1)}
                className="mt-6 bg-black text-white px-6 py-2 rounded-md shadow-md hover:bg-gray-800 transition duration-200"
            >
                Go Back
            </button>
        </div>
    );
};

export default ReturnedRentPage;