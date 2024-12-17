import {useState, useEffect, FormEvent} from 'react';
import { FaSearch } from 'react-icons/fa';
import {useFilters} from "../Context/FiltersContext.tsx";
import LoginModal from "./LoginModal.tsx";
import {useAuth} from "../Context/AuthContext.tsx";
import {useNavigate} from "react-router-dom";
import {useOffers} from "../Context/OffersContext.tsx";

interface Car{
    modelName: string;
    brandName: string;
    location: string;
    isActive: boolean;
}

const SearchBar = () => {
    const {filters, setFilters}  = useFilters();
    const {isLoggedIn} = useAuth();
    const {setPage} = useOffers();
    const navigate = useNavigate();

    const [carData, setCarData] = useState<Car[]>([]);
    const [brands, setBrands] = useState<string[]>([]);
    const [models, setModels] = useState<string[]>([]);
    const [locations, setLocations] = useState<string[]>([]);

    const [selectedBrand, setSelectedBrand] = useState<string>(filters.selectedBrand);
    const [selectedModel, setSelectedModel] = useState<string>(filters.selectedModel);
    const [selectedLocation, setSelectedLocation] = useState<string>(filters.selectedLocation);
    const [startDate, setStartDate] = useState<string>(filters.startDate);
    const [endDate, setEndDate] = useState<string>(filters.endDate);

    const [showLoginModal, setShowLoginModal] = useState<boolean>(false);
    const [errors, setErrors] = useState<{ [key: string]: string }>({});

    useEffect(() => {
        // Fetch the car list data
        const fetchData = async () => {
            try {
                const response = await fetch(`${import.meta.env.VITE_SERVER_URL}/api/CarsForward/car-list`);
                const data: Car[] = await response.json();

                setCarData(data);

                // Extract unique brands and locations from the data
                const brands = [...new Set(data.map(car => car.brandName))];
                const locations = [...new Set(data.map(car => car.location))];

                setBrands(brands);
                setLocations(locations);
            } catch (error) {
                console.error('Error fetching car list:', error);
            }
        };

        fetchData();
    }, []);

    useEffect(() => {
        // update models based on selected brand
       if(selectedBrand) {
           const filteredModels = carData
               .filter(car => car.brandName === selectedBrand)
               .map(car => car.modelName);

           setModels([...new Set(filteredModels)]);

           if (!filteredModels.includes(selectedModel)) {
               setSelectedModel('');
           }
       } else {
            setModels([...new Set(carData.map(car => car.modelName))]);
       }
    }, [selectedBrand, carData, selectedModel]);

    const handleModelChange = (model: string) => {
        setSelectedModel(model);

        // set brand to brand of selected model
        const associatedBrand = carData.find(car => car.modelName === model)?.brandName;
        if(associatedBrand){
            setSelectedBrand(associatedBrand);
        }
    }

    const validateInput = () => {
        const newErrors: { [key: string]: string } = {};

        const today = new Date();
        today.setHours(0, 0, 0, 0);

        if (!startDate) {
            newErrors.startDate = "Start date is required.";
        } else if (new Date(startDate) < today) {
            newErrors.startDate = "Start date cannot be in the past.";
        }

        if (!endDate) {
            newErrors.endDate = "End date is required.";
        } else if (startDate && new Date(startDate) > new Date(endDate)) {
            newErrors.dateRange = "Start date must be before end date.";
        }

        setErrors(newErrors);
        return Object.keys(newErrors).length === 0;
    }

    const handleSubmit = (e : FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        if(!isLoggedIn) {
            setShowLoginModal(true);
            return;
        }

        if(!validateInput()) {
            return;
        }

        setPage(0);

        setFilters({
            selectedBrand,
            selectedModel,
            selectedLocation,
            startDate,
            endDate
        })

        navigate("/offers");
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
                        onChange={(e) => handleModelChange(e.target.value)}
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
            { showLoginModal && (
                <LoginModal
                    onClose={() => setShowLoginModal(false)}
                    onLoginSuccess={() => setShowLoginModal(false)}
                />) }
        </>
    );
};

export default SearchBar;
