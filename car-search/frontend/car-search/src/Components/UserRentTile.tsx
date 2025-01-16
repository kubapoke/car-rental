import { useState } from "react";
import { Rent, RentStatus, useRents } from "../Context/RentsContext.tsx";
import { FaCar } from "react-icons/fa";

const UserRentTile = ({ rent }: { rent: Rent }) => {
    const { returnCar } = useRents();
    const [isReturned, setIsReturned] = useState(rent.status === RentStatus.Returned);
    const [error, setError] = useState<string | null>(null);

    const handleReturn = async () => {
        try {
            const response = await returnCar(rent.rentId);
            if (!response.ok) {
                console.log('Car returned:', rent.rentId);
                rent.status = RentStatus.Returned;
                setIsReturned(true);
            } else {
                setError(`Failed to return car ${rent.rentId}`);
            }
        } catch (error) {
            setError(`Error returning car ${rent.rentId}, ${error}`);
        }
    }

    return (
        <div className={`rounded-xl shadow-md relative ${isReturned ? 'bg-gray-300' : 'bg-white'}`}>
            <div className="p-4">
                <div className="flex items-center justify-center">
                    <FaCar className="text-indigo-500 text-xl mr-3" />
                    <h3 className="text-xl font-bold">
                        {rent.brand} {rent.model} {isReturned && '(Returned)'}
                    </h3>
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
                        {!isReturned && (
                            <button
                                onClick={handleReturn}
                                className="bg-blue-500 text-white rounded-lg p-2 m-4 mb-8">
                                Return
                            </button>
                        )}
                    </div>
                </div>
            </div>
            {error && (
                <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50">
                    <div className="bg-white p-4 rounded-lg shadow-lg">
                        <h2 className="text-xl font-bold mb-4">Error</h2>
                        <p>{error}</p>
                        <button
                            onClick={() => setError(null)}
                            className="bg-red-500 text-white rounded-lg p-2 mt-4">
                            Close
                        </button>
                    </div>
                </div>
            )}
        </div>
    );
};

export default UserRentTile;