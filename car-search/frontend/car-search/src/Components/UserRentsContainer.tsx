import {useEffect} from "react";
import UserRentTile from "./UserRentTile.tsx";
import {useRents} from "../Context/RentsContext.tsx";

const UserRentsContainer = () => {
    const {rents, setRents} = useRents();

    useEffect(() => {
        const fetchRents = async () => {
            // Retrieve the authToken from session storage
            const token = sessionStorage.getItem('authToken'); // or use sessionStorage.getItem('jwt_token')

            // If the token is present, add it to the Authorization header
            const headers = {
                'Content-Type': 'application/json',
                ...(token && { 'Authorization': `Bearer ${token}` }), // Add the Authorization header if the token exists
            };

            const apiUrl = `${import.meta.env.VITE_SERVER_URL}/api/Rents/get-user-rents`;

            try {
                const res = await fetch(apiUrl, {method: 'GET', headers: headers});
                const data = await res.json();
                setRents(data);
            }
            catch (error){
                console.log('Error fetching data', error);
            }
        }

        fetchRents();
    }, [rents, setRents]);
    
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