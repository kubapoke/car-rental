import SearchBar from "../Components/SearchBar.tsx";
import CarOffers from "../Components/CarOffers.tsx";
import {useState} from "react";

const BrowseOffersPage = () => {
    const [filters, setFilters] = useState({
        selectedBrand: '',
        selectedModel: '',
        selectedLocation: '',
        startDate: '',
        endDate: '',
    });

    return (
        <>
            <SearchBar updateFilters={setFilters} />
            <CarOffers isHome={false} filters={filters}/>
        </>
    );
};

export default BrowseOffersPage;