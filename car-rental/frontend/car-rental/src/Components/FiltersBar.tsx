import {FormEvent, useEffect, useState} from "react";
import {FaSearch} from "react-icons/fa";
import {rentStatus, useFilters} from "../Context/FiltersContext.tsx";
import {Rent} from "../Context/RentsContext.tsx";

const FiltersBar = () => {
    const {filters, setFilters} = useFilters();
    const [, setRentsData] = useState<Rent[]>([]);
    
    
    const [selectedRentStatus, setSelectedRentStatus] = useState<rentStatus>(filters.selectedRentStatus);

    useEffect(() => {
        // Fetch the rent list data
        const fetchData = async () => {
            console.log("fetching data")

            const token = sessionStorage.getItem('authToken');
            const headers = {
                'Content-Type': 'application/json',
                ...(token && { Authorization: `Bearer ${token}` }),
            };
            
            const query = new URLSearchParams({
                rentStatus: filters.selectedRentStatus,
            }).toString();

            const apiUrl = `${import.meta.env.VITE_SERVER_URL}/api/Rents/get-rents?${query}`;
            try {
                const response = await fetch(apiUrl, {method: 'GET', headers: headers});
                const data: Rent[] = await response.json();
                
                console.log("hello");
                
                setRentsData(data);

                console.log('Rentals:', data);
            } catch (error) {
                console.error('Error fetching car list:', error);
            }
        };

        fetchData();
    }, [filters]);
    
    const handleSubmit = (e : FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        setFilters({
            selectedRentStatus: selectedRentStatus,
        })
    };
    
    
    return (
        <>
            <form
                className="flex flex-col md:flex-row items-center p-4 bg-white rounded-lg shadow-md"
                onSubmit={handleSubmit}
            >
                <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 flex justify-between">
                    {/* Brand Dropdown */}
                    <select
                        value={selectedRentStatus}
                        onChange={(e) => setSelectedRentStatus(e.target.value as rentStatus)}
                        className="border border-gray-300 rounded-lg p-2 mb-2 md:mb-0 md:mr-2 w-full md:w-1/3"
                    >
                        <option value="">Select Status</option>
                        {Object.values(rentStatus).map((status) => (
                            <option key={status} value={status}>
                                {status}
                            </option>
                        ))}
                    </select>

                    <button
                        type="submit"
                        className="bg-black text-white rounded-lg p-2 mt-2 md:mt-0 md:ml-2"
                    >
                        <FaSearch className={'inline text-lg mb-1'}/> <br/> Apply Filters
                    </button>

                </div>
            </form>
        </>
    );
};

export default FiltersBar;