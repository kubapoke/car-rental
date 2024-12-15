import {FaCar} from "react-icons/fa";
import {Rent} from "../Context/RentsContext.tsx";

const UserRentTile = ({rent}: { rent: Rent }) => {
    function handleReturn() {
        
    }

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
                    <div className="flex items-center justify-between">
                        <div className="flex flex-col">
                            <div className="flex">
                                <span className="w-12">from</span>
                                <p>{rent.startDate.slice(0, 10)}</p>
                            </div>
                            <div className="flex">
                                <span className="w-12">to</span>
                                <p>{rent.endDate.slice(0, 10)}</p>
                            </div>
                        </div>
                        <button 
                            onClick={handleReturn}
                            className="bg-blue-500 text-white rounded-lg p-2 m-4 mb-8">Return</button>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default UserRentTile;