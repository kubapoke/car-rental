import {Rent} from "../Context/RentsContext.tsx";
import React from "react";
import {useNavigate} from "react-router-dom";

const CarRent: React.FC<{rent: Rent}> = ({rent}) => {
    const navigate = useNavigate();
    
    const handleConfirmReturn = () => {
        console.log(rent.id)
        navigate(`/logged-in/confirm-return/${rent.id}`, { state : {rent}})
    }
    return (
        <div className="p-4 border-b border-gray-300 flex items-center space-x-8">
            <div>
                <h3 className="text-lg font-semibold">{rent.car.model.brand.name} {rent.car.model.name}</h3>
                <p><strong>Status:</strong> {rent.status}</p>
                <p><strong>Start Date:</strong> {rent.rentStart}</p>
                <p><strong>End Date:</strong> {rent.rentEnd || "N/A"}</p>
            </div>
            {rent.status === "ReadyToReturn" && (
                <button 
                    className="bg-gray-500 text-white px-10 py-2 rounded-md hover:bg-gray-600"
                    onClick={handleConfirmReturn}>
                    Confirm return
                </button>
            )}
        </div>
    );
};

export default CarRent;