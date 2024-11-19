import Hero from "../Components/Hero.tsx";
import SearchBar from "../Components/SearchBar.tsx";
import CarOffers from "../Components/CarOffers.tsx";
import MoreOffersButton from "../Components/MoreOffersButton.tsx";

const HomePage = () => {
    return (
        <>
            <Hero/>
            <SearchBar/>
            <CarOffers isHome={true}/>
            <MoreOffersButton/>
        </>
    );
};

export default HomePage;