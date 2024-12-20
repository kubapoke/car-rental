import { Rent } from "../Context/RentsContext.tsx";
import React from "react";
import { useNavigate } from "react-router-dom";

const CarRent: React.FC<{ rent: Rent }> = ({ rent }) => {
    const navigate = useNavigate();

    const handleConfirmReturn = () => {
        console.log(rent.id);
        navigate(`/logged-in/confirm-return/${rent.id}`, { state: { rent } });
    };

    const handleViewReturnedRent = () => {
        navigate(`/logged-in/returned-rent/${rent.id}`, { state: { rent } });
    }

    return (
        <div className="p-6 border-b border-gray-300 flex items-center space-x-8">
            <div className="flex flex-col min-w-[200px]">
                <h3 className="text-xl font-semibold text-gray-800">{rent.car.model.brand.name} {rent.car.model.name}</h3>
                <p className="text-base text-gray-600"><strong>Status:</strong> {rent.status}</p>
                <p className="text-base text-gray-600"><strong>Start Date:</strong> {new Date(rent.rentStart).toLocaleDateString()}</p>
                <p className="text-base text-gray-600"><strong>End Date:</strong> {rent.rentEnd ? new Date(rent.rentEnd).toLocaleDateString() : "N/A"}</p>
            </div>

            <div className="flex flex-col items-start space-y-2">
                {rent.status === "ReadyToReturn" && (
                    <button
                        className="bg-black text-white px-6 py-2 rounded-md shadow-md hover:bg-gray-800 transition duration-200"
                        onClick={handleConfirmReturn}
                    >
                        Confirm Return
                    </button>
                )}

                {rent.status === "Returned" && (
                    <button
                        className="bg-black text-white px-6 py-2 rounded-md shadow-md cursor-not-allowed"
                        onClick={handleViewReturnedRent}
                    >
                        View Details
                    </button>
                )}
            </div>
        </div>
    );
};

export default CarRent;
