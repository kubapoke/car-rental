import {Rent} from "../Context/RentsContext.tsx";
import React from "react";

const CarRent: React.FC<{rent: Rent}> = ({rent}) => {
    return (
        <div className="p-4 border-b border-gray-300">
            <h3 className="text-lg font-semibold">{rent.brand} {rent.model}</h3>
            <p><strong>Status:</strong> {rent.status}</p>
            <p><strong>Start Date:</strong> {rent.startDate}</p>
            <p><strong>End Date:</strong> {rent.endDate || "N/A"}</p>
        </div>
    );
};

export default CarRent;