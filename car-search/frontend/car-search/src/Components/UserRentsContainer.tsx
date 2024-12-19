import {useEffect} from "react";
import UserRentTile from "./UserRentTile.tsx";
import {useRents} from "../Context/RentsContext.tsx";

const UserRentsContainer = () => {
    const {rents, setRents, fetchRents} = useRents();

    useEffect(() => {
        fetchRents();
    }, [setRents]);
    
    // maybe slice it to a couple of first records in the future
    const displayedRents =  rents;
    // const numberDisplayed = 3;
    
    return (
        <section className="bg-blue-50 px-4 py-10">
            <div className="container-xl lg:container m-auto">
                <h2 className="text-3xl font-bold text-indigo-500 mb-6 text-center">
                    Your rents:
                </h2>
                <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                    {displayedRents.map((rent) => (
                        <UserRentTile rent={rent}/>
                    ))}
                </div>
            </div>
        </section>
    );
};

export default UserRentsContainer;