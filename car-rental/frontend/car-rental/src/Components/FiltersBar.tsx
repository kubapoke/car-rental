import {FormEvent, useEffect, useState} from "react";
import {FaSearch} from "react-icons/fa";

const CarStatuses = ["Active", "Ready to return"];
const FiltersBar = () => {
    const [carStatus, setCarStatus] = useState<string>();
    const [selectedLocation, setSelectedLocation] = useState<string>();
    const [locations, setLocations] = useState<string[]>([]);

    useEffect(() => {
        // Fetch the rent list data
        const fetchData = async () => {
            try {
                //TODO: acctualy implement this
                
                const locations = ["Warsaw", "Bydgoszcz"];
                
                setLocations(locations);
            } catch (error) {
                console.error('Error fetching car list:', error);
            }
        };

        fetchData();
    }, []);
    
    const handleSubmit = (e : FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        console.log(e)
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
                        value={carStatus}
                        onChange={(e) => setCarStatus(e.target.value)}
                        className="border border-gray-300 rounded-lg p-2 mb-2 md:mb-0 md:mr-2 w-full md:w-1/3"
                    >
                        <option value="">Select Status</option>
                        {CarStatuses.map((brand) => (
                            <option key={brand} value={brand}>
                                {brand}
                            </option>
                        ))}
                    </select>

                    {/* Location Dropdown */}
                    <select
                        value={selectedLocation}
                        onChange={(e) => setSelectedLocation(e.target.value)}
                        className="border border-gray-300 rounded-lg p-2 mb-2 md:mb-0 md:mr-2 w-full md:w-1/3"
                    >
                        <option value="">Select Location</option>
                        {locations.map((location) => (
                            <option key={location} value={location}>
                                {location}
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