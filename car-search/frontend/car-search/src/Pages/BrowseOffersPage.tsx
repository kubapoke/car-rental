import SearchBar from "../Components/SearchBar.tsx";
import CarOffers from "../Components/CarOffers.tsx";

const BrowseOffersPage = () => {
    return (
        <>
            <SearchBar/>
            <CarOffers isHome={false}/>
        </>
    );
};

export default BrowseOffersPage;