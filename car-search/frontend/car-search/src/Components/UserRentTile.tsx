import {FaCar} from "react-icons/fa";
import {Rent} from "../Context/RentsContext.tsx";

const UserRentTile = ({rent}: { rent: Rent }) => {
    return (
        <div className="bg-white rounded-xl shadow-md relative">
            <div className="p-4">
                <div className="flex items-center justify-center">
                    <FaCar className="text-indigo-500 text-xl mr-3"/>
                    <h3 className="text-xl font-bold">{rent.brand} {rent.model}</h3>
                </div>

                <div className="border border-gray-100 mb-5"></div>

                <div className="text-xl">
                    <h4 className="text-bold">Rented:</h4>
                    <p>from {rent.startDate.slice(0,10)}</p>
                    <p>to {rent.endDate.slice(0,10)}</p>
                </div>
            </div>
        </div>
    );
};

export default UserRentTile;