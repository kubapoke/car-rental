import {Rent} from "../Context/RentsContext.tsx";
import React from "react";

const CarRent: React.FC<{rent: Rent}> = ({rent}) => {
    return (
        <div className="p-4 border-b border-gray-300">
            <h3 className="text-lg font-semibold">{rent.car.model.brand.name} {rent.car.model.name}</h3>
            <p><strong>Status:</strong> {rent.status}</p>
            <p><strong>Start Date:</strong> {rent.rentStart}</p>
            <p><strong>End Date:</strong> {rent.rentEnd || "N/A"}</p>
        </div>
    );
};

export default CarRent;