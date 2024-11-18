import SearchBar from "../Components/SearchBar.tsx";
import CarOffers from "../Components/CarOffers.tsx";
import {useFilters} from "../Context/FiltersContext.tsx";

const BrowseOffersPage = () => {
    const {filters, setFilters} = useFilters();

    return (
        <>
            <SearchBar updateFilters={setFilters} />
            <CarOffers isHome={false} filters={filters}/>
        </>
    );
};

export default BrowseOffersPage;