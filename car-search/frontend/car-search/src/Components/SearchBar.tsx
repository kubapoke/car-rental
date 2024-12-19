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
    const {page, setPage, pageSize} = useOffers();
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
    const {fetchOffers} = useOffers();

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
            newErrors.endDate = "End date must be after start date.";
        }

        setErrors(newErrors);
        return Object.keys(newErrors).length === 0;
    }

    const handleSubmit = async (e : FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        if(!isLoggedIn) {
            setShowLoginModal(true);
            return;
        }

        if(!validateInput()) {
            return;
        }

        const newFilters = {
            selectedBrand,
            selectedModel,
            selectedLocation,
            startDate,
            endDate
        };

        setPage(0);
        setFilters(newFilters);

        await fetchOffers(
            {
                brand: newFilters.selectedBrand || "",
                model: newFilters.selectedModel || "",
                location: newFilters.selectedLocation || "",
                startDate: newFilters.startDate || "",
                endDate: newFilters.endDate || "",
            },
            page, pageSize
        );

        navigate("/offers");
    };

    return (
        <>
            <form
                className="flex flex-wrap items-center p-4 bg-white rounded-lg shadow-md"
                onSubmit={handleSubmit}
            >
                <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 flex gap-4 flex-nowrap overflow-x-auto">
                    {/* Brand Dropdown */}
                    <div className="flex flex-col w-1/5 max-w-[150px] min-w-[150px]">
                        <select
                            value={selectedBrand}
                            onChange={(e) => setSelectedBrand(e.target.value)}
                            className="border border-gray-300 rounded-lg py-4 px-4 text-base h-[64px] w-full"
                        >
                            <option value="">Select Brand</option>
                            {brands.map((brand) => (
                                <option key={brand} value={brand}>
                                    {brand}
                                </option>
                            ))}
                        </select>
                        <span className="text-red-500 text-sm mt-1 min-h-[1.5rem] break-words w-full">
                {errors.selectedBrand}
            </span>
                    </div>

                    {/* Model Dropdown */}
                    <div className="flex flex-col w-1/5 max-w-[150px] min-w-[150px]">
                        <select
                            value={selectedModel}
                            onChange={(e) => handleModelChange(e.target.value)}
                            className="border border-gray-300 rounded-lg py-4 px-4 text-base h-[64px] w-full"
                        >
                            <option value="">Select Model</option>
                            {models.map((model) => (
                                <option key={model} value={model}>
                                    {model}
                                </option>
                            ))}
                        </select>
                        <span className="text-red-500 text-sm mt-1 min-h-[1.5rem] break-words w-full">
                            {errors.selectedModel}
                        </span>
                    </div>

                    {/* Location Dropdown */}
                    <div className="flex flex-col w-1/5 max-w-[150px] min-w-[150px]">
                        <select
                            value={selectedLocation}
                            onChange={(e) => setSelectedLocation(e.target.value)}
                            className="border border-gray-300 rounded-lg py-4 px-4 text-base h-[64px] w-full"
                        >
                            <option value="">Select Location</option>
                            {locations.map((location) => (
                                <option key={location} value={location}>
                                    {location}
                                </option>
                            ))}
                        </select>
                        <span className="text-red-500 text-sm mt-1 min-h-[1.5rem] break-words w-full">
                            {errors.selectedLocation}
                        </span>
                    </div>

                    {/* Start Date Picker */}
                    <div className="flex flex-col w-1/5 max-w-[150px] min-w-[150px]">
                        <input
                            type="date"
                            value={startDate}
                            onChange={(e) => setStartDate(e.target.value)}
                            className="border border-gray-300 rounded-lg py-4 px-4 text-base h-[64px] w-full"
                        />
                        <span className="text-red-500 text-sm mt-1 min-h-[1.5rem] break-words w-full">
                            {errors.startDate}
                        </span>
                    </div>

                    {/* End Date Picker */}
                    <div className="flex flex-col w-1/5 max-w-[150px] min-w-[150px]">
                        <input
                            type="date"
                            value={endDate}
                            onChange={(e) => setEndDate(e.target.value)}
                            className="border border-gray-300 rounded-lg py-4 px-4 text-base h-[64px] w-full"
                        />
                        <span className="text-red-500 text-sm mt-1 min-h-[1.5rem] break-words w-full">
                            {errors.endDate}
                        </span>
                    </div>

                    {/* Search Button */}
                    <div className="flex flex-col w-1/5 max-w-[150px] min-w-[150px] z-2">
                        <button
                            type="submit"
                            className="bg-blue-500 text-white rounded-lg py-4 px-6 text-base h-[64px]"
                        >
                            <FaSearch className="inline text-lg mb-1"/> Search
                        </button>
                        <span className="text-red-500 text-sm mt-1 min-h-[1.5rem]"> </span> {/* Dummy error space */}
                    </div>
                </div>
            </form>

            {showLoginModal && (
                <LoginModal
                    onClose={() => setShowLoginModal(false)}
                    onLoginSuccess={() => setShowLoginModal(false)}
                />)}
        </>
    );
};

export default SearchBar;
