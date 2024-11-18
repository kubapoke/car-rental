import Hero from "../Components/Hero.tsx";
import SearchBar from "../Components/SearchBar.tsx";
import CarOffers from "../Components/CarOffers.tsx";
import MoreOffersButton from "../Components/MoreOffersButton.tsx";
import {useState} from "react";

const HomePage = () => {
    const [filters, setFilters] = useState({
        selectedBrand: '',
        selectedModel: '',
        selectedLocation: '',
        startDate: '',
        endDate: '',
    });

    return (
        <>
            <Hero/>
            <SearchBar updateFilters={setFilters}/>
            <CarOffers isHome={true} filters={filters}/>
            <MoreOffersButton/>
        </>
    );
};

export default HomePage;