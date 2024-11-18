import {useState, useEffect, FormEvent} from 'react';
import { FaSearch } from 'react-icons/fa';
import {useFilters} from "../Context/FiltersContext.tsx";

interface Car{
    modelName: string;
    brandName: string;
    location: string;
    isActive: boolean;
}

interface Filters {
    selectedBrand: string;
    selectedModel: string;
    selectedLocation: string;
    startDate: string;
    endDate: string;
}

interface SearchBarProps {
    updateFilters: (filters: Filters) => void;
}

const SearchBar = ({updateFilters} : SearchBarProps) => {
    const filters  = useFilters().filters;

    const [brands, setBrands] = useState<string[]>([]);
    const [models, setModels] = useState<string[]>([]);
    const [locations, setLocations] = useState<string[]>([]);

    const [selectedBrand, setSelectedBrand] = useState<string>(filters.selectedBrand);
    const [selectedModel, setSelectedModel] = useState<string>(filters.selectedModel);
    const [selectedLocation, setSelectedLocation] = useState<string>(filters.selectedLocation);
    const [startDate, setStartDate] = useState<string>(filters.startDate);
    const [endDate, setEndDate] = useState<string>(filters.endDate);

    useEffect(() => {
        // Fetch the car list data
        const fetchData = async () => {
            try {
                const response = await fetch(`${import.meta.env.VITE_SERVER_URL}/api/CarsForward/car-list`);
                const data: Car[] = await response.json();

                // Extract unique brands, models, and locations from the data
                const brands = [...new Set(data.map(car => car.brandName))];
                const models = [...new Set(data.map(car => car.modelName))];
                const locations = [...new Set(data.map(car => car.location))];

                setBrands(brands);
                setModels(models);
                setLocations(locations);
            } catch (error) {
                console.error('Error fetching car list:', error);
            }
        };

        fetchData();
    }, []);

    const handleSubmit = (e : FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        console.log({ selectedBrand, selectedModel, selectedLocation, startDate, endDate });

        updateFilters({
            selectedBrand,
            selectedModel,
            selectedLocation,
            startDate,
            endDate
        })
    };

    return (
        <form
            className="flex flex-col md:flex-row items-center p-4 bg-white rounded-lg shadow-md"
            onSubmit={handleSubmit}
        >
            <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 flex justify-between">
                {/* Brand Dropdown */}
                <select
                    value={selectedBrand}
                    onChange={(e) => setSelectedBrand(e.target.value)}
                    className="border border-gray-300 rounded-lg p-2 mb-2 md:mb-0 md:mr-2 w-full md:w-1/3"
                >
                    <option value="">Select Brand</option>
                    {brands.map((brand) => (
                        <option key={brand} value={brand}>
                            {brand}
                        </option>
                    ))}
                </select>

                {/* Model Dropdown */}
                <select
                    value={selectedModel}
                    onChange={(e) => setSelectedModel(e.target.value)}
                    className="border border-gray-300 rounded-lg p-2 mb-2 md:mb-0 md:mr-2 w-full md:w-1/3"
                >
                    <option value="">Select Model</option>
                    {models.map((model) => (
                        <option key={model} value={model}>
                            {model}
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

                {/* Start Date Picker */}
                <input
                    type="date"
                    value={startDate}
                    onChange={(e) => setStartDate(e.target.value)}
                    className="border border-gray-300 rounded-lg p-2 mb-2 md:mb-0 md:mr-2 w-full md:w-1/3"
                />

                {/* End Date Picker */}
                <input
                    type="date"
                    value={endDate}
                    onChange={(e) => setEndDate(e.target.value)}
                    className="border border-gray-300 rounded-lg p-2 mb-2 md:mb-0 md:mr-2 w-full md:w-1/3"
                />

                {/* Search Button */}
                <button
                    type="submit"
                    className="bg-blue-500 text-white rounded-lg p-2 mt-2 md:mt-0 md:ml-2"
                >
                    <FaSearch className={'inline text-lg mb-1'} /> Search
                </button>
            </div>
        </form>
    );
};

export default SearchBar;
